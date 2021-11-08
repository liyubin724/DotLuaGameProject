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
        private Buffer sendBuffer = new Buffer();
        private MessageBuffer receivedBuffer = new MessageBuffer();

        public IServerSessionHandler Handler { get; set; }

        public SimpleServerSession(TcpServer server) : base(server)
        {
        }

        public bool SendMessage(byte[] dataBytes)
        {
            sendBuffer.Clear();

            int len = dataBytes.Length;
            byte[] lenBytes = BitConverter.GetBytes(len);
            sendBuffer.Append(lenBytes);
            sendBuffer.Append(dataBytes);

            return SendAsync(sendBuffer.Data, sendBuffer.Offset, sendBuffer.Size);
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
