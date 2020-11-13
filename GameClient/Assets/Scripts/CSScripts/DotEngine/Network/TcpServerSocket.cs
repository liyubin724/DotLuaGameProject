using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using DotEngine.Log;

namespace DotEngine.Network
{
    public class TcpServerSocket : AbstractTcpSocket
    {
        public event EventHandler<TcpSocketEventArgs> OnClientConnect;
        public event EventHandler<TcpSocketEventArgs> OnClientDisconnect;

        public int ConnectedClients { get { return clients.Count; } }

        private List<Socket> clients;

        public TcpServerSocket()
        {
            logger = LogUtil.GetLogger(GetType().Name,LogLevel.Error);

            clients = new List<Socket>();
        }

        public void Listen(int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Bind(new IPEndPoint(IPAddress.Any, port));
                socket.Listen(100);
                IsConnected = true;
                logger.Info(string.Format("Listening on port {0}...", port));
                Accept();
            }
            catch (Exception ex)
            {
                socket = null;
                logger.Warning(ex.Message);
            }
        }

        void Accept()
        {
            socket.BeginAccept(OnAccepted, socket);
        }

        void OnAccepted(IAsyncResult ar)
        {
            if (IsConnected)
            {
                var server = (Socket)ar.AsyncState;
                AcceptedClientConnection(server.EndAccept(ar));
                Accept();
            }
        }

        void AcceptedClientConnection(Socket client)
        {
            clients.Add(client);

            IPEndPoint clientEndPoint = (IPEndPoint)client.RemoteEndPoint;

            logger.Info(string.Format("New client connection accepted ({0}:{1})", clientEndPoint.Address, clientEndPoint.Port));

            OnClientConnect?.Invoke(this, new TcpSocketEventArgs(client));

            StartReceiving(client);
        }

        protected override void DisconnectedByRemote(Socket socket)
        {
            try
            {
                IPEndPoint clientEndPoint = (IPEndPoint)socket.RemoteEndPoint;

                logger.Info(string.Format("Client disconnected ({0}:{1})",clientEndPoint.Address, clientEndPoint.Port));
            }
            catch (Exception)
            {
                logger.Info("Client disconnected.");
            }

            socket.Close();
            clients.Remove(socket);
            OnClientDisconnect?.Invoke(this, new TcpSocketEventArgs(socket));
        }

        public override void Send(byte[] bytes)
        {
            foreach (var client in clients)
            {
                SendWith(client, bytes);
            }
        }

        public override void Disconnect()
        {
            foreach (var client in clients)
            {
                client.BeginDisconnect(false, OnClientDisconnected, client);
            }

            if (IsConnected)
            {
                logger.Info("Stopped listening.");
                IsConnected = false;
                socket.Close();
                TriggerOnDisconnect();
            }
            else
            {
                logger.Info("Already diconnected.");
            }
        }

        void OnClientDisconnected(IAsyncResult ar)
        {
            var client = (Socket)ar.AsyncState;
            client.EndDisconnect(ar);
            DisconnectedByRemote(client);
        }
    }
}