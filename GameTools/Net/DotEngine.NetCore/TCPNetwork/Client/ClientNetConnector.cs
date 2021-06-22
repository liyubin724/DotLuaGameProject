using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TcpClient = NetCoreServer.TcpClient;

namespace DotEngine.NetCore.TCPNetwork
{
    public enum ClientNetworkState
    {
        Unreachable = 0,
        Connecting,
        Connected,
        ConnectError,
        Disconnecting,
        Disconnected,
        OtherError,
    }

    public delegate void ClientMessageHandler(int msgID, byte[] msgBytes);

    public class ClientNetConnector : IDisposable,IClientNetworkHandler
    {
        private ClientNetwork network = null;
        
        private ClientNetworkState cachedState = ClientNetworkState.Unreachable;

        private NetMessageBuffer messageBuffer = new NetMessageBuffer();

        private Dictionary<int, ClientMessageHandler> messageHandlerDic = new Dictionary<int, ClientMessageHandler>();


        public bool IsConnected
        {
            get
            {
                return network != null && network.IsConnected;
            }
        }

        public bool IsConnecting
        {
            get
            {
                return network != null && network.IsConnecting;
            }
        }

        public void RegisterMessageHandler(int msgID,ClientMessageHandler handler)
        {
            if(!messageHandlerDic.ContainsKey(msgID))
            {
                messageHandlerDic.Add(msgID, handler);
            }else
            {

            }
        }

        public void RegisterMessageHandler(Type handlerType)
        {
            MethodInfo[] methodInfos = handlerType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if(methodInfos!=null && methodInfos.Length>0)
            {
                foreach(var methodInfo in methodInfos)
                {
                    var attr = methodInfo.GetCustomAttribute<MessageHandlerAttribute>();
                    if(attr!=null)
                    {
                        var handler = (ClientMessageHandler)Delegate.CreateDelegate(typeof(ClientMessageHandler), methodInfo);
                        if(handler!=null)
                        {
                            RegisterMessageHandler(attr.MsgID, handler);
                        }
                    }
                }
            }
        }

        public void UnregisterMessageHandler(int msgID)
        {
            if(messageHandlerDic.ContainsKey(msgID))
            {
                messageHandlerDic.Remove(msgID);
            }
        }

        public void UnregisterMessageHandler(Type handlerType)
        {
            MethodInfo[] methodInfos = handlerType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (methodInfos != null && methodInfos.Length > 0)
            {
                foreach (var methodInfo in methodInfos)
                {
                    var attr = methodInfo.GetCustomAttribute<MessageHandlerAttribute>();
                    if (attr != null)
                    {
                        UnregisterMessageHandler(attr.MsgID);
                    }
                }
            }
        }

        public void Connect(string ip,int port)
        {
            if(IsConnecting || IsConnected)
            {
                return;
            }
            network = new ClientNetwork(ip, port, this);
            network.ConnectAsync();
        }

        public void Reconnect()
        {

        }

        public void Disconnect()
        {

        }

        public void Send(int msgID,byte[] dataBytes)
        {

        }

        public void DoUpdate(float deltaTime)
        {
            if(network!=null)
            {
                return;
            }
            ClientNetworkState curState = network.State;
            if(curState != cachedState)
            {
                if (curState == ClientNetworkState.Connecting)
                {

                }
                else if (curState == ClientNetworkState.Connected)
                {

                }
                else if (curState == ClientNetworkState.Disconnecting)
                {

                }
                else if (curState == ClientNetworkState.Disconnected)
                {

                }
                else if (curState == ClientNetworkState.OtherError)
                {

                }
            }
        }

        public void Dispose()
        {
            
        }

        public void OnDataReceived(byte[] buffer, long offset, long size)
        {
        }

        
    }
}
