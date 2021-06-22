using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.NetCore.TCPNetwork
{
    public class ServerNetwork : TcpServer
    {
        public ServerNetwork(IPAddress address, int port) : base(address, port)
        {
        }

        protected override TcpSession CreateSession()
        {
            return new ServerSession(this);
        }

        protected override void OnError(SocketError error)
        {
            
        }
    }
}
