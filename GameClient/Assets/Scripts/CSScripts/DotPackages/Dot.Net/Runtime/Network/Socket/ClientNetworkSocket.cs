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
            }
            catch
            {

            }
        }

        public override void Connect()
        {
            throw new NotImplementedException();
        }
    }

}
