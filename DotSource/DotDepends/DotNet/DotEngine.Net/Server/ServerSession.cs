using NetCoreServer;
using System;
using System.Net.Sockets;

namespace DotEngine.Net
{
    public enum ServerSessionState
    {
        Unreachable = 0,
        Connected,
        Disconnected,
        Error,
    }

    public delegate void ServerSessionStateChanged(ServerSession session);
    public delegate void ServerSessionDataReceived(ServerSession session, byte[] msgBytes);

    public class ServerSession : TcpSession
    {
        private object stateLocker = new object();
        private ServerSessionState prevState = ServerSessionState.Unreachable;
        private ServerSessionState currState = ServerSessionState.Unreachable;

        private object bufferLocker = new object();
        private DataBuffer dataBuffer = new DataBuffer();

        public int ReadCountAtOnce { get; set; } = -1;

        public ServerSessionStateChanged OnSessionConnected;
        public ServerSessionStateChanged OnSessionDisconnected;
        public ServerSessionStateChanged OnSessionError;

        public ServerSessionDataReceived OnSessionDataReceived;

        public ServerSession(ServerNetwork server) : base(server)
        {
        }

        public void DoUdpate(float deltaTime)
        {
            lock (stateLocker)
            {
                if (prevState != currState)
                {
                    if (currState == ServerSessionState.Connected)
                    {
                        OnSessionConnected?.Invoke(this);
                    }
                    else if (currState == ServerSessionState.Disconnected)
                    {
                        OnSessionDisconnected?.Invoke(this);
                    }
                    else if (currState == ServerSessionState.Error)
                    {
                        OnSessionError?.Invoke(this);
                    }
                    prevState = currState;
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
                currState = ServerSessionState.Connected;
            }
        }

        protected override void OnDisconnected()
        {
            lock (stateLocker)
            {
                currState = ServerSessionState.Disconnected;
            }
            lock (bufferLocker)
            {
                dataBuffer.ResetStream();
            }
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            lock (bufferLocker)
            {
                dataBuffer.WriteBytes(buffer, (int)offset, (int)size);
                if (dataBuffer.Length > 0 && IsConnected)
                {
                    int index = 1;
                    byte[] dataBytes = dataBuffer.ReadMessage();
                    while (dataBytes != null && (ReadCountAtOnce < 0 || index < ReadCountAtOnce))
                    {
                        OnSessionDataReceived?.Invoke(this, dataBytes);
                        dataBytes = dataBuffer.ReadMessage();
                        index++;
                    }
                }
            }
        }

        protected override void OnError(System.Net.Sockets.SocketError error)
        {
            lock (stateLocker)
            {
                currState = ServerSessionState.Error;
            }
        }
    }
}
