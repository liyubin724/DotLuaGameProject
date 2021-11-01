using DotEngine.Core;
using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

namespace DotEngine.Net
{
    public enum ClientState
    {
        Unreachable = 0,
        Connected,
        Normal,
        Disconnected,
        Error,
    }

    public interface IClientHandler
    {
        void OnStateChanged(ClientState state);
        void OnMessageReceived(byte[] messageBody);
    }

    class SimpleClient : TcpClient
    {
        private MessageBuff receivedBuffer = new MessageBuff();
        private MemoryStream sendStream = new MemoryStream();
        public IClientHandler Handler { get; set; }

        public SimpleClient(string ip, int port) : base(ip, port)
        {
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

        protected override void OnConnected()
        {
            Handler.OnStateChanged(ClientState.Connected);
        }

        protected override void OnDisconnected()
        {
            Handler.OnStateChanged(ClientState.Disconnected);
        }

        protected override void OnError(SocketError error)
        {
            Handler.OnStateChanged(ClientState.Error);
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

                Handler.OnMessageReceived(messageBytes);
            }
        }
    }

    internal class ClientReceviedMessage : IPoolItem
    {
        public int Id { get; set; } = -1;
        public object Body { get; set; } = null;

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            Id = -1;
            Body = null;
        }
    }

    public delegate void ClientMessageListener(int messageId, object message);
    public delegate void ClientStateListener();

    public class ClientNetwork : ADispose, IClientHandler
    {
        private IMessageHandler messageHandler;
        private SimpleClient client = null;

        private int messagePerformMaxCount = int.MaxValue;
        public int MessagePerformMaxCount
        {
            get
            {
                return messagePerformMaxCount;
            }
            set
            {
                if (value <= 0)
                {
                    messagePerformMaxCount = int.MaxValue;
                }
                else
                {
                    messagePerformMaxCount = value;
                }
            }
        }

        private object stateLocker = new object();
        public ClientState State { get; private set; } = ClientState.Unreachable;
        private ClientState preState = ClientState.Unreachable;

        public ClientStateListener OnConnected;
        public ClientStateListener OnDisconnected;
        public ClientStateListener OnError;

        private ItemPool<ClientReceviedMessage> receviedMessagePool = new ItemPool<ClientReceviedMessage>();
        private List<ClientReceviedMessage> receviedMessageList = new List<ClientReceviedMessage>();

        private Dictionary<int, List<ClientMessageListener>> messageListenerDic = new Dictionary<int, List<ClientMessageListener>>();

        public ClientNetwork(IMessageHandler handler)
        {
            messageHandler = handler;
        }

        public bool ConenctAsync(string ip, int port)
        {
            if (client == null)
            {
                client = new SimpleClient(ip, port);
            }else
            {
                Debug.LogError("");
            }
            return client.ConnectAsync();
        }

        public bool ReconnectAsync()
        {
            if(client!=null)
            {
                return client.ReconnectAsync();
            }else
            {
                Debug.LogError("");
            }
            return false;
        }

        public bool DisconnectAsync()
        {
            if (client != null)
            {
                return client.DisconnectAsync();
            }
            else
            {
                Debug.LogError("");
            }
            return false;
        }

        public bool SendAsync(int messageId, object message)
        {
            if (client != null && client.IsConnected)
            {
                byte[] messageBytes = messageHandler.Serialize(messageId, message);
                return client.SendAsync(messageBytes);
            }
            else
            {
                Debug.LogError("");
            }
            return false;
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            if(client == null || !client.IsConnected)
            {
                return;
            }

            lock(stateLocker)
            {
                if(State!=preState)
                {
                    if(State == ClientState.Connected)
                    {
                        State = ClientState.Normal;
                        OnConnected?.Invoke();
                    }else if(State == ClientState.Disconnected)
                    {
                        OnDisconnected?.Invoke();
                    }else if(State == ClientState.Error)
                    {
                        OnError?.Invoke();
                    }

                    preState = State;
                }
            }

            lock (receviedMessageList)
            {
                int maxCount = Math.Min(receviedMessageList.Count, MessagePerformMaxCount);
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

        public void OnMessageReceived(byte[] messageBody)
        {
            bool result = messageHandler.Deserialize(messageBody, out int messageId, out object message);
            if (result)
            {
                lock (receviedMessageList)
                {
                    ClientReceviedMessage receviedMessage = receviedMessagePool.Get();
                    receviedMessage.Id = messageId;
                    receviedMessage.Body = message;
                    receviedMessageList.Add(receviedMessage);
                }
            }
            else
            {
                Debug.LogError("");
                DisconnectAsync();
            }
        }

        public void OnStateChanged(ClientState state)
        {
            lock(stateLocker)
            {
                State = state;
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

        #region dispose
        protected override void DisposeManagedResource()
        {

        }

        protected override void DisposeUnmanagedResource()
        {

        }
        #endregion
    }
}
