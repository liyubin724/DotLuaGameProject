using System;
using System.Net;
using System.Net.Sockets;

namespace DotEngine.Network
{
    public class TcpClientSocket : AbstractTcpSocket
    {
        public event EventHandler OnConnect;

        public TcpClientSocket()
        {
        }

        public void Connect(IPAddress ip, int port)
        {
            DebugLog.Debug($"TcpClientSocket::Connect->Connecting to {ip}:{port}...");

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

                DebugLog.Debug("TcpClientSocket::Connect->Connected");

                OnConnect?.Invoke(this, null);

                StartReceiving(socket);
            }
            catch (Exception ex)
            {
                DebugLog.Warning($"TcpClientSocket::Connect->{ex.Message}");

                TriggerOnDisconnect();
            }
        }

        public override void Send(byte[] bytes)
        {
            SendWith(socket, bytes);
        }

        protected override void DisconnectedByRemote(Socket socket)
        {
            DebugLog.Info("TcpClientSocket::DisconnectedByRemote->Disconnected by remote.");

            Disconnect();
        }

        public override void Disconnect()
        {
            if (IsConnected)
            {
                DebugLog.Debug("TcpClientSocket::Disconnect->Disconnecting...");
                IsConnected = false;
                socket.BeginDisconnect(false, OnDisconnected, socket);
            }
            else
            {
                DebugLog.Info("TcpClientSocket::Disconnect->Already disconnected.");
            }
        }

        void OnDisconnected(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;
            socket.EndDisconnect(ar);
            socket.Close();

            base.socket = null;

            DebugLog.Debug("TcpClientSocket::OnDisconnected->Disconnected.");

            TriggerOnDisconnect();
        }
    }
}

