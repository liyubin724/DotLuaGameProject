using System;
using System.Net;
using System.Net.Sockets;

namespace DotEngine.Net
{
    public interface IServerHandler
    {
        void OnStateChanged(ServerState state);
        void OnMessageReceived(Guid guid, byte[] dataBytes);
    }

    public class SimpleServer : TcpServer,IServerSessionHandler
    {
        private ByteBuffer sendBuffer = new ByteBuffer();

        public IServerHandler Handler { get; set; }

        public SimpleServer(IPEndPoint endpoint) : base(endpoint)
        {
        }

        public SimpleServer(IPAddress address, int port) : base(address, port)
        {
        }

        public SimpleServer(string address, int port) : base(address, port)
        {
        }
        
        public void OnMessageReceived(Guid guid, byte[] dataBytes)
        {
            Handler.OnMessageReceived(guid, dataBytes);
        }

        public bool MulticastMessage(byte[] dataBytes)
        {
            if (!IsStarted)
            {
                return false;
            }

            sendBuffer.Clear();
            int len = dataBytes.Length;
            byte[] lenBytes = BitConverter.GetBytes(len);
            sendBuffer.Append(lenBytes);
            sendBuffer.Append(dataBytes);

            return Multicast(sendBuffer.Data, sendBuffer.Offset, sendBuffer.Size);
        }

        public bool MulticastMessageTo(Guid guid, byte[] dataBytes)
        {
            if (!IsStarted)
            {
                return false;
            }

            SimpleServerSession session = FindSession(guid) as SimpleServerSession;
            if(session == null)
            {
                return false;
            }
            sendBuffer.Clear();

            int len = dataBytes.Length;
            byte[] lenBytes = BitConverter.GetBytes(len);
            sendBuffer.Append(lenBytes);
            sendBuffer.Append(dataBytes);

            return session.SendAsync(sendBuffer.Data, sendBuffer.Offset, sendBuffer.Size);
        }

        protected override TcpSession CreateSession()
        {
            SimpleServerSession session = new SimpleServerSession(this);
            session.Handler = this;

            return session;
        }

        protected override void OnStarted()
        {
            Handler.OnStateChanged(ServerState.Started);
        }

        protected override void OnStopped()
        {
            Handler.OnStateChanged(ServerState.Stoped);
        }

        protected override void OnError(SocketError error)
        {
            Handler.OnStateChanged(ServerState.Error);
        }
    }
}
