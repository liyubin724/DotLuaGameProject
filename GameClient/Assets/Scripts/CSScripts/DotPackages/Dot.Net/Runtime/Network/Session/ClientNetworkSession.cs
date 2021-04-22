using System;
using System.Net;
using System.Net.Sockets;

namespace DotEngine.Net
{
    public class ClientNetworkSession : NetworkSession
    {
        public ClientNetworkSession()
        {
            AddAsyncOperationAction(SocketAsyncOperation.Connect, ProcessConnect);
            AddAsyncOperationAction(SocketAsyncOperation.Send, ProcessSend);
            AddAsyncOperationAction(SocketAsyncOperation.Receive, ProcessReceive);
            AddAsyncOperationAction(SocketAsyncOperation.Disconnect, ProcessDisconnect);
        }

        public void DoConnect(string ip, int port)
        {
            if (NetSocket != null)
            {
                if (!IsConnected)
                {
                    try
                    {
                        IPAddress ipAddress = IPAddress.Parse(ip);
                        SocketAsyncEventArgs connectAsyncEvent = new SocketAsyncEventArgs()
                        {
                            RemoteEndPoint = new IPEndPoint(ipAddress, port),
                            UserToken = NetSocket,
                        };
                        connectAsyncEvent.Completed += OnHandleSocketEvent;

                        OnSessionOperation(NetworkSessionOperation.Connecting);

                        NetSocket.ConnectAsync(connectAsyncEvent);
                    }catch(Exception e)
                    {

                    }
                }
            }
        }

        private void ProcessConnect(SocketAsyncEventArgs socketEvent)
        {
            socketEvent.Completed -= OnHandleSocketEvent;

            if (socketEvent.SocketError == SocketError.Success)
            {
                OnSessionOperation(NetworkSessionOperation.Connected);

                DoReceive();
            }
            else
            {
                OnSessionError(NetworkSessionError.ProcessConnectSocketError, socketEvent);
            }
        }
    }
}
