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
    public enum ServerNetSessionState
    {
        Connected = 0,
        Disconnected,
        Error,
    }

    internal class ServerSessionDesc
    {
        public TcpSession Session { get; set; }
        public long UserId { get; set; } = -1;

        public Guid Id => Session.Id;
        public bool IsVerified => UserId > 0;
    }

    public class ServerNetwork : TcpServer, IServerNetSessionHandler
    {
        public string Name { get; private set; }

        private object sessionDescLocker = new object();
        private Dictionary<Guid, ServerSessionDesc> sessionDescDic = new Dictionary<Guid, ServerSessionDesc>();


        public ServerNetwork(string name,IPAddress address, int port) : base(address, port)
        {
            Name = name;
        }

        public void OnDataReceived(Guid id, byte[] buffer, long offset, long size)
        {

        }

        public void OnStateChanged(Guid id, ServerNetSessionState state)
        {

        }

        protected override TcpSession CreateSession()
        {
            ServerNetSession session = new ServerNetSession(this,this);

            ServerSessionDesc sessionDesc = new ServerSessionDesc();
            sessionDesc.Session = session;
            sessionDescDic.Add(session.Id, sessionDesc);

            return session;
        }

        protected override void OnError(SocketError error)
        {

        }

        public void DoUpdate(float deltaTime)
        {

        }

        public void OnMessageReceived(Guid id, byte[][] msgBytes)
        {
            
        }
    }
}
