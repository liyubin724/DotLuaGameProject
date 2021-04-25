using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace DotEngine.Net
{
    public enum NetworkStates
    {
        Unavailable = 0,
        Connecting,
        ConnectedFailed,
        Normal,
        Disconnected,
    }

    public enum NetworkDisconnectErrors
    {
        ProcessReceiveError = 0,
    }

    public enum NetworkOperations
    {
        None = 0,
        Connecting,
        Connected,
        Sending,
        Sended,
        Receiving,
        ReceiveBytes,
        ReceiveMessage,
        Disconnecting,
        Disconnected,
        DisconnectedByError,
    }

    public abstract class ANetworkSocket
    {
        protected Socket netSocket;
        protected INetworkHandler netHandler;

        private NetworkStates state = NetworkStates.Unavailable;
        private object stateLocker = new object();
        public NetworkStates State
        {
            get
            {
                lock(stateLocker)
                {
                    return state;
                }
            }
            set
            {
                lock(stateLocker)
                {
                    if(value != state)
                    {
                        NetworkStates preState = state;
                        state = value;

                        netHandler.OnStateChanged(preState, state);
                    }
                }
            }
        }

        public bool IsConnected => netSocket != null && netSocket.Connected && State == NetworkStates.Normal;

        private byte receivedMessageSeriousIndex = 0;
        private NetMessageBuffer receivedMessageBuffer = new NetMessageBuffer();

        protected Dictionary<SocketAsyncOperation, Action<SocketAsyncEventArgs>> asyncOperationDic = new Dictionary<SocketAsyncOperation, Action<SocketAsyncEventArgs>>();

        public ANetworkSocket(Socket socket,INetworkHandler handler)
        {
            netSocket = socket;
            netHandler = handler;
        }

        public abstract void Connect(string ip, int port);
        public abstract void Connect();

        public void Send(int messageID,MessageCompressType compressType,MessageCryptoType cryptoType,byte[] dataBytes)
        {

        }

        public void Disconnect()
        {

        }

        protected void DoReceive()
        {

        }

        protected void DoSend()
        {

        }

        protected void OnHandleSocketEvent(object sender, SocketAsyncEventArgs socketEvent)
        {
            if (socketEvent.SocketError == SocketError.Success)
            {
                if (asyncOperationDic.TryGetValue(socketEvent.LastOperation, out var action))
                {
                    action(socketEvent);
                }
            }
            else
            {
                
            }
        }

        protected void ProcessReceive(SocketAsyncEventArgs socketEvent)
        {
            if (socketEvent.SocketError == SocketError.Success)
            {
                if (socketEvent.BytesTransferred > 0)
                {
                    OnDataReceived(socketEvent.Buffer, socketEvent.BytesTransferred);
                    return;
                }
            }

            DoDisconnectByError(NetworkDisconnectErrors.ProcessReceiveError);
        }

        private void DoDisconnectByError(NetworkDisconnectErrors error)
        {
            Disconnect();
        }

        private void OnDataReceived(byte[] bytes,int size)
        {
            receivedMessageBuffer.WriteBytes(bytes, size);
            byte[] messageBytes = receivedMessageBuffer.ReadMessage();
            while(messageBytes!=null)
            {
                ++receivedMessageSeriousIndex;
                Desrialize(messageBytes, out int messageID, out byte seriousIndex, out byte[] dataBytes);

                if(seriousIndex != receivedMessageSeriousIndex)
                {

                    break;
                }else
                {
                    netHandler.OnMessageHandler(this, messageID, dataBytes);
                }
            }
        }

        private void Desrialize(byte[] bytes, out int messageID, out byte seriousIndex, out byte[] dataBytes)
        {
            int offsetIndex = 0;
            messageID = BitConverter.ToInt32(bytes, offsetIndex);
            offsetIndex += sizeof(int);

            seriousIndex = bytes[offsetIndex];
            offsetIndex += sizeof(byte);

            if (bytes.Length - 1 > offsetIndex)
            {
                MessageCompressType compressType = (MessageCompressType)bytes[offsetIndex];
                offsetIndex += sizeof(byte);
                MessageCryptoType cryptoType = (MessageCryptoType)bytes[offsetIndex];
                offsetIndex += sizeof(byte);

                dataBytes = new byte[bytes.Length - offsetIndex];
                Array.Copy(bytes, offsetIndex, dataBytes, 0, dataBytes.Length);

                dataBytes = netHandler.DecodeMessage(cryptoType, dataBytes);
                dataBytes = netHandler.UncompressMessage(compressType, dataBytes);
            }else
            {
                dataBytes = null;
            }
        }
    }
}