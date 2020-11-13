using System;
using System.Net;
using System.Net.Sockets;
using DotEngine.Log;

namespace DotEngine.Network
{
    public class TcpClientSocket : AbstractTcpSocket
    {
        public event EventHandler OnConnect;

        public TcpClientSocket()
        {
            logger = LogUtil.GetLogger(GetType().Name,LogLevel.Error);
        }

        public void Connect(IPAddress ip, int port)
        {
            logger.Debug(string.Format("Connecting to {0}:{1}...", ip, port));
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(ip, port, OnConnected, socket);
        }

        void OnConnected(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;
            try
            {
                socket.EndConnect(ar);
                IsConnected = true;

                IPEndPoint clientEndPoint = (IPEndPoint)socket.RemoteEndPoint;

                logger.Info(string.Format("Connected to {0}:{1}", clientEndPoint.Address, clientEndPoint.Port));

                OnConnect?.Invoke(this, null);

                StartReceiving(socket);
            }
            catch (Exception ex)
            {
                logger.Warning(ex.Message);

                TriggerOnDisconnect();
            }
        }

        public override void Send(byte[] bytes)
        {
            SendWith(socket, bytes);
        }

        protected override void DisconnectedByRemote(Socket socket)
        {
            logger.Info("Disconnected by remote.");
            Disconnect();
        }

        public override void Disconnect()
        {
            if (IsConnected)
            {
                logger.Debug("Disconnecting...");
                IsConnected = false;
                socket.BeginDisconnect(false, OnDisconnected, socket);
            }
            else
            {
                logger.Debug("Already diconnected.");
            }
        }

        void OnDisconnected(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;
            socket.EndDisconnect(ar);
            socket.Close();

            base.socket = null;
            
            logger.Debug("Disconnected.");

            TriggerOnDisconnect();
        }
    }
}

