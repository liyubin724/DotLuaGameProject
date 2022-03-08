using NetCoreServer;
using System;
using System.Net;

namespace DotEngine.Net
{
    public enum ClientNetworkState
    {
        Unreachable = 0,
        Connected,
        Disconnected,
        Error,
    }

    public delegate void ClientNetworkStateChanged();
    public delegate void ClientNetworkDataReceived(byte[] msgBytes);

    public class ClientNetwork : TcpClient
    {
        private object stateLocker = new object();
        private ClientNetworkState prevState = ClientNetworkState.Unreachable;
        private ClientNetworkState currState = ClientNetworkState.Unreachable;

        private object bufferLocker = new object();
        private DataBuffer dataBuffer = new DataBuffer();

        public int ReadCountAtOnce { get; set; } = -1;

        public ClientNetworkStateChanged OnNetConnected;
        public ClientNetworkStateChanged OnNetDisconnected;
        public ClientNetworkStateChanged OnNetError;

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
                    if (currState == ClientNetworkState.Connected)
                    {
                        OnNetConnected?.Invoke();
                    }
                    else if (currState == ClientNetworkState.Disconnected)
                    {
                        OnNetDisconnected?.Invoke();
                    }
                    else if (currState == ClientNetworkState.Error)
                    {
                        OnNetError?.Invoke();
                    }
                    prevState = currState;
                }
            }

            lock (bufferLocker)
            {
                if (dataBuffer.Length > 0 && IsConnected)
                {
                    int index = 1;
                    byte[] dataBytes = dataBuffer.ReadMessage();
                    while (dataBytes != null && (ReadCountAtOnce < 0 || index < ReadCountAtOnce))
                    {
                        OnDataReceived?.Invoke(dataBytes);
                        dataBytes = dataBuffer.ReadMessage();
                        index++;
                    }
                }
            }
        }

        public override long Send(byte[] buffer, long offset, long size)
        {
            if (buffer == null || buffer.Length - offset < size || size < 0)
            {
                return 0;
            }
            byte[] lenBytes = BitConverter.GetBytes((int)size);
            int dataLen = (int)size + sizeof(int);
            byte[] dataBytes = new byte[dataLen];
            Array.Copy(lenBytes, dataBytes, sizeof(int));
            Array.Copy(buffer, offset, dataBytes, sizeof(int), size);
            return base.Send(dataBytes, 0, dataLen);
        }

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            if (buffer == null || buffer.Length - offset < size || size < 0)
            {
                return false;
            }

            byte[] lenBytes = BitConverter.GetBytes((int)size);
            int dataLen = (int)size + sizeof(int);
            byte[] dataBytes = new byte[dataLen];
            Array.Copy(lenBytes, dataBytes, sizeof(int));
            Array.Copy(buffer, offset, dataBytes, sizeof(int), size);
            return base.SendAsync(dataBytes, 0, dataLen);
        }

        public long SendMessage(int messId, byte[] messBytes)
        {
            int dataLen = sizeof(int);
            if (messBytes != null && messBytes.Length > 0)
            {
                dataLen += messBytes.Length;
            }
            byte[] lenBytes = BitConverter.GetBytes(dataLen);
            dataLen += sizeof(int);
            byte[] messIdBytes = BitConverter.GetBytes(messId);
            byte[] dataBytes = new byte[dataLen];
            Array.Copy(lenBytes, dataBytes, sizeof(int));
            Array.Copy(messIdBytes, dataBytes, sizeof(int));
            if (messBytes != null && messBytes.Length > 0)
            {
                Array.Copy(messBytes, 0, dataBytes, sizeof(int) + sizeof(int), messBytes.Length);
            }
            return base.Send(dataBytes, 0, dataLen);
        }

        public bool SendMessageAsync(int messId, byte[] messBytes)
        {
            int dataLen = sizeof(int);
            if (messBytes != null && messBytes.Length > 0)
            {
                dataLen += messBytes.Length;
            }
            byte[] lenBytes = BitConverter.GetBytes(dataLen);
            dataLen += sizeof(int);
            byte[] messIdBytes = BitConverter.GetBytes(messId);
            byte[] dataBytes = new byte[dataLen];
            Array.Copy(lenBytes, dataBytes, sizeof(int));
            Array.Copy(messIdBytes, dataBytes, sizeof(int));
            if (messBytes != null && messBytes.Length > 0)
            {
                Array.Copy(messBytes, 0, dataBytes, sizeof(int) + sizeof(int), messBytes.Length);
            }
            return base.SendAsync(dataBytes, 0, dataLen);
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
