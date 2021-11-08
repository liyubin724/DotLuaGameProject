using DotEngine.Pool;
using System;
using System.Collections.Generic;
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
            if(client !=null)
            {
                client.Dispose();
                client = null;
            }
            client = new SimpleClient(ip, port);
            return client.Connect();
        }

        public bool Reconnect()
        {
            if(client == null)
            {
                Debug.LogError("");
                return false;
            }

            return client.ReconnectAsync();
        }

        public bool Disconnect()
        {
            if(client!=null)
            {
                return client.DisconnectAsync();
            }

            return false;
        }

        public bool SendEmptyMessage(int messageId)
        {
            return SendMessage(messageId, null);
        }

        public bool SendMessage(int messageId,byte[] messageBody)
        {
            if(IsConnected)
            {
                byte[] dataBytes = messageEncoder.Encode(messageId, messageBody, IsMessageNeedEncrypt(messageId), IsMessageNeedCompress(messageId));

                return client.SendMessage(dataBytes);
            }

            return false;
        }

        public bool SendText(int messageId,string messageText)
        {
            return SendMessage(messageId, Encoding.Unicode.GetBytes(messageText));
        }

        private bool IsMessageNeedEncrypt(int messageId)
        {
            return false;
        }

        private bool IsMessageNeedCompress(int messageId)
        {
            return false;
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
                        State = ClientState.Normal;
                        NetConnected?.Invoke();
                    }else if(targetState == ClientState.Disconnected)
                    {
                        NetDisconnected?.Invoke();
                    }else if(targetState == ClientState.Error)
                    {
                        NetError?.Invoke();
                    }
                }
            }

            if(!IsConnected)
            {
                return;
            }

            if(State == ClientState.Error)
            {
                Disconnect();
                return;
            }

            lock (receviedMessageList)
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

        public void UnRegisterListener(int messageId)
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

        public void UnRegisterAll()
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
