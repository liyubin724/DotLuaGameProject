using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace DotEngine.Net.TCPNetwork
{
    public enum ClientNetworkStatus
    {
        None = 0,
        Connecting,
        Connected,
        Disconnecting,
        Disconnected,
    }

    public class ClientNetwork
    {
        public string Name { get; private set; }
        private TcpClientSocket m_ClientSocket = null;

        private object m_StatusLocker = new object();
        private ClientNetworkStatus m_Status = ClientNetworkStatus.None;
        public ClientNetworkStatus Status
        {
            get
            {
                lock (m_StatusLocker)
                {
                    return m_Status;
                }
            }

            set
            {
                lock (m_StatusLocker)
                {
                    m_Status = value;
                }
            }
        }

        public bool IsConnected => m_ClientSocket != null && Status == ClientNetworkStatus.Connected;

        public Action OnConnectingCallback = null;
        public Action OnConnectedCallback = null;
        public Action OnDisconnectingCallback = null;
        public Action OnDisconnectedCallback = null;

        private object m_MessageLocker = new object();
        private List<byte[]> m_Messages = new List<byte[]>();
        private Dictionary<int, Action<byte[]>> m_MessagHandlerDic = new Dictionary<int, Action<byte[]>>();

        private ClientNetworkStatus m_PreStatus = ClientNetworkStatus.None;

        public ClientNetwork(string name)
        {
            Name = name;
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            if (m_ClientSocket == null)
            {
                return;
            }
            ClientNetworkStatus curStatus = Status;
            if (m_PreStatus != curStatus)
            {
                if (curStatus == ClientNetworkStatus.Connecting)
                {
                    OnConnectingCallback?.Invoke();
                }
                else if (curStatus == ClientNetworkStatus.Connected)
                {
                    OnConnectedCallback?.Invoke();
                }
                else if (curStatus == ClientNetworkStatus.Disconnecting)
                {
                    OnDisconnectingCallback?.Invoke();
                }
                else if (curStatus == ClientNetworkStatus.Disconnected)
                {
                    OnDisconnectedCallback?.Invoke();
                }

                m_PreStatus = curStatus;
            }

            if (curStatus == ClientNetworkStatus.Connected)
            {
                lock (m_MessageLocker)
                {
                    foreach (var bytes in m_Messages)
                    {
                        int id = BitConverter.ToInt32(bytes, 0);
                        if (m_MessagHandlerDic.TryGetValue(id, out var callback))
                        {
                            byte[] contentBytes = new byte[0];
                            if (bytes.Length > sizeof(int))
                            {
                                contentBytes = new byte[bytes.Length - sizeof(int)];
                                Array.Copy(bytes, sizeof(int), contentBytes, 0, contentBytes.Length);
                            }
                            callback(contentBytes);
                        }
                        else
                        {
                            NetUtil.LogWarning(NetUtil.CLIENT_LOG_TAG,$"the handler of  id({id}) is not found");
                        }
                    }

                    m_Messages.Clear();
                }
            }
        }

        public bool Connect(string ipString, int port)
        {
            if (m_ClientSocket != null)
            {
                NetUtil.LogError(NetUtil.CLIENT_LOG_TAG, "The socket has been created");
                return false;
            }

            m_ClientSocket = new TcpClientSocket();
            m_ClientSocket.OnConnect += OnConnected;
            m_ClientSocket.OnReceive += OnReceived;
            m_ClientSocket.OnDisconnect += OnDisconnected;

            Status = ClientNetworkStatus.Connecting;

            m_ClientSocket.Connect(IPAddress.Parse(ipString), port);

            return true;
        }

        public void SendMessage(int id, byte[] messageBytes)
        {
            if (IsConnected)
            {
                byte[] idBytes = BitConverter.GetBytes(id);
                if (messageBytes != null && messageBytes.Length > 0)
                {
                    byte[] bytes = new byte[sizeof(int) + messageBytes.Length];
                    Array.Copy(idBytes, 0, bytes, 0, idBytes.Length);
                    Array.Copy(messageBytes, 0, bytes, idBytes.Length, messageBytes.Length);

                    m_ClientSocket.Send(bytes);
                }
                else
                {
                    m_ClientSocket.Send(idBytes);
                }
            }
        }

        public void RegistMessageHandler(int id, Action<byte[]> messageHandler)
        {
            if (!m_MessagHandlerDic.ContainsKey(id))
            {
                m_MessagHandlerDic.Add(id, messageHandler);
            }
            else
            {
                NetUtil.LogError(NetUtil.CLIENT_LOG_TAG, "The handler has been added.id = " + id);
            }
        }

        public void RegistMessageHandler(Type handlerType)
        {
            var methodInfos = handlerType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methodInfos)
            {
                var attr = method.GetCustomAttribute<ClientMessageHandlerAttribute>();
                if (attr != null)
                {
                    if (Delegate.CreateDelegate(typeof(Action<byte[]>), null, method) is Action<byte[]> messageHandler)
                    {
                        RegistMessageHandler(attr.ID, messageHandler);
                    }
                }
            }
        }

        public void UnregistMessageHandler(int id)
        {
            if (m_MessagHandlerDic.ContainsKey(id))
            {
                m_MessagHandlerDic.Remove(id);
            }
        }

        public void UnregistMessageHandler(Type handlerType)
        {
            var methodInfos = handlerType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methodInfos)
            {
                var attr = method.GetCustomAttribute<ClientMessageHandlerAttribute>();
                if (attr != null)
                {
                    UnregistMessageHandler(attr.ID);
                }
            }
        }

        public bool Disconnect()
        {
            if (m_ClientSocket != null && (Status == ClientNetworkStatus.Connecting || Status == ClientNetworkStatus.Connected))
            {
                Status = ClientNetworkStatus.Disconnecting;
                m_ClientSocket.Disconnect();
                return true;
            }
            return false;
        }

        private void OnConnected(object sender, EventArgs eventArgs)
        {
            Status = ClientNetworkStatus.Connected;
        }

        private void OnReceived(object sender, ReceiveEventArgs eventArgs)
        {
            if (Status != ClientNetworkStatus.Connected)
            {
                return;
            }

            lock (m_MessageLocker)
            {
                m_Messages.Add(eventArgs.bytes);
            }
        }

        private void OnDisconnected(object sender, EventArgs eventArgs)
        {
            Status = ClientNetworkStatus.Disconnected;
            m_ClientSocket.OnConnect -= OnConnected;
            m_ClientSocket.OnReceive -= OnReceived;
            m_ClientSocket.OnDisconnect -= OnDisconnected;

            lock (m_MessageLocker)
            {
                m_Messages.Clear();
            }

            m_ClientSocket = null;
        }
    }
}
