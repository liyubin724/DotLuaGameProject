using System;
using System.Net;
using System.Net.Sockets;

namespace DotEngine.Net.TcpNetwork
{
    public class TcpClientSocket : AbstractTcpSocket
    {
        public event EventHandler OnConnect;

        public TcpClientSocket()
        {
            logTag = NetUtil.CLIENT_LOG_TAG;
        }

        public void Connect(IPAddress ip, int port)
        {
            NetUtil.LogInfo(logTag,$"Connecting to {ip}:{port}...");

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

                NetUtil.LogInfo(logTag, "Connected");

                OnConnect?.Invoke(this, null);

                StartReceiving(socket);
            }
            catch (Exception ex)
            {
                NetUtil.LogError(logTag, $"Connected failed .message = {ex.Message}");
                TriggerOnDisconnect();
            }
        }

        public override void Send(byte[] bytes)
        {
            SendWith(socket, bytes);
        }

        protected override void DisconnectedByRemote(Socket socket)
        {
            NetUtil.LogWarning(logTag, "Disconnected by remote.");

            Disconnect();
        }

        public override void Disconnect()
        {
            if (IsConnected)
            {
                NetUtil.LogInfo(logTag, "Disconnecting...");
                IsConnected = false;
                socket.BeginDisconnect(false, OnDisconnected, socket);
            }
            else
            {
                NetUtil.LogWarning(logTag, "Already disconnected.");
            }
        }

        void OnDisconnected(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;
            socket.EndDisconnect(ar);
            socket.Close();

            base.socket = null;

            NetUtil.LogInfo(logTag, "Disconnected.");

            TriggerOnDisconnect();
        }
    }
}

