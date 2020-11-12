using DotEngine.Log;
using DotEngine.Net.Message;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace DotEngine.Net.Client
{
    public enum ClientNetSessionState
    {
        Unavailable = 0,
        Connecting,
        Normal,
        ConnectedFailed,
        Disconnected,
    }

    public class ClientNetSession //: IDispose
    {
        private static readonly string IP_ADDRESS_REGEX = @"^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$";

        private string ip = null;
        private int port = -1;
        public string IP { get => ip; }
        public int Port { get => port; }
        public string Address { get => $"{ip}:{port}"; }
        
        private Socket socket = null;

        private SocketAsyncEventArgs sendAsyncEvent = null;
        private SocketAsyncEventArgs receiveAsyncEvent = null;

        private object sessionStateLock = new object();
        private ClientNetSessionState state = ClientNetSessionState.Unavailable;
        public ClientNetSessionState State 
        {
            get
            {
                lock(sessionStateLock)
                {
                    return state;
                }
            }
            private set
            {
                lock(sessionStateLock)
                {
                    if(state!=value)
                    {
                        state = value;
                    }
                }
            }
         }

        private object sendingLock = new object();
        private bool isSending = false;
        private List<byte> waitingSendBytes = new List<byte>();

        private object receiverLock = new object();
        private MessageReader messageReader = null;

        public ClientNetSession(MessageReader reader)
        {
            messageReader = reader;
        }

        public bool IsConnected()
        {
            return State == ClientNetSessionState.Normal;
        }

        public bool Connect(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                LogUtil.Error(NetConst.CLIENT_LOGGER_TAG, "ClientNetSession::Connect->ipAddress is empty");
                return false;
            }

            string[] splitStrArr = ipAddress.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitStrArr == null || splitStrArr.Length != 2)
            {
                LogUtil.Error(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::Connect->The lenght of ip is not correct.ipAddress = {ipAddress}");
                return false;
            }

            if (!int.TryParse(splitStrArr[1], out int port) || port<=0)
            {
                LogUtil.Error(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::Connect->the port is not a int.port = {splitStrArr[1]}");
                return false;
            }

            return Connect(splitStrArr[0], port);
        }

        public bool Connect(string ip,int port)
        {
            if(string.IsNullOrEmpty(ip) || port<=0)
            {
                LogUtil.Error(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::Connect->The ip is empty or the port is not correct.ip = {ip},port={port}");
                return false;
            }

            if(!Regex.IsMatch(ip,IP_ADDRESS_REGEX))
            {
                LogUtil.Error(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::Connect->The format of ip is not correct.ip = {ip}");
                return false;
            }

            if(socket == null)
            {
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    this.ip = ip;
                    this.port = port;

                    State = ClientNetSessionState.Connecting;

                    IPAddress ipAddress = IPAddress.Parse(ip);
                    SocketAsyncEventArgs connectAsyncEvent = new SocketAsyncEventArgs()
                    {
                        RemoteEndPoint = new IPEndPoint(ipAddress, port),
                        UserToken = socket,
                    };
                    connectAsyncEvent.Completed += OnHandleSocketEvent;

                    LogUtil.Info(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::Connect->Begin connecting... (Address = {Address})");

                    socket.ConnectAsync(connectAsyncEvent);
                    return true;
                }catch(Exception e)
                {
                    LogUtil.Error(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::Connect->{e.Message}");
                    State = ClientNetSessionState.ConnectedFailed;
                    socket = null;
                    return false;
                }
            }else
            {
                LogUtil.Error(NetConst.CLIENT_LOGGER_TAG, "ClientNetSession::Connect->The socket has been created.");
            }
            return false;
        }

        public bool Reconnect()
        {
            if(string.IsNullOrEmpty(ip) || port<=0)
            {
                LogUtil.Error(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::Reconnect->the ip is empty,or the port is not correct.");
                return false;
            }

            if(State >= ClientNetSessionState.ConnectedFailed)
            {
                return Connect(ip, port);
            }

            LogUtil.Error(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::Reconnect->the net is connected or the net is connecting.{State}");

            return false;
        }

        public void Disconnect()
        {
            Close();
            State = ClientNetSessionState.Disconnected;
        }

        public void Send(byte[] datas)
        {
            if(State != ClientNetSessionState.Normal)
            {
                return;
            }
            lock(sendingLock)
            {
                waitingSendBytes.AddRange(datas);
            }
        }

       private void Receive()
        {
            try
            {
                if(!socket.ReceiveAsync(receiveAsyncEvent))
                {
                    Disconnect();
                }
            }catch(Exception e)
            {
                LogUtil.Error(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::Receive->{e.Message}");
                Disconnect();
            }
        }

        private void Close()
        {
            LogUtil.Info(NetConst.CLIENT_LOGGER_TAG, "ClientNetSession::Close->Closed Session");
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
            }

            if (socket != null)
            {
                if (socket.Connected)
                {
                    try
                    {
                        socket.Shutdown(SocketShutdown.Both);
                    }
                    catch
                    {

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

        private void OnHandleSocketEvent(object sender,SocketAsyncEventArgs socketEvent)
        {
            switch (socketEvent.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    ProcessConnect(socketEvent);
                    break;
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
                    LogUtil.Warning(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::OnHandleSocketEvent->received unkown event.opration = {socketEvent.LastOperation}");
                    break;
            }
        }

        private void ProcessConnect(SocketAsyncEventArgs socketEvent)
        {
            socketEvent.Completed -= OnHandleSocketEvent;

            if(socketEvent.SocketError == SocketError.Success)
            {
                State = ClientNetSessionState.Normal;

                LogUtil.Info(NetConst.CLIENT_LOGGER_TAG, "ClientNetSession::ProcessConnect->Connect success");

                receiveAsyncEvent = new SocketAsyncEventArgs();
                receiveAsyncEvent.SetBuffer(new byte[NetConst.BUFFER_SIZE], 0, NetConst.BUFFER_SIZE);
                receiveAsyncEvent.Completed += OnHandleSocketEvent;

                Receive();
            }else
            {
                LogUtil.Info(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::ProcessConnect->Connect failed.error = {socketEvent.SocketError}");
                Close();
                State = ClientNetSessionState.ConnectedFailed;
            }
        }

        private void ProcessSend(SocketAsyncEventArgs socketEvent)
        {
            if(socketEvent.SocketError == SocketError.Success)
            {
                LogUtil.Info(NetConst.CLIENT_LOGGER_TAG, "ClientNetSession::ProcessSend->Send success");
                lock (sendingLock)
                {
                    isSending = false;
                }
            }else
            {
                Disconnect();
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs socketEvent)
        {
            if(socketEvent.SocketError == SocketError.Success)
            {
                if(socketEvent.BytesTransferred>0)
                {
                    LogUtil.Info(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::ProcessReceive->Received message.length = {socketEvent.BytesTransferred}");
                    lock (receiverLock)
                    {
                        messageReader.OnDataReceived(socketEvent.Buffer, socketEvent.BytesTransferred);
                    }
                    Receive();
                    return;
                }
            }
            Disconnect();
        }

        private void ProcessDisconnect(SocketAsyncEventArgs socketEvent)
        {
            Disconnect();
        }

        internal void DoLateUpdate()
        {
            if (State != ClientNetSessionState.Normal)
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
                        LogUtil.Error(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::DoLateUpdate->e = {e.Message}");
                        Disconnect();
                    }
                }
            }

            lock(receiverLock)
            {
                messageReader?.DoReadData();
            }
        }

        public void Dispose()
        {
            Close();
        }
    }

}
