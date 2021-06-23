using NetCoreServer;
using System;
using System.Net.Sockets;

namespace DotEngine.NetCore.TCPNetwork
{
    public interface IServerNetSessionHandler
    {
        void OnDataReceived(Guid id, byte[] buffer, long offset, long size);
        void OnStateChanged(ServerSessionState state);
    }

    public class ServerNetSession : TcpSession
    {
        public ServerNetSession(TcpServer server) : base(server)
        {
        }

        protected override void OnConnected()
        {
            
        }

        protected override void OnDisconnected()
        {
            
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            
        }

        protected override void OnError(SocketError error)
        {
            
        }
    }
}
