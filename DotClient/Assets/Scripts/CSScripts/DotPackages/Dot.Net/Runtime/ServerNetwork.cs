using DotEngine.Core;
using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEngine.Net
{
    public interface ISessionHandler
    {
        void OnMessageReceived(Guid sessionId, int messageId,object message);
    }

    public class SimpleSession : TcpSession
    {
        private MessageBuffer receivedBuffer = new MessageBuffer();
        private MemoryStream sendStream = new MemoryStream();

        public IMessageHandler MessageHandler { get; set; }
        public ISessionHandler SessionHandler { get; set; }

        public SimpleSession(TcpServer server) : base(server)
        {
        }

        public bool SendMessage(int messageId,object message)
        {
            if(IsConnected)
            {
                byte[] messageBytes = MessageHandler.Serialize(messageId, message);
                return SendMessage(messageBytes);
            }
            return false;
        }

        public bool SendMessage(byte[] messageBytes)
        {
            sendStream.SetLength(0);
            byte[] lenBytes = BitConverter.GetBytes(messageBytes.Length);
            sendStream.Write(lenBytes, 0, lenBytes.Length);
            if (messageBytes != null && messageBytes.Length > 0)
            {
                sendStream.Write(messageBytes, 0, messageBytes.Length);
            }
            return SendAsync(sendStream.ToArray());
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            receivedBuffer.WriteBytes(buffer, (int)offset, (int)size);
            while (true)
            {
                byte[] messageBytes = receivedBuffer.ReadMessage();
                if (messageBytes == null)
                {
                    break;
                }

                bool result = MessageHandler.Deserialize(messageBytes, out int messageId, out object message);
                if (result)
                {
                    SessionHandler.OnMessageReceived(Id, messageId, message);   
                }
                else
                {
                    Debug.LogError("");
                    
                    Disconnect();

                    break;
                }
            }
        }
    }

    internal class ServerReceivedMessage : IPoolItem
    {
        public Guid SessionId { get; set; } = Guid.Empty;
        public int MessageId { get; set; } = -1;
        public object MessageBody { get; set; } = null;

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
    public delegate void ServerSessionListener(SimpleSession session);

    public class ServerNetwork : TcpServer,ISessionHandler
    {
        private IMessageHandler messageHandler;

        private ItemPool<ServerReceivedMessage> receviedMessagePool = new ItemPool<ServerReceivedMessage>();
        private List<ServerReceivedMessage> receviedMessageList = new List<ServerReceivedMessage>();

        private Dictionary<int, List<ServerMessageListener>> messageListenerDic = new Dictionary<int, List<ServerMessageListener>>();
        public ServerStateListener NetStarted;
        public ServerStateListener NetStopped;
        public ServerStateListener NetError;

        private object stateLocker = new object();
        public ServerState State { get; set; } = ServerState.Unreachable;
        public ServerState nextState = ServerState.Unreachable;

        public ServerNetwork(IMessageHandler handler, int port) : base(IPAddress.Any, port)
        {
            messageHandler = handler;
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            if (!IsStarted)
            {
                return;
            }

            lock (stateLocker)
            {
                if (State != nextState)
                {
                    if (nextState == ServerState.Started)
                    {
                        NetStarted?.Invoke();
                    }
                    else if (nextState == ServerState.Stoped)
                    {
                        NetStopped?.Invoke();
                    }
                    else if (nextState == ServerState.Error)
                    {
                        NetError?.Invoke();
                    }
                    State = nextState;
                }
            }

            lock (receviedMessageList)
            {
                for (int i = 0; i < receviedMessageList.Count; ++i)
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
                }
            }
        }

        protected override void OnStarted()
        {
            lock (stateLocker)
            {
                nextState = ServerState.Started;
            }
        }

        protected override void OnStopped()
        {
            lock (stateLocker)
            {
                nextState = ServerState.Stoped;
            }
        }

        protected override void OnError(SocketError error)
        {
            lock (stateLocker)
            {
                nextState = ServerState.Error;
            }
        }

        protected override void OnConnected(TcpSession session)
        {
            base.OnConnected(session);
        }

        protected override void OnDisconnected(TcpSession session)
        {
            base.OnDisconnected(session);
        }

        protected override TcpSession CreateSession()
        {
            return base.CreateSession();
        }

        public void OnMessageReceived(Guid sessionId, int messageId, object message)
        {
            lock (receviedMessageList)
            {
                ServerReceivedMessage receviedMessage = receviedMessagePool.Get();
                receviedMessage.MessageId = messageId;
                receviedMessage.MessageBody = message;
                receviedMessage.SessionId = sessionId;
                receviedMessageList.Add(receviedMessage);
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

        public void UnRegisterListener(int messageId)
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
