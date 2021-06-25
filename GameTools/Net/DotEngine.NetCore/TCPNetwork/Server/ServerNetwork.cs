using NetCoreServer;
using System;
using System.Collections.Concurrent;
using System.Net;

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
        public Guid Id { get; set; }
        public long UserId{get;set;}

        public bool IsVerified => UserId > 0;
    }

    internal class ServerMessageData
    {
        public Guid Id { get; set; }
        public int MsgId { get; set; }
        public byte[] MsgBytes { get; set; }
    }

    public class ServerNetwork : TcpServer, IServerNetSessionHandler
    {
        public string Name { get; private set; }

        private ConcurrentDictionary<Guid, ServerSessionDesc> sessionDescDic = new ConcurrentDictionary<Guid, ServerSessionDesc>();
        private ConcurrentQueue<ServerMessageData> messageDataQueue = new ConcurrentQueue<ServerMessageData>();

        internal Func<IMessageEncoder> MessageEncoderCreateFunc { get; set; }
        internal Func<IMessageDecoder> MessageDecoderCreateFunc { get; set; }
        
        public ServerNetwork(string name, IPAddress address, int port) : base(address, port)
        {
            Name = name;
        }

        internal ServerMessageData[] ExtractMessageDatas()
        {
            if(messageDataQueue.Count>0)
            {
                ServerMessageData[] datas = new ServerMessageData[messageDataQueue.Count];
                messageDataQueue.CopyTo(datas, 0);
                return datas;
            }
            return null;
        }

        public void OnStateChanged(Guid id, ServerNetSessionState state)
        {
            if (state == ServerNetSessionState.Disconnected)
            {
                sessionDescDic.TryRemove(id,out _);
            }
        }

        protected override TcpSession CreateSession()
        {
            ServerNetSession session = new ServerNetSession(this);
            session.SessionHandler = this;
            session.MessageEncoder = MessageEncoderCreateFunc();
            session.MessageDecoder = MessageDecoderCreateFunc();

            ServerSessionDesc sessionDesc = new ServerSessionDesc()
            {
                Id = session.Id,
            };
            sessionDescDic.TryAdd(session.Id, sessionDesc);
            return session;
        }

        public bool Multicast(int msgId, byte[] msgBytes)
        {
            bool result = true;
            foreach(var session in Sessions)
            {
                if(!((ServerNetSession)session.Value).SendMessage(msgId, msgBytes))
                {
                    result = false;
                }
            }
            return result;
        }

        public bool Send(Guid id,int msgId,byte[] msgBytes)
        {
            if(Sessions.TryGetValue(id,out var session))
            {
                return (session as ServerNetSession).SendMessage(msgId, msgBytes);
            }
            return false;
        }

        public void OnMessageReceived(Guid id, int msgId, byte[] msgBytes)
        {
            messageDataQueue.Enqueue(new ServerMessageData()
            {
                Id = id,
                MsgId = msgId,
                MsgBytes = msgBytes,
            });
        }
    }
}
