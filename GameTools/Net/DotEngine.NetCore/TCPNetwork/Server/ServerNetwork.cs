using NetCoreServer;
using System;
using System.Collections.Generic;
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
        public TcpSession Session { get; set; }
        public long UserId { get; set; } = -1;

        public Guid Id => Session.Id;
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

        private IMessageEncoder messageEncoder;
        private IMessageDecoder messageDecoder;

        private object sessionDescLocker = new object();
        private Dictionary<Guid, ServerSessionDesc> sessionDescDic = new Dictionary<Guid, ServerSessionDesc>();

        private object messageDatasLocker = new object();
        private List<ServerMessageData> messageDatas = new List<ServerMessageData>();

        public ServerNetwork(string name, IPAddress address, int port, IMessageEncoder encoder, IMessageDecoder decoder) : base(address, port)
        {
            Name = name;
            messageEncoder = encoder;
            messageDecoder = decoder;
        }

        internal ServerMessageData[] ExtractMessageDatas()
        {
            lock(messageDatasLocker)
            {
                ServerMessageData[] datas = messageDatas.ToArray();
                messageDatas.Clear();

                return datas;
            }
        }

        public void OnStateChanged(Guid id, ServerNetSessionState state)
        {
            lock (sessionDescLocker)
            {
                if (state == ServerNetSessionState.Disconnected)
                {
                    if (sessionDescDic.ContainsKey(id))
                    {
                        sessionDescDic.Remove(Id);
                    }
                }
            }
        }

        protected override TcpSession CreateSession()
        {
            ServerNetSession session = new ServerNetSession(this, this);

            ServerSessionDesc sessionDesc = new ServerSessionDesc();
            sessionDesc.Session = session;
            lock (sessionDescLocker)
            {
                sessionDescDic.Add(session.Id, sessionDesc);
            }

            return session;
        }

        public void OnMessageReceived(Guid id, byte[][] dataBytes)
        {
            lock(messageDatasLocker)
            {
                foreach(var data in dataBytes)
                {
                    if(messageDecoder.DecodeMessage(data,out var msgId,out byte[] msgBytes))
                    {
                        messageDatas.Add(new ServerMessageData()
                        {
                            Id = id,
                            MsgId = msgId,
                            MsgBytes = msgBytes,
                        });
                    }
                }
            }
        }

        public bool Multicast(int msgId, byte[] msgBytes)
        {
            byte[] dataBytes = messageEncoder.EncodeMessage(msgId, msgBytes);
            return Multicast(dataBytes);
        }

        public bool Send(Guid id,int msgId,byte[] msgBytes)
        {
            byte[] dataBytes = messageEncoder.EncodeMessage(msgId, msgBytes);

            lock (sessionDescLocker)
            {
                if(sessionDescDic.TryGetValue(id,out var desc))
                {
                    return desc.Session.SendAsync(dataBytes);
                }
            }

            return false;
        }
    }
}
