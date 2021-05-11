using System;
using System.Net.Sockets;

namespace DotEngine.Net.TCPNetwork
{
    public class TcpSocketEventArgs : EventArgs
    {
        public Socket socket { get; private set; }

        public TcpSocketEventArgs(Socket socket)
        {
            this.socket = socket;
        }
    }
}
