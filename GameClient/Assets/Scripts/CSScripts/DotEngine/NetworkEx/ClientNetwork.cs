using DotEngine.Log;
using DotEngine.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.NetworkEx
{
    public enum ClientNetworkStatus
    {
        None = 0,
        Connecting,
        Connected,
        Disconnecting,
        Disconnected,
    }


    public class ClientNetwork : IUpdate
    {
        private TcpClientSocket m_ClientSocket = null;

        private object m_StatusLocker = new object();
        private ClientNetworkStatus m_Status = ClientNetworkStatus.None;

        private Logger m_Logger = null;

        public ClientNetworkStatus Status
        {
            get
            {
                lock(m_StatusLocker)
                {
                    return m_Status;
                }
            }

            set
            {
                lock(m_StatusLocker)
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
            m_Logger = LogUtil.GetLogger($"ClientNet({name})", LogLevel.Error, LogLevel.Error);
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            ClientNetworkStatus curStatus = Status;

            if(m_PreStatus != curStatus)
            {
                if(curStatus == ClientNetworkStatus.Connecting)
                {
                    OnConnectingCallback?.Invoke();
                }else if(curStatus == ClientNetworkStatus.Connected)
                {
                    OnConnectedCallback?.Invoke();
                }else if(curStatus == ClientNetworkStatus.Disconnecting)
                {
                    OnDisconnectingCallback?.Invoke();
                }else if(curStatus == ClientNetworkStatus.Disconnected)
                {
                    OnDisconnectedCallback?.Invoke();
                }

                m_PreStatus = curStatus;
            }
        }

        public bool Connect(string ipString,int port)
        {
            if(m_ClientSocket != null)
            {
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

        public void SendMessage(int id,byte[] messageBytes)
        {
            if(IsConnected)
            {
                byte[] idBytes = BitConverter.GetBytes(id);
                byte[] bytes = new byte[sizeof(int) + messageBytes.Length];
                Array.Copy(idBytes, 0, bytes, 0, idBytes.Length);
                Array.Copy(messageBytes, 0, bytes, idBytes.Length, messageBytes.Length);

                m_ClientSocket.Send(bytes);
            }
        }

        public void RegistMessageHandler(int id,Action<byte[]> messageHandler)
        {
            if(!m_MessagHandlerDic.ContainsKey(id))
            {
                m_MessagHandlerDic.Add(id, messageHandler);
            }else
            {

            }
        }

        public void UnregistMessageHandler(int id)
        {
            if(m_MessagHandlerDic.ContainsKey(id))
            {
                m_MessagHandlerDic.Remove(id);
            }else
            {

            }
        }

        public bool Disconnect()
        {
            if(m_ClientSocket !=null && (Status == ClientNetworkStatus.Connecting || Status == ClientNetworkStatus.Connected))
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
            if(Status == ClientNetworkStatus.Connected)
            {

            }
        }

        private void OnDisconnected(object sender, EventArgs eventArgs)
        {
            Status = ClientNetworkStatus.Disconnected;
            m_ClientSocket.OnConnect -= OnConnected;
            m_ClientSocket.OnReceive -= OnReceived;
            m_ClientSocket.OnDisconnect -= OnDisconnected;

            m_ClientSocket = null;
        }
    }
}
