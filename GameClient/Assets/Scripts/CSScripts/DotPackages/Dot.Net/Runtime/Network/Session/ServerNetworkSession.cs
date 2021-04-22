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

        public void DoConnect()
        {
            if(IsConnected)
            {
                OnSessionOperation(NetworkSessionOperation.Connected);

                DoReceive();
            }
        }
    }
}
