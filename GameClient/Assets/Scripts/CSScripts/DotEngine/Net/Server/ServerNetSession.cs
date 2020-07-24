using DotEngine.Log;
using DotEngine.Net.Message;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace DotEngine.Net.Server
{
    public enum ServerNetSessionState
    {
        Unavailable = 0,
        Normal,
        Disconnected,
    }

    public class ServerNetSession //: IDispose
    {
        private Socket socket = null;
        private MessageReader messageReader = null;

        private SocketAsyncEventArgs sendAsyncEvent = null;
        private object sendingLock = new object();
        private bool isSending = false;
        private List<byte> waitingSendBytes = new List<byte>();

        private SocketAsyncEventArgs receiveAsyncEvent = null;
        private object receiverLock = new object();

        private object sessionStateLock = new object();
        private ServerNetSessionState state = ServerNetSessionState.Unavailable;
        public ServerNetSessionState State
        {
            get
            {
                lock (sessionStateLock)
                {
                    return state;
                }
            }
            private set
            {
                lock (sessionStateLock)
                {
                    if (state != value)
                    {
                        state = value;
                    }
                }
            }
        }

        public ServerNetSession(Socket socket, MessageReader reader)
        {
            this.socket = socket;
            messageReader = reader;

            State = ServerNetSessionState.Normal;

            Receive();
        }

        public bool IsConnected()
        {
            return State == ServerNetSessionState.Normal;
        }

        private void Receive()
        {
            if (receiveAsyncEvent == null)
            {
                receiveAsyncEvent = new SocketAsyncEventArgs();
                receiveAsyncEvent.SetBuffer(new byte[NetConst.BUFFER_SIZE], 0, NetConst.BUFFER_SIZE);
                receiveAsyncEvent.Completed += OnHandleSocketEvent;
            }

            try
            {
                if (!socket.ReceiveAsync(receiveAsyncEvent))
                {
                    Disconnect();
                }
            }
            catch (Exception e)
            {
                LogUtil.LogError(NetConst.SERVER_LOGGER_TAG, $"ServerNetSession::Receive->Receive message cased a error.message = {e.Message}");
                Disconnect();
            }
        }

        public void Send(byte[] datas)
        {
            lock (sendingLock)
            {
                waitingSendBytes.AddRange(datas);
            }
        }

        private void Close()
        {
            if (sendAsyncEvent != null)
            {
                sendAsyncEvent.Completed -= OnHandleSocketEvent;
                sendAsyncEvent = null;
            }
            if (receiveAsyncEvent != null)
            {
                receiveAsyncEvent.Completed -= OnHandleSocketEvent;
                receiveAsyncEvent = null;
            }

            lock (sendingLock)
            {
                waitingSendBytes.Clear();
                isSending = false;
            }

            if (socket != null)
            {
                if (socket.Connected)
                {
                    try
                    {
                        socket.Shutdown(SocketShutdown.Both);
                    }
                    catch (Exception e)
                    {
                        LogUtil.LogError(NetConst.SERVER_LOGGER_TAG, $"ServerNetSession::Disconnect->e = {e.Message}");
                    }
                    finally
                    {
                        socket.Close();
                        socket = null;
                    }
                }
                else
                {
                    socket.Close();
                    socket = null;
                }
            }
        }

        public void Disconnect()
        {
            Close();
            State = ServerNetSessionState.Disconnected;
        }

        private void OnHandleSocketEvent(object sender, SocketAsyncEventArgs socketEvent)
        {
            switch(socketEvent.LastOperation)
            {
                case SocketAsyncOperation.Send:
                    ProcessSend(socketEvent);
                    break;
                case SocketAsyncOperation.Receive:
                    ProcessReceive(socketEvent);
                    break;
                case SocketAsyncOperation.Disconnect:
                    ProcessDisconnect(socketEvent);
                    break;
                default:
                    LogUtil.LogWarning(NetConst.SERVER_LOGGER_TAG, $"ClientNetSession::OnHandleSocketEvent->received unkown event.opration = {socketEvent.LastOperation}");
                    break;
            }
        }

        private void ProcessSend(SocketAsyncEventArgs socketEvent)
        {
            if (socketEvent.SocketError == SocketError.Success)
            {
                lock (sendingLock)
                {
                    isSending = false;
                }
            }
            else
            {
                LogUtil.LogError(NetConst.SERVER_LOGGER_TAG, $"ServerNetSession::ProcessSend->send message error.error = {socketEvent.SocketError}");
                Disconnect();
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs socketEvent)
        {
            if (socketEvent.SocketError == SocketError.Success)
            {
                if (socketEvent.BytesTransferred > 0)
                {
                    lock (receiverLock)
                    {
                        messageReader.OnDataReceived(socketEvent.Buffer, socketEvent.BytesTransferred);
                    }

                    Receive();
                    return;
                }
            }
            LogUtil.LogError(NetConst.SERVER_LOGGER_TAG, $"ServerNetSession::ProcessReceive->Receive message error.error = {socketEvent.SocketError}");
            Disconnect();
        }

        private void ProcessDisconnect(SocketAsyncEventArgs socketEvent)
        {
            Disconnect();
        }

        internal void DoLateUpdate()
        {
            if(State != ServerNetSessionState.Normal)
            {
                return;
            }
            lock (sendingLock)
            {
                if (waitingSendBytes.Count > 0 && !isSending)
                {
                    try
                    {
                        if (sendAsyncEvent == null)
                        {
                            sendAsyncEvent = new SocketAsyncEventArgs();
                            sendAsyncEvent.Completed += OnHandleSocketEvent;
                        }
                        sendAsyncEvent.SetBuffer(waitingSendBytes.ToArray(), 0, waitingSendBytes.Count);
                        waitingSendBytes.Clear();
                        if (!socket.SendAsync(sendAsyncEvent))
                        {
                            Disconnect();
                        }
                        else
                        {
                            isSending = true;
                        }
                    }
                    catch (Exception e)
                    {
                        LogUtil.LogError(NetConst.SERVER_LOGGER_TAG, $"ServerNetSession::DoLateUpdate->e = {e.Message}");
                        Disconnect();
                    }
                }
            }

            lock (receiverLock)
            {
                messageReader.DoReadData();
            }
        }

        public void Dispose()
        {
            Close();

            messageReader = null;
            State = ServerNetSessionState.Unavailable;
        }
    }
}
