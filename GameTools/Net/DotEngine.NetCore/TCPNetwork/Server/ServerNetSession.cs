using NetCoreServer;
using System;
using System.Net.Sockets;

namespace DotEngine.NetCore.TCPNetwork
{
    public interface IServerNetSessionHandler
    {
        void OnStateChanged(Guid id, ServerNetSessionState state);
        void OnMessageReceived(Guid id, int msgId, byte[] msgBytes);
    }

    public class ServerNetSession : TcpSession
    {
        private static readonly string LOG_TAG = "ServerNetSession";

        private NetMessageBuffer messageBuffer = new NetMessageBuffer();

        public IServerNetSessionHandler SessionHandler { get; set; }
        public IMessageEncoder MessageEncoder { get; set; }
        public IMessageDecoder MessageDecoder { get; set; }

        public ServerNetSession(TcpServer server) : base(server)
        {
        }

        internal bool SendMessage(int msgID,byte[] msgBytes)
        {
            byte[] dataBytes = MessageEncoder.EncodeMessage(msgID, msgBytes);
            return SendAsync(dataBytes);
        }

        protected override void OnConnected()
        {
            SessionHandler.OnStateChanged(Id, ServerNetSessionState.Connected);
        }

        protected override void OnDisconnected()
        {
            SessionHandler.OnStateChanged(Id, ServerNetSessionState.Disconnected);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if(MessageDecoder == null)
            {
                NetLogger.Error(LOG_TAG, "The decoder is not null");
                return;
            }
            messageBuffer.WriteBytes(buffer, (int)offset, (int)size);

            byte[][] dataBytesArr = messageBuffer.ReadMessages();
            if (dataBytesArr != null)
            {
                foreach(var dataBytes in dataBytesArr)
                {
                    if(MessageDecoder.DecodeMessage(dataBytes,out int msgId, out byte[] msgBytes))
                    {
                        SessionHandler.OnMessageReceived(Id, msgId, msgBytes);
                    }
                }
            }
        }

        protected override void OnError(SocketError error)
        {
            SessionHandler.OnStateChanged(Id, ServerNetSessionState.Error);
        }
    }
}
