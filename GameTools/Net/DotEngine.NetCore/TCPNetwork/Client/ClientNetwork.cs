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
        private IClientNetworkHandler networkHandler = null;

        public ClientNetwork(string address, int port, IClientNetworkHandler handler) : base(address, port)
        {
            networkHandler = handler;
        }

        public override bool ConnectAsync()
        {
            networkHandler.OnStateChanged(ClientNetworkState.Connecting);
            return base.ConnectAsync();
        }

        public override bool DisconnectAsync()
        {
            networkHandler.OnStateChanged(ClientNetworkState.Disconnecting);
            return base.DisconnectAsync();
        }

        protected override void OnConnected()
        {
            networkHandler.OnStateChanged(ClientNetworkState.Connected);
        }

        protected override void OnDisconnected()
        {
            networkHandler.OnStateChanged(ClientNetworkState.Disconnected);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            networkHandler?.OnDataReceived(buffer, offset, size);
        }

        protected override void OnError(SocketError error)
        {
            networkHandler.OnStateChanged(ClientNetworkState.Error);
        }
    }
}
