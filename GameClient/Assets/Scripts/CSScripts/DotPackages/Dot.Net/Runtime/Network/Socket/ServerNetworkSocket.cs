using DotEngine.Generic;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace DotEngine.Net
{
    public class ServerNetworkSocket : ANetworkSocket
    {
        public ServerNetworkSocket(Socket socket,INetworkHandler handler):base(socket,handler)
        {
            asyncOperationDic.Add(SocketAsyncOperation.Send, ProcessSend);
            asyncOperationDic.Add(SocketAsyncOperation.Receive, ProcessReceive);
            asyncOperationDic.Add(SocketAsyncOperation.Disconnect, ProcessDisconnect);
        }

        public override void Connect(string ip, int port)
        {
            throw new NotImplementedException();
        }

        public override void Connect()
        {
            State = NetworkStates.Normal;
        }
    }
}
