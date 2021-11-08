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
        private Buffer sendBuffer = new Buffer();

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

            foreach (var session in Sessions.Values)
            {
                session.SendAsync(sendBuffer.Data, sendBuffer.Offset, sendBuffer.Size);
            }
            return true;
        }

        public bool MulticastMessageTo(Guid guid, byte[] dataBytes)
        {

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
