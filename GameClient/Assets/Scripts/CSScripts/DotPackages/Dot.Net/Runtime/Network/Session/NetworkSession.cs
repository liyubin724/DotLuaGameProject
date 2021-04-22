using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace DotEngine.Net
{
    internal class NetworkCachedWillSendMessage
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

    public abstract class NetworkSession : INetworkSession
    {
        private Dictionary<MessageCompressType, IMessageCompressor> compressorDic = new Dictionary<MessageCompressType, IMessageCompressor>();
        private Dictionary<MessageCryptoType, IMessageCryptor> cryptorDic = new Dictionary<MessageCryptoType, IMessageCryptor>();

        private Dictionary<SocketAsyncOperation, Action<SocketAsyncEventArgs>> asyncOperationDic = new Dictionary<SocketAsyncOperation, Action<SocketAsyncEventArgs>>();

        private byte serializedSeriousIndex = 0;
        private NetMessageStream serializeMessageStream = new NetMessageStream();

        private byte deserializedSeriousIndex = 0;
        private NetMessageBuffer receivedMessageBuffer = new NetMessageBuffer();

        private GenericObjectPool<NetworkCachedWillSendMessage> willSendMessagePool = new GenericObjectPool<NetworkCachedWillSendMessage>(
                () => new NetworkCachedWillSendMessage(),
                null,
                (nsm) =>
                {
                    nsm.DoReset();
                });
        private object cachedWillSendMessageLocker = new object();
        private List<NetworkCachedWillSendMessage> cachedWillSendMessages = new List<NetworkCachedWillSendMessage>();

        private object isSendingLocker = new object();
        private bool isSending = false;

        private static readonly int RECEIVE_BUFFER_SIZE = 4096;

        private SocketAsyncEventArgs sendAsyncEvent = null;
        private SocketAsyncEventArgs receiveAsyncEvent = null;

        public Socket NetSocket { get; private set; }
        public INetworkSessionHandler SessionHandler { get; private set; }

        public bool IsConnected => NetSocket != null && NetSocket.Connected;

        public NetworkSession()
        {
        }

        public void BindSocket(Socket socket, INetworkSessionHandler handler)
        {
            NetSocket = socket;
            SessionHandler = handler;
        }

        public void AddCompressor(MessageCompressType compressType, IMessageCompressor compressor)
        {
            if (!compressorDic.ContainsKey(compressType))
            {
                compressorDic.Add(compressType, compressor);
            }
        }

        public void RemoveCompressor(MessageCompressType compressType)
        {
            if (compressorDic.ContainsKey(compressType))
            {
                compressorDic.Remove(compressType);
            }
        }

        public void AddCryptor(MessageCryptoType cryptoType, IMessageCryptor cryptor)
        {
            if (!cryptorDic.ContainsKey(cryptoType))
            {
                cryptorDic.Add(cryptoType, cryptor);
            }
        }

        public void RemoveCryptor(MessageCryptoType cryptoType)
        {
            if (cryptorDic.ContainsKey(cryptoType))
            {
                cryptorDic.Remove(cryptoType);
            }
        }

        public void OnDataReceived(byte[] bytes, int size)
        {
            receivedMessageBuffer.WriteBytes(bytes, size);

            byte[] messageBytes = receivedMessageBuffer.ReadMessage();
            while(messageBytes!=null)
            {
                ++deserializedSeriousIndex;

                byte seriousIndex = Desrialize(messageBytes, out int messageID, out byte[] dataBytes);
                if(seriousIndex != deserializedSeriousIndex)
                {
                    OnSessionError(NetworkSessionError.ReadMessageSeriousError);
                }else
                {
                    OnMessageReceived(messageID, dataBytes);
                }
            }
        }

        public byte Desrialize(byte[] bytes, out int messageID, out byte[] dataBytes)
        {
            dataBytes = null;

            int offsetIndex = 0;
            messageID = BitConverter.ToInt32(bytes, offsetIndex);
            offsetIndex += sizeof(int);
            byte seriousIndex = bytes[offsetIndex];
            offsetIndex += sizeof(byte);

            if(bytes.Length-1 > offsetIndex)
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

        public byte[] Serialize(int messageID, MessageCompressType compressType, MessageCryptoType cryptoType, byte[] dataBytes)
        {
            serializeMessageStream.Clear();

            byte[] bodyBytes = dataBytes;
            if(dataBytes!=null && dataBytes.Length>0)
            {
                if(compressorDic.TryGetValue(compressType,out var compressor))
                {
                    bodyBytes = compressor.Compress(bodyBytes);
                }
                if(cryptorDic.TryGetValue(cryptoType, out var cryptor))
                {
                    bodyBytes = cryptor.Encode(bodyBytes);
                }
            }

            ++serializedSeriousIndex;

            int messageLen = sizeof(int) + sizeof(byte);
            if(bodyBytes!=null && bodyBytes.Length>0)
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

        public void OnMessageReceived(int messageID, byte[] dataBytes)
        {
            SessionHandler?.OnMessageHandler(messageID, dataBytes);
        }

        public void SendMessage(int messageID, MessageCompressType compressType, MessageCryptoType cryptoType, byte[] dataBytes)
        {
            lock(cachedWillSendMessageLocker)
            {
                var willSendMessage = willSendMessagePool.Get();
                willSendMessage.ID = messageID;
                willSendMessage.CompressType = compressType;
                willSendMessage.CryptoType = cryptoType;
                willSendMessage.DataBytes = dataBytes;

                cachedWillSendMessages.Add(willSendMessage);
            }

            DoSend();
        }

        public void Dispose()
        {

        }

        protected void AddAsyncOperationAction(SocketAsyncOperation operation, Action<SocketAsyncEventArgs> action)
        {
            if (!asyncOperationDic.ContainsKey(operation))
            {
                asyncOperationDic.Add(operation, action);
            }
        }

        protected void RemoveAsyncOperationAction(SocketAsyncOperation operation)
        {
            if (asyncOperationDic.ContainsKey(operation))
            {
                asyncOperationDic.Remove(operation);
            }
        }

        protected void OnHandleSocketEvent(object sender, SocketAsyncEventArgs socketEvent)
        {
            if(socketEvent.SocketError == SocketError.Success)
            {
                if(asyncOperationDic.TryGetValue(socketEvent.LastOperation,out var action))
                {
                    action(socketEvent);
                }
            }else
            {
                OnSessionError(NetworkSessionError.HandleSocketEventError, socketEvent);
            }
        }

        public void DoReceive()
        {
            if(!IsConnected)
            {
                return;
            }

            if(receiveAsyncEvent == null)
            {
                receiveAsyncEvent = new SocketAsyncEventArgs();
                receiveAsyncEvent.SetBuffer(new byte[RECEIVE_BUFFER_SIZE], 0, RECEIVE_BUFFER_SIZE);
                receiveAsyncEvent.Completed += OnHandleSocketEvent;
            }

            try
            {
                if(NetSocket.ReceiveAsync(receiveAsyncEvent))
                {
                    return;
                }
            }catch(Exception e)
            {

            }
        }

        protected void DoSend()
        {
            if(!IsConnected)
            {
                return;
            }

            lock(isSendingLocker)
            {
                if(isSending)
                {
                    return;
                }
            }

            byte[] sendBytes = null;
            lock(cachedWillSendMessageLocker)
            {
                if(cachedWillSendMessages.Count>0)
                {
                    var willSendMessage = cachedWillSendMessages[0];
                    cachedWillSendMessages.RemoveAt(0);

                    sendBytes = Serialize(willSendMessage.ID, willSendMessage.CompressType, willSendMessage.CryptoType, willSendMessage.DataBytes);

                    willSendMessagePool.Release(willSendMessage);
                }else
                {
                    return;
                }
            }
            lock(isSendingLocker)
            {
                isSending = true;
            }

            if (sendAsyncEvent == null)
            {
                sendAsyncEvent = new SocketAsyncEventArgs();
                sendAsyncEvent.Completed += OnHandleSocketEvent;
            }
            sendAsyncEvent.SetBuffer(sendBytes, 0, sendBytes.Length);
            if (NetSocket.SendAsync(sendAsyncEvent))
            {
                return;
            }

            OnSessionError(NetworkSessionError.DoSendFailedError);
        }

        protected void DoDisconnect()
        {
            if(sendAsyncEvent !=null)
            {
                sendAsyncEvent.Completed -= OnHandleSocketEvent;
                sendAsyncEvent = null;
            }

            if(receiveAsyncEvent != null)
            {
                receiveAsyncEvent.Completed -= OnHandleSocketEvent;
                receiveAsyncEvent = null;
            }
            lock(cachedWillSendMessageLocker)
            {
                cachedWillSendMessages.Clear();
            }
            
            if(IsConnected)
            {
                try
                {
                    NetSocket.Shutdown(SocketShutdown.Both);
                }catch(Exception e)
                {

                }finally
                {
                    NetSocket.Close();
                }
            }
            NetSocket = null;

        }

        protected void ProcessSend(SocketAsyncEventArgs socketEvent)
        {
            if(socketEvent.SocketError == SocketError.Success)
            {
                lock(isSendingLocker)
                {
                    isSending = false;
                }

                DoSend();
                return;
            }
            OnSessionError(NetworkSessionError.ProcessSendSocketError, socketEvent);
        }

        protected void ProcessReceive(SocketAsyncEventArgs socketEvent)
        {
            if(socketEvent.SocketError == SocketError.Success)
            {
                if(socketEvent.BytesTransferred>0)
                {
                    OnDataReceived(socketEvent.Buffer, socketEvent.BytesTransferred);
                    return;
                }
            }

            SessionHandler?.OnSessionError(NetworkSessionError.ProcessReceiveSocketError, this, socketEvent);
        }

        protected void ProcessDisconnect(SocketAsyncEventArgs socketEvent)
        {

        }

        protected void OnSessionError(NetworkSessionError error,object userdata = null)
        {
            SessionHandler?.OnSessionError(error, this, userdata);
        }

        protected void OnSessionOperation(NetworkSessionOperation operation, object userdata = null)
        {
            SessionHandler?.OnSessionOperation(operation, this, userdata);
        }
    }
}
