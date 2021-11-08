using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public enum ClientState
    {
        Unreachable = 0,
        Connected,
        Disconnected,
        Error,
    }

    public interface IClientHandler
    {
        void OnStateChanged(ClientState state);
        void OnMessageReceived(byte[] dataBytes);
    }

    public class SimpleClient : TcpClient
    {
        private ByteBuffer sendBuffer = new ByteBuffer();
        private MessageBuffer receivedBuffer = new MessageBuffer();

        public IClientHandler Handler { get; set; }

        public SimpleClient(string address, int port) : base(address, port)
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

        protected override void OnConnected()
        {
            Handler?.OnStateChanged(ClientState.Connected);
        }

        protected override void OnDisconnected()
        {
            Handler?.OnStateChanged(ClientState.Disconnected);
        }

        protected override void OnError(SocketError error)
        {
            Handler?.OnStateChanged(ClientState.Error);
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

                Handler?.OnMessageReceived(messageBytes);
            }
        }
    }
}
