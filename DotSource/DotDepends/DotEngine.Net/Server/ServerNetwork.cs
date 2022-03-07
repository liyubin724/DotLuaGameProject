using NetCoreServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public enum ServerNetworkState
    {
        Unreachable = 0,
        Started,
        Stopped,
        Error,
    }

    public delegate void ServerNetworkStateChanged();
    public delegate void ServerNetworkSessionChanged(Guid guid);

    class SessionData
    {
        public Guid guid;
        public byte[] dataBytes;
    }

    public class ServerNetwork : TcpServer
    {
        public ServerNetworkStateChanged OnNetStarted;
        public ServerNetworkStateChanged OnNetStopped;
        public ServerNetworkStateChanged OnNetError;

        public ServerNetworkSessionChanged OnSessionConnected;
        public ServerNetworkSessionChanged OnSessionDisconnected;
        public ServerNetworkSessionChanged OnSessionError;

        private ConcurrentQueue<SessionData> sessionDatas = new ConcurrentQueue<SessionData>();

        public ServerNetwork(string address, int port) : base(address, port)
        {
        }

        public void DoUpdate(float deltaTime)
        {
            if(!IsStarted)
            {
                return;
            }
            if(Sessions.Count>0)
            {
                foreach(var session in Sessions.Values)
                {
                    (session as ServerSession).DoUdpate(deltaTime);
                }
            }

        }

        protected override TcpSession CreateSession()
        {
            var serverSession = new ServerSession(this);
            serverSession.OnSessionConnected = (session) =>
            {
                OnSessionConnected?.Invoke(session.Id);
            };
            serverSession.OnSessionDisconnected = (session) =>
            {
                OnSessionDisconnected?.Invoke(session.Id);
            };
            serverSession.OnSessionError = (session) =>
            {
                OnSessionError?.Invoke(session.Id);
            };
            serverSession.OnSessionDataReceived = (session, dataBytes) =>
            {
                sessionDatas.Enqueue(new SessionData()
                {
                    guid = session.Id,
                    dataBytes = dataBytes
                });
            };

            return serverSession;
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            base.OnStopped();
        }

        protected override void OnError(SocketError error)
        {
            base.OnError(error);
        }
    }
}
