using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.NetCore.TCPNetwork
{
    public class ServerSession : TcpSession
    {
        public ServerSession(TcpServer server) : base(server)
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
