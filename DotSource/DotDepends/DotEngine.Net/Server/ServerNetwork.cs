using NetCoreServer;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;

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
    public delegate void ServerNetowrkSessionDataReceived(Guid guid, byte[] dataBytes);

    class SessionData
    {
        public Guid guid;
        public byte[] bytes;
    }

    public class ServerNetwork : TcpServer
    {
        public ServerNetworkStateChanged OnNetStarted;
        public ServerNetworkStateChanged OnNetStopped;
        public ServerNetworkStateChanged OnNetError;

        public ServerNetworkSessionChanged OnSessionConnected;
        public ServerNetworkSessionChanged OnSessionDisconnected;
        public ServerNetworkSessionChanged OnSessionError;

        public ServerNetowrkSessionDataReceived OnDataReceived;

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
            if(sessionDatas.Count>0)
            {
                while(sessionDatas.TryDequeue(out var data))
                {
                    OnDataReceived?.Invoke(data.guid, data.bytes);
                }
            }
        }

        public bool Multicast(int messId, byte[] messBody)
        {
            if (!IsStarted)
                return false;

            foreach (var session in Sessions.Values)
                (session as ServerSession).SendMessage(messId,messBody);

            return true;
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
                    bytes = dataBytes
                });
            };

            return serverSession;
        }

        protected override void OnStarted()
        {
            OnNetStarted?.Invoke();
        }

        protected override void OnStopped()
        {
            OnNetStopped?.Invoke();
        }

        protected override void OnError(SocketError error)
        {
            OnNetError?.Invoke();
        }
    }
}
