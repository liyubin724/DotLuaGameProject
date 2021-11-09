using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace DotEngine.Net
{
    public delegate void ClientStateListener();
    public delegate void ClientMessageListener(int messageId, byte[] messageBody);

    class ClientReceviedMessage : IPoolItem
    {
        public int Id { get; set; } = -1;
        public byte[] Body { get; set; } = null;

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            Id = -1;
            Body = null;
        }
    }

    public class ClientNetwork : IClientHandler
    {
        private SimpleClient client = null;
        private Dictionary<int, List<ClientMessageListener>> messageListenerDic = new Dictionary<int, List<ClientMessageListener>>();

        public int decodeMessageMaxCount = int.MaxValue;
        public int DecodeMessageMaxCount
        {
            get
            {
                return decodeMessageMaxCount;
            }
            set
            {
                if (value <= 0)
                {
                    decodeMessageMaxCount = int.MaxValue;
                }
                else
                {
                    decodeMessageMaxCount = value;
                }
            }
        }

        public bool IsConnnecting
        {
            get
            {
                return client != null && client.IsConnecting;
            }
        }
        public bool IsConnected
        {
            get
            {
                return client != null && client.IsConnected;
            }
        }

        public bool AutoDisconnectedWhenError { get; set; } = true;

        private IMessageEncoder messageEncoder;
        private IMessageDecoder messageDecoder;

        private object stateLocker = new object();
        public ClientState State { get; private set; } = ClientState.Unreachable;
        private ClientState targetState = ClientState.Unreachable;

        private ItemPool<ClientReceviedMessage> receviedMessagePool = new ItemPool<ClientReceviedMessage>();
        private List<ClientReceviedMessage> receviedMessageList = new List<ClientReceviedMessage>();

        public ClientStateListener NetConnected;
        public ClientStateListener NetDisconnected;
        public ClientStateListener NetError;

        public ClientNetwork(IMessageEncoder encoder,IMessageDecoder decoder)
        {
            messageEncoder = encoder;
            messageDecoder = decoder;
        }

        public bool Connect(string ip,int port)
        {
            if(client!=null)
            {
                Debug.LogError("The network has been created.");
                return false;
            }

            client = new SimpleClient(ip, port);
            client.Handler = this;
            
            return client.ConnectAsync();
        }

        public bool Reconnect()
        {
            if(client == null)
            {
                Debug.LogError("The network hasn't been created");
                return false;
            }
            if(client.IsConnecting || client.IsConnected)
            {
                Debug.LogError("The network is connecting");
                return false;
            }

            return client.ConnectAsync();
        }

        public bool Disconnect()
        {
            if(client == null)
            {
                Debug.LogError("The network hasn't been created");
                return false;
            }

            return client.DisconnectAsync();
        }

        public void Dispose()
        {
            if(client!=null)
            {
                client.Dispose();
                client = null;
            }
        }

        public bool SendEmptyMessage(int messageId)
        {
            return SendMessage(messageId, null);
        }

        public bool SendMessage(int messageId,byte[] messageBody,bool needEncrypt = false,bool needCompress = false)
        {
            if(IsConnected)
            {
                byte[] dataBytes = messageEncoder.Encode(messageId, messageBody, needEncrypt, needCompress);
                return client.SendMessage(dataBytes);
            }

            return false;
        }

        public bool SendText(int messageId,string messageText)
        {
            return SendMessage(messageId, Encoding.Unicode.GetBytes(messageText));
        }

        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {
            if(client == null)
            {
                return;
            }

            lock(stateLocker)
            {
                if(State!=targetState)
                {
                    State = targetState;
                    if(targetState == ClientState.Connected)
                    {
                        NetConnected?.Invoke();
                    }else if(targetState == ClientState.Disconnected)
                    {
                        NetDisconnected?.Invoke();
                    }else if(targetState == ClientState.Error)
                    {
                        NetError?.Invoke();
                        if(AutoDisconnectedWhenError)
                        {
                            Disconnect();
                        }
                    }
                }
            }

            lock (receviedMessageList)
            {
                if(receviedMessageList.Count>0)
                {
                    if(IsConnected)
                    {
                        int maxCount = Math.Min(receviedMessageList.Count, DecodeMessageMaxCount);
                        for (int i = 0; i < maxCount; ++i)
                        {
                            ClientReceviedMessage message = receviedMessageList[0];
                            receviedMessageList.RemoveAt(0);

                            if (messageListenerDic.TryGetValue(message.Id, out var list))
                            {
                                foreach (var listener in list)
                                {
                                    listener(message.Id, message.Body);
                                }
                            }

                            receviedMessagePool.Release(message);
                        }
                    }else
                    {
                        foreach (var message in receviedMessageList)
                        {
                            receviedMessagePool.Release(message);
                        }
                        receviedMessageList.Clear();
                    }
                }

            }
        }

        #region message listener
        public void RegisterListener(int messageId, ClientMessageListener listener)
        {
            if (!messageListenerDic.TryGetValue(messageId, out var list))
            {
                list = ListPool<ClientMessageListener>.Get();
                messageListenerDic.Add(messageId, list);
            }

            list.Add(listener);
        }

        public void RegisterListener(Type targetType)
        {
            MethodInfo[] methodInfos = targetType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach(var mInfo in methodInfos)
            {
                ParameterInfo[] parameterInfos = mInfo.GetParameters();
                if(parameterInfos.Length == 2 && 
                    parameterInfos[0].ParameterType == typeof(int) && 
                    parameterInfos[1].ParameterType == typeof(byte[]) )
                {
                    var attr = mInfo.GetCustomAttribute<CustomMessageListenerAttribute>(true);
                    if(attr!=null && attr.ListenerType == MessageListenerType.Client && attr.MessageId>0)
                    {
                        RegisterListener(attr.MessageId, (messageId, messageBody) =>
                        {
                            mInfo.Invoke(null, new object[] { messageId, messageBody });
                        });
                    }
                }
            }
        }

        public void UnregisterListener(Type targetType)
        {
            MethodInfo[] methodInfos = targetType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var mInfo in methodInfos)
            {
                ParameterInfo[] parameterInfos = mInfo.GetParameters();
                if (parameterInfos.Length == 2 &&
                    parameterInfos[0].ParameterType == typeof(int) &&
                    parameterInfos[1].ParameterType == typeof(byte[]))
                {
                    var attr = mInfo.GetCustomAttribute<CustomMessageListenerAttribute>(true);
                    if (attr != null && attr.ListenerType == MessageListenerType.Client && attr.MessageId > 0)
                    {
                        UnregisterListener(attr.MessageId);
                    }
                }
            }
        }

        public void UnregisterListener(int messageId)
        {
            if (messageListenerDic.TryGetValue(messageId, out var list))
            {
                messageListenerDic.Remove(messageId);
                ListPool<ClientMessageListener>.Release(list);
            }
        }

        public void UnregisterListener(int messageId, ClientMessageListener listener)
        {
            if (messageListenerDic.TryGetValue(messageId, out var list))
            {
                if (list.Remove(listener))
                {
                    if (list.Count == 0)
                    {
                        messageListenerDic.Remove(messageId);
                        ListPool<ClientMessageListener>.Release(list);
                    }
                }
            }
        }

        public void UnregisterAll()
        {
            foreach (var kvp in messageListenerDic)
            {
                ListPool<ClientMessageListener>.Release(kvp.Value);
            }
            messageListenerDic.Clear();
        }
        #endregion

        public void OnStateChanged(ClientState state)
        {
            lock(stateLocker)
            {
                targetState = state;
            }
        }

        public void OnMessageReceived(byte[] dataBytes)
        {
            lock(receviedMessageList)
            {
                if(messageDecoder.Decode(dataBytes,out int messageId,out byte[] body))
                {
                    ClientReceviedMessage receviedMessage = receviedMessagePool.Get();
                    receviedMessage.Id = messageId;
                    receviedMessage.Body = body;
                    receviedMessageList.Add(receviedMessage);
                }else
                {
                    Debug.LogError("");
                }
            }
        }
    }
}
