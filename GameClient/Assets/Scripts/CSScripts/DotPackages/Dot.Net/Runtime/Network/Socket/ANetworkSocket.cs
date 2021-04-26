﻿using System;
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
        Disconnecting,
        Disconnected,
    }

    public enum NetworkDisconnectErrors
    {
        None = 0,
        ConnectError,
        DoReceiveError,
        ProcessReceiveError,
        ReceivedMessageSerousIndexError,
        DoSendError,
        ProcessSendError,
    }

    public enum NetworkOperations
    {
        None = 0,
        Working,
        Connecting,
        Connected,
        ConnectedFailed,
        Sending,
        Sended,
        Receiving,
        ReceiveingError,
        ReceivedBytes,
        ReceiveMessage,
        Disconnecting,
        Disconnected,
        DisconnectedByError,
    }

    public abstract class ANetworkSocket
    {
        private static readonly int RECEIVE_BUFFER_SIZE = 1024;

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
            protected set
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
        private NetMessageBuffer receivedMessageBuffer = null;

        private object isSendingLocker = new object();
        private bool isSending = false;
        private byte sendedMessageSeriousIndex = 0;
        private object sendingMessageStreamLocker = new object();
        private NetMessageStream sendingMessageStream = new NetMessageStream();

        private SocketAsyncEventArgs sendAsyncEvent = null;
        private SocketAsyncEventArgs receiveAsyncEvent = null;

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
            if (!IsConnected)
            {
                return;
            }

            lock (sendingMessageStreamLocker)
            {
                Serialize(messageID, compressType, cryptoType, dataBytes);
            }

            DoSend();
        }

        protected void Receive()
        {
            receivedMessageBuffer = new NetMessageBuffer();
            receivedMessageSeriousIndex = 0;

            DoReceive();
        }

        public void Disconnect()
        {

        }

        private void DoReceive()
        {
            if(!IsConnected)
            {
                return;
            }
            if(receiveAsyncEvent == null)
            {
                receiveAsyncEvent = new SocketAsyncEventArgs();
                receiveAsyncEvent.Completed += OnHandleSocketEvent;
                receiveAsyncEvent.SetBuffer(new byte[RECEIVE_BUFFER_SIZE],0,RECEIVE_BUFFER_SIZE);
            }
            netHandler.OnOperationLog(NetworkOperations.Receiving,"Start receiving");
            try
            {
                if(netSocket.ReceiveAsync(receiveAsyncEvent))
                {
                    return;
                }
            }catch(Exception e)
            {
                netHandler.OnOperationLog(NetworkOperations.ReceiveingError, e.Message);
            }

            DoDisconnectByError(NetworkDisconnectErrors.DoReceiveError);
        }

        private void DoSend()
        {
            if(!IsConnected)
            {
                return;
            }
            if(sendAsyncEvent == null)
            {
                sendAsyncEvent = new SocketAsyncEventArgs();
                sendAsyncEvent.Completed += OnHandleSocketEvent;
            }

            lock(isSendingLocker)
            {
                if(isSending)
                {
                    return;
                }
            }

            byte[] sendingBytes = null;
            lock(sendingMessageStreamLocker)
            {
                sendingBytes = sendingMessageStream.ToArray();
                sendingMessageStream.Clear();
            }

            if(sendingBytes == null || sendingBytes.Length == 0)
            {
                return;
            }

            lock (isSendingLocker)
            {
                isSending = true;
            }

            sendAsyncEvent.SetBuffer(sendingBytes, 0, sendingBytes.Length);
            if(!netSocket.SendAsync(sendAsyncEvent))
            {
                DoDisconnectByError(NetworkDisconnectErrors.DoSendError);
            }
        }

        private void DoDisconnect()
        {
            if(sendAsyncEvent != null)
            {
                sendAsyncEvent.Completed -= OnHandleSocketEvent;
                sendAsyncEvent = null;
            }
            if(receiveAsyncEvent !=null)
            {
                receiveAsyncEvent.Completed -= OnHandleSocketEvent;
                receiveAsyncEvent = null;
            }
            if(State == NetworkStates.Connecting || State == NetworkStates.Normal)
            {
                SocketAsyncEventArgs disconnectAsyncEvent = new SocketAsyncEventArgs();
                disconnectAsyncEvent.Completed += OnHandleSocketEvent;

                State = NetworkStates.Disconnecting;

                netSocket.DisconnectAsync(disconnectAsyncEvent);
            }

            lock(sendingMessageStreamLocker)
            {
                sendingMessageStream.Clear();
            }

        }

        protected void DoDisconnectByError(NetworkDisconnectErrors error)
        {
            DoDisconnect();
        }

        protected void OnHandleSocketEvent(object sender, SocketAsyncEventArgs socketEvent)
        {
            if (asyncOperationDic.TryGetValue(socketEvent.LastOperation, out var action))
            {
                action(socketEvent);
            }
        }

        protected void ProcessReceive(SocketAsyncEventArgs socketEvent)
        {
            if (socketEvent.SocketError == SocketError.Success)
            {
                if (socketEvent.BytesTransferred > 0)
                {
                    netHandler.OnOperationLog(NetworkOperations.ReceivedBytes,$"Received bytes.length = {socketEvent.BytesTransferred}");

                    OnDataReceived(socketEvent.Buffer, socketEvent.BytesTransferred);

                    DoReceive();
                    return;
                }
            }

            DoDisconnectByError(NetworkDisconnectErrors.ProcessReceiveError);
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

            DoDisconnectByError(NetworkDisconnectErrors.ProcessSendError);
        }

        protected void ProcessDisconnect(SocketAsyncEventArgs socketEvent)
        {

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
                    DoDisconnectByError(NetworkDisconnectErrors.ReceivedMessageSerousIndexError);
                    break;
                }else
                {
                    netHandler.OnMessageHandler(this, messageID, dataBytes);
                }
            }
        }

        private void Serialize(int messageID, MessageCompressType compressType, MessageCryptoType cryptoType, byte[] dataBytes)
        {
            byte[] bodyBytes = dataBytes;
            if (dataBytes != null && dataBytes.Length > 0)
            {
                bodyBytes = netHandler.CompressMessage(compressType, bodyBytes);
                bodyBytes = netHandler.EncodeMessage(cryptoType, bodyBytes);
            }

            int messageLen = sizeof(int) + sizeof(byte);
            if (bodyBytes != null && bodyBytes.Length > 0)
            {
                messageLen += sizeof(byte);
                messageLen += sizeof(byte);
                messageLen += bodyBytes.Length;
            }
            byte[] messageLenBytes = BitConverter.GetBytes(messageLen);
            sendingMessageStream.Write(messageLenBytes, 0, messageLenBytes.Length);
            byte[] messageIDBytes = BitConverter.GetBytes(messageID);
            sendingMessageStream.Write(messageIDBytes, 0, messageIDBytes.Length);
            sendingMessageStream.WriteByte(sendedMessageSeriousIndex);
            if (bodyBytes != null && bodyBytes.Length > 0)
            {
                sendingMessageStream.WriteByte((byte)compressType);
                sendingMessageStream.WriteByte((byte)cryptoType);
                sendingMessageStream.Write(bodyBytes, 0, bodyBytes.Length);
            }

            ++sendedMessageSeriousIndex;
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