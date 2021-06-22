using System.Net.Sockets;
using TcpClient = NetCoreServer.TcpClient;

namespace DotEngine.NetCore.TCPNetwork
{
    public interface IClientNetworkHandler
    {
        void OnDataReceived(byte[] buffer, long offset, long size);
    }

    public class ClientNetwork : TcpClient
    {
        private IClientNetworkHandler networkHandler = null;

        private object stateLocker = new object();
        private ClientNetworkState state = ClientNetworkState.Unreachable;

        public ClientNetworkState State
        {
            get
            {
                lock(stateLocker)
                {
                    return state;
                }
            }
            private set
            {
                lock(stateLocker)
                {
                    if(state != value)
                    {
                        state = value;
                    }
                }
            }
        }

        public ClientNetwork(string address, int port,IClientNetworkHandler handler) : base(address, port)
        {
            networkHandler = handler;
        }

        public override bool ConnectAsync()
        {
            State = ClientNetworkState.Connecting;

            return base.ConnectAsync();
        }

        public override bool DisconnectAsync()
        {
            State = ClientNetworkState.Disconnecting;

            return base.DisconnectAsync();
        }

        protected override void OnConnected()
        {
            State = ClientNetworkState.Connected;
        }

        protected override void OnDisconnected()
        {
            State = ClientNetworkState.Disconnected;
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            networkHandler?.OnDataReceived(buffer, offset, size);
        }

        protected override void OnError(SocketError error)
        {
            State = ClientNetworkState.OtherError;
        }
    }
}
