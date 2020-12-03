using System;
using System.Net.Sockets;

namespace DotEngine.Network
{
    public class ReceiveEventArgs : EventArgs
    {
        public Socket client { get; private set; }
        public byte[] bytes { get; private set; }

        public ReceiveEventArgs(Socket client, byte[] bytes)
        {
            this.client = client;
            this.bytes = bytes;
        }
    }
}

