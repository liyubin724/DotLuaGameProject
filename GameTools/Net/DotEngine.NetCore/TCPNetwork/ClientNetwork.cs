using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TcpClient = NetCoreServer.TcpClient;

namespace DotEngine.NetCore.TCPNetwork
{
    public enum ClientNetworkState
    {
        Unreachable = 0,
        Connected,
        Disconnected,
    }

    public interface IClientNetHandler
    {
        void OnNetworkStateChanged(ClientNetworkState state);

    }

    public class ClientNet : TcpClient
    {
        private ClientNetworkState networkState = ClientNetworkState.Unreachable;
        
        public ClientNetworkState State => networkState;
        private object stateLocker = new object();

        public ClientNet(IPAddress address, int port) : base(address, port)
        {
        }

        protected override void OnConnected()
        {
            lock(stateLocker)
            {
                networkState = ClientNetworkState.Connected;
            }
        }

        protected override void OnDisconnected()
        {
            lock (stateLocker)
            {
                networkState = ClientNetworkState.Disconnected;
            }
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            
        }

        protected override void OnError(SocketError error)
        {
            
        }
    }

    public class ClientNetwork : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
