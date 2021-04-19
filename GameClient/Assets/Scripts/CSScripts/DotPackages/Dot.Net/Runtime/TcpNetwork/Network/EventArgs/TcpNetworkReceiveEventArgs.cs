using System;
using System.Net.Sockets;

namespace DotEngine.Net.TcpNetwork
{
    public class TcpNetworkReceiveEventArgs : EventArgs
    {
        public Socket client { get; private set; }
        public byte[] bytes { get; private set; }

        public TcpNetworkReceiveEventArgs(Socket client, byte[] bytes)
        {
            this.client = client;
            this.bytes = bytes;
        }
    }
}

