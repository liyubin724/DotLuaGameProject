using NetCoreServer;
using System;
using System.Net.Sockets;

namespace DotEngine.NetCore.TCPNetwork
{
    public interface IServerNetSessionHandler
    {
        void OnStateChanged(Guid id, ServerNetSessionState state);
        void OnMessageReceived(Guid id, byte[][] msgBytes);
    }

    public class ServerNetSession : TcpSession
    {
        private IServerNetSessionHandler sessionHandler = null;

        private NetMessageBuffer messageBuffer = new NetMessageBuffer();

        public ServerNetSession(TcpServer server, IServerNetSessionHandler handler) : base(server)
        {
            sessionHandler = handler;
        }

        protected override void OnConnected()
        {
            sessionHandler.OnStateChanged(Id, ServerNetSessionState.Connected);
        }

        protected override void OnDisconnected()
        {
            sessionHandler.OnStateChanged(Id, ServerNetSessionState.Disconnected);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            messageBuffer.WriteBytes(buffer, (int)offset, (int)size);

            byte[][] msgBytes = messageBuffer.ReadMessages();
            if(msgBytes!=null)
            {
                sessionHandler.OnMessageReceived(Id, msgBytes);
            }
        }

        protected override void OnError(SocketError error)
        {
            sessionHandler.OnStateChanged(Id, ServerNetSessionState.Error);
        }
    }
}
