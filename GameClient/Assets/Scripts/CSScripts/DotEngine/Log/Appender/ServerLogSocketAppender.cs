using DotEngine.Log.Formatter;
using DotEngine.Network;
using System;
using System.Text;

namespace DotEngine.Log.Appender
{
    public class ServerLogSocketAppender : ALogAppender
    {
        public static readonly string NAME = "SocketServerLog";
        public static readonly int PORT = 8899;

        public event Action<string> OnNetMessageReceived;
        public event Action OnNetConnected;
        public event Action OnNetDisconnected;

        private TcpServerSocket m_serverSocket;
        private bool m_IsDisposed = false;

        protected ServerLogSocketAppender(ILogFormatter formatter) : base(NAME, formatter)
        {
            m_serverSocket = new TcpServerSocket();
            m_serverSocket.OnClientConnect += OnConnected;
            m_serverSocket.OnReceive += OnReceived;
            m_serverSocket.OnDisconnect += OnDisconnected;
            m_serverSocket.Listen(PORT);
        }

        public ServerLogSocketAppender():this(new JsonLogFormatter())
        {
        }

        ~ServerLogSocketAppender()
        {
            Dispose(false);
        }

        bool IsReadyForSend()
        {
            return m_serverSocket != null && m_serverSocket.IsConnected && m_serverSocket.ConnectedClients > 0;
        }

        public void Disconnect()
        {
            m_serverSocket.Disconnect();
        }

        private void OnConnected(object sender, EventArgs e)
        {
            OnNetConnected?.Invoke();
        }

        private void OnReceived(object sender, ReceiveEventArgs e)
        {
            OnNetMessageReceived?.Invoke(Encoding.UTF8.GetString(e.bytes));
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            OnNetDisconnected?.Invoke();
            m_serverSocket = null;
        }

        protected override void DoLogMessage(LogLevel level, string message)
        {
            if (IsReadyForSend())
            {
                m_serverSocket.Send(Encoding.UTF8.GetBytes(message));
            }
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (m_IsDisposed) return;

            if (disposing)
            {

            }
            if(m_serverSocket!=null)
            {
                m_serverSocket.OnClientConnect -= OnConnected;
                m_serverSocket.OnReceive -= OnReceived;
                m_serverSocket.OnDisconnect -= OnDisconnected;
                m_serverSocket.Disconnect();
                m_serverSocket = null;
            }

            m_IsDisposed = true;
        }
    }
}
