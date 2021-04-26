using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace DotEngine.Net
{
    public class ClientNetworkSocket :ANetworkSocket
    {
        public ClientNetworkSocket(Socket socket, INetworkHandler handler) : base(socket, handler)
        {
            asyncOperationDic.Add(SocketAsyncOperation.Connect,ProcessConnect);
            asyncOperationDic.Add(SocketAsyncOperation.Send,ProcessSend);
            asyncOperationDic.Add(SocketAsyncOperation.Receive,ProcessReceive);
            asyncOperationDic.Add(SocketAsyncOperation.Disconnect,ProcessDisconnect);
        }

        public override void Connect(string ip,int port)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ip);
                SocketAsyncEventArgs connectAsyncEvent = new SocketAsyncEventArgs()
                {
                    RemoteEndPoint = new IPEndPoint(ipAddress, port),
                    UserToken = netSocket,
                };
                connectAsyncEvent.Completed += OnHandleSocketEvent;

                netSocket.ConnectAsync(connectAsyncEvent);
                State = NetworkStates.Connecting;

                netHandler.OnOperationLog(NetworkOperations.Connecting,$"Start connect to the server({ip}:{port})");
            }
            catch
            {

            }
        }

        public override void Connect()
        {
            throw new NotImplementedException();
        }

        private void ProcessConnect(SocketAsyncEventArgs socketEvent)
        {
            socketEvent.Completed -= OnHandleSocketEvent;
            if(socketEvent.SocketError == SocketError.Success)
            {
                netHandler.OnOperationLog(NetworkOperations.Connected,"Connect to the server sucess");
                DoReceive();
            }else
            {
                DoDisconnectByError(NetworkDisconnectErrors.ConnectError);
            }
            
        }
    }

}
