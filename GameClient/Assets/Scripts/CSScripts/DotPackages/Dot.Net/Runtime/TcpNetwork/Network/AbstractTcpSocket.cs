using System;
using System.Net.Sockets;

namespace DotEngine.Net.TcpNetwork
{
    public struct ReceiveVO
    {
        public Socket socket;
        public byte[] buffer;
    }

    public abstract class AbstractTcpSocket
    {
        public event EventHandler<TcpNetworkReceiveEventArgs> OnReceive;
        public event EventHandler OnDisconnect;

        protected Socket socket;
        protected string logTag = string.Empty;

        public bool IsConnected { get; protected set; }

        protected void TriggerOnReceive(ReceiveVO receiveVO, int bytesReceived)
        {
            OnReceive?.Invoke(this, new TcpNetworkReceiveEventArgs(receiveVO.socket, TrimmedBuffer(receiveVO.buffer, bytesReceived)));
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
                    NetLogger.LogInfo(logTag,"the length of bytes which was received from net is zero.the net was closed by remote");

                    DisconnectedByRemote(receiveVO.socket);
                }
                else
                {
                    NetLogger.LogInfo(logTag, $"Received {bytesReceived} bytes.");
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
            }else
            {
                NetLogger.LogError(logTag, "the net is disconnected");
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
