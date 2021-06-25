using System.Net;
using System.Net.Sockets;
using TcpClient = NetCoreServer.TcpClient;

namespace DotEngine.NetCore.TCPNetwork
{
    public interface IClientNetworkHandler
    {
        void OnDataReceived(byte[] buffer, long offset, long size);
        void OnStateChanged(ClientNetworkState state);
    }

    public class ClientNetwork : TcpClient
    {
        internal IClientNetworkHandler NetworkHandler { get; set; }

        public ClientNetwork(string address, int port) : base(address, port)
        {
        }

        public ClientNetwork(IPAddress ip, int port) : base(ip, port)
        {
        }

        public override bool ConnectAsync()
        {
            NetworkHandler.OnStateChanged(ClientNetworkState.Connecting);
            return base.ConnectAsync();
        }

        public override bool DisconnectAsync()
        {
            NetworkHandler.OnStateChanged(ClientNetworkState.Disconnecting);
            return base.DisconnectAsync();
        }

        protected override void OnConnected()
        {
            NetworkHandler.OnStateChanged(ClientNetworkState.Connected);
        }

        protected override void OnDisconnected()
        {
            NetworkHandler.OnStateChanged(ClientNetworkState.Disconnected);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            NetworkHandler.OnDataReceived(buffer, offset, size);
        }

        protected override void OnError(SocketError error)
        {
            NetworkHandler.OnStateChanged(ClientNetworkState.Error);
        }
    }
}
