using DotEngine.Generic;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace DotEngine.Net
{
    public delegate void ServerMessageHandler(ClientData client, int messageID, byte[] dataBytes);
    
    public enum ServerNetworkState
    {
        Startup = 0,
        Shuntdown,
        ClientConnected,
        ClientDisconnected,
    }

    public class ClientData
    {
        public int Index { get; set; }
        public INetworkSession Session { get; set; }
    }

    public class ServerNetworkSocket : ANetworkSocket
    {

















        private Socket socket = null;
        private Dictionary<INetworkSession, ClientData> clientDic = new Dictionary<INetworkSession, ClientData>();
        
        private Dictionary<int, ServerMessageHandler> messageHandlerDic = new Dictionary<int, ServerMessageHandler>();
        public ServerMessageHandler FiniallyMessageHandler { get; set; } = null;

        public int ConnectedClients => clientDic.Count;
        public UniqueIntID sessionIndexCreator = new UniqueIntID(1);

        public void Startup(int port,int maxCount)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Bind(new IPEndPoint(IPAddress.Any, port));
                socket.Listen(maxCount);

                DoAccept();
            }catch(Exception e)
            {

            }
        }

        public void Shuntdown()
        {

        }

        private void DoAccept()
        {
            socket.BeginAccept(ProcessAccept, socket);
        }

        private void ProcessAccept(IAsyncResult ar)
        {
            var server = (Socket)ar.AsyncState;
            AcceptedClientConnection(server.EndAccept(ar));
            DoAccept();
        }

        private void AcceptedClientConnection(Socket client)
        {
            ServerNetworkSession session = new ServerNetworkSession();
            session.BindSocket(client, this);
            ClientData clientData = new ClientData()
            {
                Index = sessionIndexCreator.GetNextID(),
                Session = session,
            };

            clientDic.Add(session, clientData);

            session.DoReceive();
        }

        public void Send(int messageID)
        {
            Send(messageID, MessageCompressType.None, MessageCryptoType.None, null);
        }

        public void Send(int messageID,MessageCompressType compressType,MessageCryptoType cryptoType,byte[] dataBytes)
        {
            foreach(var client in clientDic.Keys)
            {
                client.SendMessage(messageID, compressType, cryptoType, dataBytes);
            }
        }

        public void SendExcept(INetworkSession clientSession, int messageID)
        {
            SendExcept(clientSession, messageID, MessageCompressType.None, MessageCryptoType.None, null);
        }

        public void SendExcept(INetworkSession clientSession, int messageID, MessageCompressType compressType, MessageCryptoType cryptoType, byte[] dataBytes)
        {
            foreach (var client in clientDic.Keys)
            {
                if(client != clientSession)
                {
                    client.SendMessage(messageID, compressType, cryptoType, dataBytes);
                }
            }
        }

        public void RegistMessageHandler(int messageID, ServerMessageHandler handler)
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
                var attr = method.GetCustomAttribute<ServerMessageHandlerAttribute>();
                if (attr != null && attr.ID > 0)
                {
                    if (Delegate.CreateDelegate(typeof(ServerMessageHandler), null, method) is ServerMessageHandler messageHandler)
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
                var attr = method.GetCustomAttribute<ServerMessageHandlerAttribute>();
                if (attr != null && attr.ID > 0)
                {
                    UnregistMessageHandler(attr.ID);
                }
            }
        }

        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {

        }

        public void OnMessageHandler(INetworkSession session, int messageID, byte[] dataBytes)
        {
            if(clientDic.TryGetValue(session,out var clientData))
            {
                if (messageHandlerDic.TryGetValue(messageID, out var handler))
                {
                    handler(clientData, messageID, dataBytes);
                }
                else if (FiniallyMessageHandler != null)
                {
                    FiniallyMessageHandler(clientData, messageID, dataBytes);
                }
            }
        }

        public void OnSessionError(NetworkSessionError error, INetworkSession session, object userdata)
        {

        }

        public void OnSessionOperation(NetworkSessionOperation operation, INetworkSession session, object userdata)
        {
            
        }
    }

}
