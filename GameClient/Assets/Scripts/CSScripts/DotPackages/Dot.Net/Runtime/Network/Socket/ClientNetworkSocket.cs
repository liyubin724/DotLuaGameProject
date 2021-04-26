using System;
using System.Net;
using System.Net.Sockets;

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
            NetworkStates state = State;
            if(state== NetworkStates.Connecting|| state== NetworkStates.Normal|| state==NetworkStates.Disconnecting)
            {
                netHandler.OnOperationLog(NetworkOperations.Working, $"the network is working.({ip}:{port})");
                return;
            }

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
                State = NetworkStates.ConnectedFailed;
                netHandler.OnOperationLog(NetworkOperations.ConnectedFailed, $"connected failed({ip}:{port})");
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
                State = NetworkStates.Normal;
                netHandler.OnOperationLog(NetworkOperations.Connected,"Connect to the server sucess");
                
                Receive();
            }else
            {
                State = NetworkStates.ConnectedFailed;
                netHandler.OnOperationLog(NetworkOperations.ConnectedFailed, "Connect to the server failed");
            }
        }
    }

}
