using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net.Services
{
    public static class NetNotification
    {
        public const string CLIENT_NET_CREATE = "OnNetCreate";

        public const string CLIENT_NET_CONNECDTING = "DoNetConnecting";
        public const string CLIENT_NET_CONNECTED_SUCCESS = "DoNetConnectedSuccess";
        public const string CLIENT_NET_CONNECTED_FAILED = "DoNetConnectedFailed";
        public const string CLIENT_NET_DISCONNECTED = "DoNetDisconnected";
    }
}
