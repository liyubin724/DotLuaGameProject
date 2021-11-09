using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;

namespace DotEngine.Net
{
    public delegate void ServerStateListener();
    public delegate void ServerMessageListener(Guid guid, int messageId, byte[] messageBody);

    internal class ServerReceivedMessage : IPoolItem
    {
        public Guid SessionId { get; set; } = Guid.Empty;
        public int MessageId { get; set; } = -1;
        public byte[] MessageBody { get; set; } = null;

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            SessionId = Guid.Empty;
            MessageId = -1;
            MessageBody = null;
        }
    }

    public class ServerNetwork : IServerHandler
    {
        private SimpleServer server = null;
        private Dictionary<int, List<ServerMessageListener>> messageListenerDic = new Dictionary<int, List<ServerMessageListener>>();

        private IMessageEncoder messageEncoder;
        private IMessageDecoder messageDecoder;

        private object stateLocker = new object();
        public ServerState State { get; private set; } = ServerState.Unreachable;
        private ServerState targetState = ServerState.Unreachable;

        private ItemPool<ServerReceivedMessage> receviedMessagePool = new ItemPool<ServerReceivedMessage>();
        private List<ServerReceivedMessage> receviedMessageList = new List<ServerReceivedMessage>();

        public ServerStateListener NetStarted;
        public ServerStateListener NetStopped;
        public ServerStateListener NetError;

        public bool IsStarted
        {
            get
            {
                return server != null && server.IsStarted;
            }
        }

        public ServerNetwork(IMessageEncoder encoder, IMessageDecoder decoder)
        {
            messageEncoder = encoder;
            messageDecoder = decoder;
        }

        public bool Start(int port)
        {
            if (server != null)
            {
                server.Handler = null;
                server.Stop();
                server = null;
            }

            server = new SimpleServer(IPAddress.Any, port);
            server.Handler = this;
            return server.Start();
        }

        public bool Stop()
        {
            if (server == null)
            {
                return false;
            }
            return server.Stop();
        }

        public bool MulticastMessage(int messageId, byte[] messageBody)
        {
            if (!IsStarted)
            {
                return false;
            }
            byte[] dataBytes = messageEncoder.Encode(messageId, messageBody, IsMessageNeedEncrypt(messageId), IsMessageNeedCompress(messageId));
            return server.MulticastMessage(dataBytes);
        }

        public bool MulticastText(int messageId, string messageText)
        {
            return MulticastMessage(messageId, Encoding.Unicode.GetBytes(messageText));
        }

        public bool MulticastMessageTo(Guid guid, int messageId, byte[] messageBody)
        {
            if (!IsStarted)
            {
                return false;
            }
            byte[] dataBytes = messageEncoder.Encode(messageId, messageBody, IsMessageNeedEncrypt(messageId), IsMessageNeedCompress(messageId));
            return server.MulticastMessageTo(guid, dataBytes);
        }

        public bool MulticastMessageTo(Guid guid, int messageId, string messageText)
        {
            return MulticastMessageTo(guid, messageId, Encoding.Unicode.GetBytes(messageText));
        }

        private bool IsMessageNeedEncrypt(int messageId)
        {
            return false;
        }

        private bool IsMessageNeedCompress(int messageId)
        {
            return false;
        }

        public void OnMessageReceived(Guid guid, byte[] dataBytes)
        {
            if (messageDecoder.Decode(dataBytes, out int messageId, out byte[] messageBody))
            {
                lock (receviedMessageList)
                {
                    ServerReceivedMessage receviedMessage = receviedMessagePool.Get();
                    receviedMessage.SessionId = guid;
                    receviedMessage.MessageId = messageId;
                    receviedMessage.MessageBody = messageBody;
                    receviedMessageList.Add(receviedMessage);
                }
            }
            else
            {
                server.DisconnectSession(guid);
            }
        }

        public void OnStateChanged(ServerState state)
        {
            lock (stateLocker)
            {
                targetState = state;
            }
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            if (server == null)
            {
                return;
            }
            lock (stateLocker)
            {
                if (State != targetState)
                {
                    State = targetState;
                    if (targetState == ServerState.Started)
                    {
                        NetStarted?.Invoke();
                    }
                    else if (targetState == ServerState.Stoped)
                    {
                        NetStarted?.Invoke();
                    }
                    else if (targetState == ServerState.Error)
                    {
                        NetError?.Invoke();
                    }
                }
            }
            if (!IsStarted)
            {
                return;
            }
            lock (receviedMessageList)
            {
                while (receviedMessageList.Count > 0)
                {
                    ServerReceivedMessage message = receviedMessageList[0];
                    receviedMessageList.RemoveAt(0);

                    if (messageListenerDic.TryGetValue(message.MessageId, out var list))
                    {
                        foreach (var listener in list)
                        {
                            listener(message.SessionId, message.MessageId, message.MessageBody);
                        }
                    }

                    receviedMessagePool.Release(message);
                }
            }
        }

        #region message listener
        public void RegisterListener(int messageId, ServerMessageListener listener)
        {
            if (!messageListenerDic.TryGetValue(messageId, out var list))
            {
                list = ListPool<ServerMessageListener>.Get();
                messageListenerDic.Add(messageId, list);
            }

            list.Add(listener);
        }

        public void UnregisterListener(int messageId)
        {
            if (messageListenerDic.TryGetValue(messageId, out var list))
            {
                messageListenerDic.Remove(messageId);
                ListPool<ServerMessageListener>.Release(list);
            }
        }

        public void UnregisterListener(int messageId, ServerMessageListener listener)
        {
            if (messageListenerDic.TryGetValue(messageId, out var list))
            {
                if (list.Remove(listener))
                {
                    if (list.Count == 0)
                    {
                        messageListenerDic.Remove(messageId);
                        ListPool<ServerMessageListener>.Release(list);
                    }
                }
            }
        }

        public void RegisterListener(Type targetType)
        {
            MethodInfo[] methodInfos = targetType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var mInfo in methodInfos)
            {
                ParameterInfo[] parameterInfos = mInfo.GetParameters();
                if (parameterInfos.Length == 3 &&
                    parameterInfos[0].ParameterType == typeof(Guid) &&
                    parameterInfos[1].ParameterType == typeof(int) &&
                    parameterInfos[2].ParameterType == typeof(byte[]))
                {
                    var attr = mInfo.GetCustomAttribute<CustomMessageListenerAttribute>(true);
                    if (attr != null && attr.ListenerType == MessageListenerType.Server && attr.MessageId > 0)
                    {
                        RegisterListener(attr.MessageId, (guid, messageId, messageBody) =>
                        {
                            mInfo.Invoke(null, new object[] { guid, messageId, messageBody });
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
                if (parameterInfos.Length == 3 &&
                    parameterInfos[0].ParameterType == typeof(Guid) &&
                    parameterInfos[1].ParameterType == typeof(int) &&
                    parameterInfos[2].ParameterType == typeof(byte[]))
                {
                    var attr = mInfo.GetCustomAttribute<CustomMessageListenerAttribute>(true);
                    if (attr != null && attr.ListenerType == MessageListenerType.Client && attr.MessageId > 0)
                    {
                        UnregisterListener(attr.MessageId);
                    }
                }
            }
        }

        public void UnRegisterAll()
        {
            foreach (var kvp in messageListenerDic)
            {
                ListPool<ServerMessageListener>.Release(kvp.Value);
            }
            messageListenerDic.Clear();
        }
        #endregion
    }
}
