﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public enum NetSessionState
    {
        Unavailable = 0,
        Connecting,
        Normal,
        ConnectedFailed,
        Disconnected,
    }


    public abstract class NetworkSession : INetworkSession
    {
        private static readonly int RECEIVE_BUFFER_SIZE = 1024;

        private NetMessageBuffer receivedMessageBuffer = new NetMessageBuffer();
        private NetMessageStream sendedMessageStream = new NetMessageStream();

        private Dictionary<MessageCompressType, IMessageCompressor> compressorDic = new Dictionary<MessageCompressType, IMessageCompressor>();
        private Dictionary<MessageCryptoType, IMessageCryptor> cryptorDic = new Dictionary<MessageCryptoType, IMessageCryptor>();

        private Dictionary<SocketAsyncOperation, Action<SocketAsyncEventArgs>> asyncOperationDic = new Dictionary<SocketAsyncOperation, Action<SocketAsyncEventArgs>>();
        
        private SocketAsyncEventArgs sendAsyncEvent = null;
        private SocketAsyncEventArgs receiveAsyncEvent = null;

        private object sendLocker = new object();
        private List<byte[]> cachedMessageList = new List<byte[]>();

        public Socket NetSocket => throw new NotImplementedException();

        public NetworkSession()
        {
            
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

        protected void AddAsyncOperationAction(SocketAsyncOperation operation,Action<SocketAsyncEventArgs> action)
        {
            if(!asyncOperationDic.ContainsKey(operation))
            {
                asyncOperationDic.Add(operation, action);
            }
        }

        protected void RemoveAsyncOperationAction(SocketAsyncOperation operation)
        {
            if(asyncOperationDic.ContainsKey(operation))
            {
                asyncOperationDic.Remove(operation);
            }
        }

        public void OnDataReceived(byte[] bytes, int size)
        {
            receivedMessageBuffer.WriteBytes(bytes, size);
        }

        public byte[] Serialize(int messageID)
        {
            return Serialize(messageID, null);
        }

        public byte[] Serialize(int messageID, byte[] dataBytes)
        {
            return null;
        }

        public void SendMessage(int messageID, MessageCompressType compressType, MessageCryptoType cryptoType, byte[] dataBytes)
        {
            
        }

        private void OnHandleSocketEvent(object sender, SocketAsyncEventArgs socketEvent)
        {
            if(socketEvent.SocketError == SocketError.Success)
            {
                if(asyncOperationDic.TryGetValue(socketEvent.LastOperation,out var action))
                {
                    action(socketEvent);
                }else
                {

                }
            }else
            {

            }
        }

        protected void DoReceive()
        {
            if(receiveAsyncEvent == null)
            {
                receiveAsyncEvent = new SocketAsyncEventArgs();
                receiveAsyncEvent.SetBuffer(new byte[RECEIVE_BUFFER_SIZE], 0, RECEIVE_BUFFER_SIZE);
                receiveAsyncEvent.Completed += OnHandleSocketEvent;
            }

            try
            {
                if(!NetSocket.ReceiveAsync(receiveAsyncEvent))
                {

                }
            }catch(Exception e)
            {

            }
        }

        protected void DoSend()
        {
            if(sendAsyncEvent == null)
            {
                sendAsyncEvent = new SocketAsyncEventArgs();
                sendAsyncEvent.Completed += OnHandleSocketEvent;
            }
            lock (sendLocker)
            {
                if(cachedMessageList.Count>0)
                {
                    byte[] datas = cachedMessageList[0];
                    sendAsyncEvent.SetBuffer(datas, 0, datas.Length);
                    if(!NetSocket.SendAsync(sendAsyncEvent))
                    {

                    }
                }
            }
        }

        protected void DoDisconnect()
        {

        }

        protected void ProcessSend(SocketAsyncEventArgs socketEvent)
        {
            if(socketEvent.SocketError == SocketError.Success)
            {
                lock(sendLocker)
                {
                    cachedMessageList.RemoveAt(0);
                }
                DoSend();

                return;
            }
            //TODO:
        }

        private void ProcessReceive(SocketAsyncEventArgs socketEvent)
        {
            if(socketEvent.SocketError == SocketError.Success)
            {
                if(socketEvent.BytesTransferred>0)
                {
                    receivedMessageBuffer.WriteBytes(socketEvent.Buffer, socketEvent.BytesTransferred);

                    byte[] bytes = receivedMessageBuffer.ReadMessage();
                    while(bytes!=null)
                    {
                        bytes = receivedMessageBuffer.ReadMessage();
                    }
                }
            }
            
            //TODO:
        }

        private void ProcessDisconnect(SocketAsyncEventArgs socketEvent)
        {

        }
    }
}
