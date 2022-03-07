using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public class ServerNetwork : TcpServer
    {
        public ServerNetwork(string address, int port) : base(address, port)
        {
        }

        protected override TcpSession CreateSession()
        {
            return new TcpSession(this);
        }

        protected override void OnConnected(TcpSession session)
        {
            base.OnConnected(session);
        }

        protected override void OnDisconnected(TcpSession session)
        {
            base.OnDisconnected(session);
        }
    }
}
