using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public enum NetworkState
    {
        Unavailable = 0,
        Connecting,
        Normal,
        Disconnecting,
        ConnectedFailed,
        Disconnected,
    }

    public abstract class NetworkSocket
    {
        private static readonly string IP_ADDRESS_REGEX = @"^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$";

        protected Socket socket = null;
        protected string LogTag { get; private set; }
        public NetworkSocket(string logTag, INetworkSession session)
        {
            LogTag = logTag;
        }

        public bool Startup(string ip,int port)
        {
            if (string.IsNullOrEmpty(ip) || port <= 0)
            {
                DebugLog.Error(LogTag, $"ClientNetSession::Connect->The ip is empty or the port is not correct.ip = {ip},port={port}");
                return false;
            }

            if (!Regex.IsMatch(ip, IP_ADDRESS_REGEX))
            {
                DebugLog.Error(NetConst.CLIENT_LOGGER_TAG, $"ClientNetSession::Connect->The format of ip is not correct.ip = {ip}");
                return false;
            }
            if(socket == null)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                return DoStartup();
            }

            return false;
        }

        protected abstract bool DoStartup();

        public void Shuntdown()
        {

        }

        public void SendData(byte[] datas)
        {

        }

        protected void DoReceive()
        {

        }
    }
}
