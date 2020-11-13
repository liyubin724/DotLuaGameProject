using System;
using System.Net.Sockets;
using DotEngine.Log;

namespace DotEngine.Network
{
    public struct ReceiveVO
    {
        public Socket socket;
        public byte[] buffer;
    }

    public abstract class AbstractTcpSocket
    {
        public event EventHandler<ReceiveEventArgs> OnReceive;
        public event EventHandler OnDisconnect;

        public bool IsConnected { get; protected set; }

        protected Logger logger;
        protected Socket socket;

        protected void TriggerOnReceive(ReceiveVO receiveVO, int bytesReceived)
        {
            OnReceive?.Invoke(this, new ReceiveEventArgs(receiveVO.socket, TrimmedBuffer(receiveVO.buffer, bytesReceived)));
        }

        protected void TriggerOnDisconnect()
        {
            OnDisconnect?.Invoke(this, null);
        }

        protected void StartReceiving(Socket socket)
        {
            var receiveVO = new ReceiveVO
            {
                socket = socket,
                buffer = new byte[socket.ReceiveBufferSize]
            };
            Receive(receiveVO);
        }

        protected void Receive(ReceiveVO receiveVO)
        {
            receiveVO.socket.BeginReceive(receiveVO.buffer, 0, receiveVO.buffer.Length, SocketFlags.None, OnReceived, receiveVO);
        }

        protected void OnReceived(IAsyncResult ar)
        {
            var receiveVO = (ReceiveVO)ar.AsyncState;
            if (IsConnected)
            {
                var bytesReceived = receiveVO.socket.EndReceive(ar);

                if (bytesReceived == 0)
                {
                    DisconnectedByRemote(receiveVO.socket);
                }
                else
                {
                    logger.Debug(string.Format("Received {0} bytes.", bytesReceived));
                    TriggerOnReceive(receiveVO, bytesReceived);

                    Receive(receiveVO);
                }
            }
        }

        public void SendWith(Socket socket, byte[] bytes)
        {
            if (IsConnected && socket.Connected)
            {
                socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, OnSended, socket);
            }
        }

        void OnSended(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }

        protected abstract void DisconnectedByRemote(Socket socket);

        protected byte[] TrimmedBuffer(byte[] buffer, int length)
        {
            var trimmed = new byte[length];
            Array.Copy(buffer, trimmed, length);
            return trimmed;
        }

        public abstract void Send(byte[] bytes);

        public abstract void Disconnect();
    }
}
