using System;
using System.Net.Sockets;

namespace DotEngine.Net.TcpNetwork
{
    public class TcpNetworkEventArgs : EventArgs
    {
        public Socket socket { get; private set; }

        public TcpNetworkEventArgs(Socket socket)
        {
            this.socket = socket;
        }
    }
}

