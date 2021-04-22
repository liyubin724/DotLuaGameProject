using System.Net.Sockets;

namespace DotEngine.Net
{
    public class ServerNetworkSession : NetworkSession
    {
        public ServerNetworkSession()
        {
            AddAsyncOperationAction(SocketAsyncOperation.Send, ProcessSend);
            AddAsyncOperationAction(SocketAsyncOperation.Receive, ProcessReceive);
            AddAsyncOperationAction(SocketAsyncOperation.Disconnect, ProcessDisconnect);
        }

        public override void DoConnect()
        {
            if(IsConnected)
            {
                OnSessionOperation(NetworkSessionOperation.Connected);

                DoReceive();
            }
        }

        public override void DoConnect(string ip, int port)
        {
            throw new System.NotImplementedException();
        }
    }
}
