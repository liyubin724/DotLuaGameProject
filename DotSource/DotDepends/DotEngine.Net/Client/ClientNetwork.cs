using NetCoreServer;
using System.Net;

namespace DotEngine.Net
{
    public enum ClientNetworkState
    {
        Unreachable = 0,
        Connecting,
        Connected,
        Disconnecting,
        Disconnected,
        Error,
    }

    public delegate void ClientNetworkStateChanged(ClientNetwork client, ClientNetworkState prevState, ClientNetworkState curState);
    public delegate void ClientNetworkDataReceived(ClientNetwork client, byte[] msgBytes);

    public class ClientNetwork : TcpClient
    {
        private object stateLocker = new object();
        private ClientNetworkState prevState = ClientNetworkState.Unreachable;
        private ClientNetworkState currState = ClientNetworkState.Unreachable;

        private object bufferLocker = new object();
        private DataBuffer dataBuffer = new DataBuffer();

        public int ReadCountAtOnce { get; set; } = -1;

        public ClientNetworkStateChanged OnStateChanged;
        public ClientNetworkDataReceived OnDataReceived;

        public ClientNetwork(string address, int port) : base(new IPEndPoint(IPAddress.Parse(address), port))
        {
        }

        public void DoUpdate(float deltaTime)
        {
            lock (stateLocker)
            {
                if (prevState != currState)
                {
                    OnStateChanged?.Invoke(this, prevState, currState);
                    prevState = currState;
                }
            }

            lock (bufferLocker)
            {
                if (dataBuffer.Length > 0 && IsConnected)
                {
                    int index = 1;
                    byte[] dataBytes = dataBuffer.ReadMessage();
                    while (dataBytes != null && (index < 0 || index < ReadCountAtOnce))
                    {
                        OnDataReceived?.Invoke(this, dataBytes);
                        dataBytes = dataBuffer.ReadMessage();
                        index++;
                    }
                }
            }
        }

        public override bool Connect()
        {
            lock (stateLocker)
            {
                currState = ClientNetworkState.Connecting;
            }
            return base.Connect();
        }

        public override bool ConnectAsync()
        {
            lock (stateLocker)
            {
                currState = ClientNetworkState.Connecting;
            }
            return base.ConnectAsync();
        }

        public override bool Disconnect()
        {
            lock (stateLocker)
            {
                currState = ClientNetworkState.Disconnecting;
            }
            return base.Disconnect();
        }

        public override bool DisconnectAsync()
        {
            lock (stateLocker)
            {
                currState = ClientNetworkState.Disconnecting;
            }
            return base.DisconnectAsync();
        }

        protected override void OnConnected()
        {
            lock (stateLocker)
            {
                currState = ClientNetworkState.Connected;
            }
        }

        protected override void OnDisconnected()
        {
            lock (stateLocker)
            {
                currState = ClientNetworkState.Disconnected;
            }
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            lock (bufferLocker)
            {
                dataBuffer.WriteBytes(buffer, (int)offset, (int)size);
            }
        }

        protected override void OnError(System.Net.Sockets.SocketError error)
        {
            lock (stateLocker)
            {
                currState = ClientNetworkState.Error;
            }
        }
    }
}
