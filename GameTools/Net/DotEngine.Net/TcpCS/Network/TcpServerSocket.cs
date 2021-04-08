using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace DotEngine.Net.TcpNetwork
{
    public class TcpServerSocket : AbstractTcpSocket
    {
        public event EventHandler<TcpSocketEventArgs> OnClientConnect;
        public event EventHandler<TcpSocketEventArgs> OnClientDisconnect;

        public int ConnectedClients { get { return clients.Count; } }

        private List<Socket> clients;

        public TcpServerSocket()
        {
            logTag = NetUtil.SERVER_LOG_TAG;
            clients = new List<Socket>();
        }

        public void Listen(int port, int backlog = 100)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Bind(new IPEndPoint(IPAddress.Any, port));
                socket.Listen(backlog);
                IsConnected = true;

                NetUtil.LogInfo(logTag, $"Listening on port {port}..."); 

                Accept();
            }
            catch (Exception ex)
            {
                socket = null;
                NetUtil.LogError(logTag, $"Listen failed.message = {ex.Message}");
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

            NetUtil.LogInfo(logTag, $"New client connection accepted ({clientEndPoint.Address}:{clientEndPoint.Port})");

            OnClientConnect?.Invoke(this, new TcpSocketEventArgs(client));

            StartReceiving(client);
        }

        protected override void DisconnectedByRemote(Socket socket)
        {
            try
            {
                IPEndPoint clientEndPoint = (IPEndPoint)socket.RemoteEndPoint;

                NetUtil.LogInfo(logTag, $"Client disconnected ({clientEndPoint.Address}:{clientEndPoint.Port})");
            }
            catch (Exception)
            {
                NetUtil.LogWarning(logTag, "TcpServerSocket::DisconnectedByRemote->Client disconnected.");
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

        public void SendExcept(Socket exceptClient,byte[] bytes)
        {
            foreach (var client in clients)
            {
                if(client != exceptClient)
                {
                    SendWith(client, bytes);
                }
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
                NetUtil.LogInfo(logTag, "Stopped listening.");

                IsConnected = false;
                socket.Close();
                TriggerOnDisconnect();
            }
            else
            {
                NetUtil.LogWarning(logTag, "Already disconnected.");
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