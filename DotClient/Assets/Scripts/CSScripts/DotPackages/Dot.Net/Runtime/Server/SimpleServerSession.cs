using System;

namespace DotEngine.Net
{
    public enum ServerState
    {
        Unreachable = 0,
        Started,
        Stoped,
        Error,
    }

    public interface IServerSessionHandler
    {
        void OnMessageReceived(Guid guid, byte[] dataBytes);
    }

    public class SimpleServerSession : TcpSession
    {
        private MessageBuffer receivedBuffer = new MessageBuffer();

        public IServerSessionHandler Handler { get; set; }

        public SimpleServerSession(TcpServer server) : base(server)
        {
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            receivedBuffer.WriteBytes(buffer, (int)offset, (int)size);
            while (true)
            {
                byte[] messageBytes = receivedBuffer.ReadMessage();
                if (messageBytes == null)
                {
                    break;
                }

                Handler?.OnMessageReceived(Id, messageBytes);
            }
        }
    }
}
