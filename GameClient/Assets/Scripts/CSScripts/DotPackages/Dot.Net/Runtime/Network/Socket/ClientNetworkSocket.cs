using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;

namespace DotEngine.Net
{
    public delegate void ClientMessageHandler(int messageID, byte[] dataBytes);

    public delegate void ClientNetworkStateChanged(ClientNetworkSocket networkSocket);

    public enum ClientNetworkState
    {
        Unavailable = 0,
        Connecting,
        Normal,
        ConnectedFailed,
        Disconnected,
    }

    public class ClientNetworkSocket : INetworkSessionHandler
    {
        private Socket socket = null;
        private INetworkSession session = null;

        private Dictionary<int, ClientMessageHandler> messageHandlerDic = new Dictionary<int, ClientMessageHandler>();
        public ClientMessageHandler FiniallyMessageHandler { get; set; } = null;

        public event ClientNetworkStateChanged NetConnecting;
        public event ClientNetworkStateChanged NetConnectedSuccess;
        public event ClientNetworkStateChanged NetConnectedFailed;
        public event ClientNetworkStateChanged NetDisconnected;

        private ClientNetworkState state = ClientNetworkState.Unavailable;

        public ClientNetworkSocket() : base()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            session = new ClientNetworkSession();
            session.BindSocket(socket, this);
        }

        public void Startup(string ip,int port)
        {
            session.DoConnect(ip, port);
        }

        public void Shuntdown()
        {

        }

        public void RegistMessageHandler(int messageID, ClientMessageHandler handler)
        {
            if (!messageHandlerDic.ContainsKey(messageID))
            {
                messageHandlerDic.Add(messageID, handler);
            }
        }

        public void UnregistMessageHandler(int messageID)
        {
            if (messageHandlerDic.ContainsKey(messageID))
            {
                messageHandlerDic.Remove(messageID);
            }
        }

        public void RegistMessageHandler(Type handlerType)
        {
            var methodInfos = handlerType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methodInfos)
            {
                var attr = method.GetCustomAttribute<ClientMessageHandlerAttribute>();
                if (attr != null && attr.ID > 0)
                {
                    if (Delegate.CreateDelegate(typeof(ClientMessageHandler), null, method) is ClientMessageHandler messageHandler)
                    {
                        RegistMessageHandler(attr.ID, messageHandler);
                    }
                }
            }
        }

        public void UnregistMessagehandler(Type handlerType)
        {
            var methodInfos = handlerType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methodInfos)
            {
                var attr = method.GetCustomAttribute<ClientMessageHandlerAttribute>();
                if (attr != null && attr.ID > 0)
                {
                    UnregistMessageHandler(attr.ID);
                }
            }
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {

        }

        public void OnMessageHandler(INetworkSession session, int messageID, byte[] dataBytes)
        {
            if(messageHandlerDic.TryGetValue(messageID,out var handler))
            {
                handler(messageID, dataBytes);
            }else if(FiniallyMessageHandler !=null)
            {
                FiniallyMessageHandler(messageID, dataBytes);
            }
        }

        public void OnSessionError(NetworkSessionError error, INetworkSession session, object userdata)
        {
            
        }

        public void OnSessionOperation(NetworkSessionOperation operation, INetworkSession session, object userdata)
        {
           
        }

        public void OnSessionLog(NetworkSessionLogType logType, string log)
        {
            
        }
    }
}
