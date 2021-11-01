using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    internal class SimpleSession : TcpSession
    {
        public SimpleSession(TcpServer server) : base(server)
        {
        }

        protected override void OnConnected()
        {
            
        }

        protected override void OnDisconnected()
        {
            
        }

        protected override void OnError(SocketError error)
        {
            
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            
        }
    }

    internal class SimpleServer : TcpServer
    {
        public SimpleServer(string ip,int port):base(ip,port)
        {

        }

        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {

        }

        protected override TcpSession CreateSession()
        {
            return new SimpleSession(this);
        }

        protected override void OnConnected(TcpSession session)
        {
            
        }

        protected override void OnDisconnected(TcpSession session)
        {
            
        }

        protected override void OnError(SocketError error)
        {
            
        }

        protected override void OnStarted()
        {
            
        }

        protected override void OnStopped()
        {
            
        }
    }

    public class ServerNetwork
    {
    }
}
