using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    internal class CachedNetworkSendMessage
    {
        public int ID { get; set; }
        public MessageCompressType CompressType { get; set; } = MessageCompressType.None;
        public MessageCryptoType CryptoType { get; set; } = MessageCryptoType.None;
        public byte[] DataBytes { get; set; }

        public void DoReset()
        {
            ID = -1;
            CompressType = MessageCompressType.None;
            CryptoType = MessageCryptoType.None;
            DataBytes = null;
        }
    }

    public abstract class ANetworkSocket
    {
        private static readonly int RECEIVE_BUFFER_SIZE = 4096;

        protected Socket NetSocket { get; set; }
        protected INetworkHandler Handler { get; set; }

        private SocketAsyncEventArgs sendAsyncEvent = null;
        private SocketAsyncEventArgs receiveAsyncEvent = null;

        protected Dictionary<SocketAsyncOperation, Action<SocketAsyncEventArgs>> asyncOperationDic = new Dictionary<SocketAsyncOperation, Action<SocketAsyncEventArgs>>();

        private byte serializedSeriousIndex = 0;
        private NetMessageStream serializeMessageStream = new NetMessageStream();

        private byte deserializedSeriousIndex = 0;
        private NetMessageBuffer receivedMessageBuffer = new NetMessageBuffer();

        private GenericObjectPool<CachedNetworkSendMessage> sendMessagePool = new GenericObjectPool<CachedNetworkSendMessage>(
                () => new CachedNetworkSendMessage(),
                null,
                (nsm) =>
                {
                    nsm.DoReset();
                });
        private object sendMessageLocker = new object();
        private List<CachedNetworkSendMessage> cachedSendMessages = new List<CachedNetworkSendMessage>();
        private object isSendingLocker = new object();
        private bool isSending = false;

        public bool IsConnected => NetSocket != null && NetSocket.Connected;

        public void BindSocket(Socket socket, INetworkHandler handler)
        {
            NetSocket = socket;
            Handler = handler;
        }

        public abstract void DoConnect(string ip, int port);
        public abstract void DoConnect();

        public void DoSend()
        {
            
        }

        public void DoReceive()
        {
            if (!IsConnected)
            {
                return;
            }

            if (receiveAsyncEvent == null)
            {
                receiveAsyncEvent = new SocketAsyncEventArgs();
                receiveAsyncEvent.SetBuffer(new byte[RECEIVE_BUFFER_SIZE], 0, RECEIVE_BUFFER_SIZE);
                receiveAsyncEvent.Completed += OnHandleSocketEvent;
            }

            try
            {
                if (NetSocket.ReceiveAsync(receiveAsyncEvent))
                {
                    return;
                }
            }
            catch (Exception e)
            {

            }
        }

        public void DoDisconnect()
        {

        }

        public void SendMessage(int messageID, MessageCompressType compressType, MessageCryptoType cryptoType, byte[] dataBytes)
        {
            var message = sendMessagePool.Get();
            message.ID = messageID;
            message.CompressType = compressType;
            message.CryptoType = cryptoType;
            message.DataBytes = dataBytes;

            cachedSendMessages.Add(message);
        }

        protected void ProcessSend(SocketAsyncEventArgs socketEvent)
        {
            if (socketEvent.SocketError == SocketError.Success)
            {
                lock (isSendingLocker)
                {
                    isSending = false;
                }

                DoSend();
                return;
            }
            //OnSessionError(NetworkSessionError.ProcessSendSocketError, socketEvent);
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

            //SessionHandler?.OnSessionError(NetworkSessionError.ProcessReceiveSocketError, this, socketEvent);
        }

        protected void ProcessDisconnect(SocketAsyncEventArgs socketEvent)
        {

        }

        private void OnHandleSocketEvent(object sender, SocketAsyncEventArgs socketEvent)
        {
            if (socketEvent.SocketError == SocketError.Success)
            {
                if (asyncOperationDic.TryGetValue(socketEvent.LastOperation, out var action))
                {
                    action(socketEvent);
                    return;
                }
            }

                //OnSessionError(NetworkSessionError.HandleSocketEventError, socketEvent);
        }

        private void OnDataReceived(byte[] bytes, int size)
        {
            receivedMessageBuffer.WriteBytes(bytes, size);

            byte[] messageBytes = receivedMessageBuffer.ReadMessage();
            while (messageBytes != null)
            {
                ++deserializedSeriousIndex;

                byte seriousIndex = Desrialize(messageBytes, out int messageID, out byte[] dataBytes);
                if (seriousIndex == deserializedSeriousIndex)
                {
                    //OnMessageReceived(messageID, dataBytes);
                }
                else
                {
                    //OnSessionError(NetworkSessionError.ReadMessageSeriousError);
                    break;
                }
            }
        }

        private byte Desrialize(byte[] bytes, out int messageID, out byte[] dataBytes)
        {
            dataBytes = null;

            int offsetIndex = 0;
            messageID = BitConverter.ToInt32(bytes, offsetIndex);
            offsetIndex += sizeof(int);
            byte seriousIndex = bytes[offsetIndex];
            offsetIndex += sizeof(byte);

            if (bytes.Length - 1 > offsetIndex)
            {
                MessageCompressType compressType = (MessageCompressType)bytes[offsetIndex];
                offsetIndex += sizeof(byte);
                MessageCryptoType cryptoType = (MessageCryptoType)bytes[offsetIndex];
                offsetIndex += sizeof(byte);

                dataBytes = new byte[bytes.Length - offsetIndex];
                Array.Copy(bytes, offsetIndex, dataBytes, 0, dataBytes.Length);
            }
            return seriousIndex;
        }

        private byte[] Serialize(int messageID, MessageCompressType compressType, MessageCryptoType cryptoType, byte[] dataBytes)
        {
            serializeMessageStream.Clear();

            byte[] bodyBytes = dataBytes;
            if (dataBytes != null && dataBytes.Length > 0)
            {
                if(compressType!= MessageCompressType.None)
                {
                    bodyBytes = Handler.CompressMessage(compressType, bodyBytes);
                }
                if(cryptoType!= MessageCryptoType.None)
                {
                    bodyBytes = Handler.EncodeMessage(cryptoType, bodyBytes);
                }
            }

            ++serializedSeriousIndex;

            int messageLen = sizeof(int) + sizeof(byte);
            if (bodyBytes != null && bodyBytes.Length > 0)
            {
                messageLen += sizeof(byte);
                messageLen += sizeof(byte);
                messageLen += bodyBytes.Length;
            }
            byte[] messageLenBytes = BitConverter.GetBytes(messageLen);
            serializeMessageStream.Write(messageLenBytes, 0, messageLenBytes.Length);
            byte[] messageIDBytes = BitConverter.GetBytes(messageID);
            serializeMessageStream.Write(messageIDBytes, 0, messageIDBytes.Length);
            serializeMessageStream.WriteByte(serializedSeriousIndex);
            if (bodyBytes != null && bodyBytes.Length > 0)
            {
                serializeMessageStream.WriteByte((byte)compressType);
                serializeMessageStream.WriteByte((byte)cryptoType);
                serializeMessageStream.Write(bodyBytes, 0, bodyBytes.Length);
            }

            return serializeMessageStream.ToArray();
        }

        private void OnNetError()
        {

        }
    }
}
