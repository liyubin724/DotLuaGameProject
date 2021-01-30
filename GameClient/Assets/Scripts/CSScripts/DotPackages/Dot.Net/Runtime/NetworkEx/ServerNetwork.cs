using DotEngine.Network;
using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;

namespace DotEngine.NetworkEx
{
    public class ServerLogMessage
    {
        public Socket Client { get; set; }
        public int ID { get; set; }
        public byte[] Message { get; set; }
    }

    public class ServerNetwork : IUpdate
    {
        public string Name { get; private set; }
        private int m_Port = 0;
        private TcpServerSocket m_serverSocket;

        private readonly object m_MessageLocker = new object();
        private GenericObjectPool<ServerLogMessage> m_MessagePool = null;
        private List<ServerLogMessage> m_ReceivedMessages = new List<ServerLogMessage>();

        private Dictionary<int, Action<ServerLogMessage>> m_MessageHandlerDic = new Dictionary<int, Action<ServerLogMessage>>();

        public ServerNetwork(string name)
        {
            Name = name;

            m_MessagePool = new GenericObjectPool<ServerLogMessage>(
                () => new ServerLogMessage() { Client = null, Message = null },
                null,
                (message) => { message.Client = null; message.Message = null; });
        }

        public void Listen(int port)
        {
            if(m_Port!=port && m_serverSocket!=null)
            {
                DebugLog.Warning("");
                Disconnect();
            }

            m_Port = port;

            m_serverSocket = new TcpServerSocket();
            
            m_serverSocket.OnClientConnect += OnClientConnected;
            m_serverSocket.OnClientDisconnect += OnClientDisconnected;

            m_serverSocket.OnReceive += OnReceived;
            m_serverSocket.OnDisconnect += OnDisconnected;

            m_serverSocket.Listen(m_Port);

            UpdateRunner.AddUpdate(this);
        }

        public void RegistAllMessageHandler(object instance)
        {
            if (instance == null)
            {
                return;
            }
            var methods = instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public|BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<ServerNetworkMessageHandlerAttribute>();
                if (attr != null)
                {
                    Action<ServerLogMessage> messageHandler = Delegate.CreateDelegate(typeof(Action<ServerLogMessage>), instance, method) as Action<ServerLogMessage>;
                    if (messageHandler != null)
                    {
                        RegistMessageHandler(attr.ID, messageHandler);
                    }
                    else
                    {
                        DebugLog.Warning("");
                    }
                }
            }
        }

        public void UnregistAllMessageHandler(object instance)
        {
            if (instance == null)
            {
                return;
            }
            var methods = instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<ServerNetworkMessageHandlerAttribute>();
                if (attr != null)
                {
                    UnregistAllMessageHandler(attr.ID);
                }
            }
        }

        public void RegistMessageHandler(int id,Action<ServerLogMessage> handler)
        {
            if(!m_MessageHandlerDic.ContainsKey(id))
            {
                m_MessageHandlerDic.Add(id, handler);
            }else
            {
                DebugLog.Error("");
            }
        }

        public void UnregistMessageHandler(int id)
        {
            if(m_MessageHandlerDic.ContainsKey(id))
            {
                m_MessageHandlerDic.Remove(id);
            }else
            {
                DebugLog.Error("");
            }
        }

        public void Disconnect()
        {
            if(m_serverSocket!=null)
            {
                UpdateRunner.RemoveUpdate(this);

                m_serverSocket.OnClientConnect -= OnClientConnected;
                m_serverSocket.OnClientDisconnect -= OnClientDisconnected;

                m_serverSocket.OnReceive -= OnReceived;
                m_serverSocket.OnDisconnect -= OnDisconnected;

                if(m_serverSocket.IsConnected)
                {
                    m_serverSocket.Disconnect();
                }

                m_serverSocket = null;
            }
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            lock (m_MessageLocker)
            {
                foreach (var message in m_ReceivedMessages)
                {
                    if (message.Client != null && message.Client.Connected)
                    {
                        if(m_MessageHandlerDic.TryGetValue(message.ID,out var callback))
                        {
                            callback(message);
                        }else
                        {

                        }
                    }

                    m_MessagePool.Release(message);
                }

                m_ReceivedMessages.Clear();
            }
        }

        public void SendMessage(int id,byte[] messageBytes)
        {
            if(IsReadyForSend())
            {
                byte[] idBytes = BitConverter.GetBytes(id);

                if(messageBytes!=null && messageBytes.Length>0)
                {
                    byte[] dataBytes = new byte[sizeof(int) + messageBytes.Length];
                    Array.Copy(idBytes, 0, dataBytes, 0, idBytes.Length);
                    Array.Copy(messageBytes, 0, dataBytes, idBytes.Length, messageBytes.Length);
                    m_serverSocket.Send(dataBytes);
                }
                else
                {
                    m_serverSocket.Send(idBytes);
                }
            }
        }

        public void SendMessage(Socket clientSocket,int id,byte[] messageBytes)
        {
            if (IsReadyForSend())
            {
                byte[] idBytes = BitConverter.GetBytes(id);

                if (messageBytes != null && messageBytes.Length > 0)
                {
                    byte[] dataBytes = new byte[sizeof(int) + messageBytes.Length];
                    Array.Copy(idBytes, 0, dataBytes, 0, idBytes.Length);
                    Array.Copy(messageBytes, 0, dataBytes, idBytes.Length, messageBytes.Length);
                    m_serverSocket.SendWith(clientSocket, dataBytes);
                }
                else
                {
                    m_serverSocket.SendWith(clientSocket,idBytes);
                }
            }
        }
     
        bool IsReadyForSend()
        {
            return m_serverSocket != null && m_serverSocket.IsConnected && m_serverSocket.ConnectedClients > 0;
        }

        private void OnClientConnected(object sender, TcpSocketEventArgs e)
        {

        }

        private void OnClientDisconnected(object sender, TcpSocketEventArgs e)
        {

        }

        private void OnReceived(object sender, ReceiveEventArgs e)
        {
            lock (m_MessageLocker)
            {
                ServerLogMessage message = m_MessagePool.Get();

                int id = BitConverter.ToInt32(e.bytes, 0);
                byte[] contentBytes = new byte[0];
                if (e.bytes.Length > sizeof(int))
                {
                    contentBytes = new byte[e.bytes.Length - sizeof(int)];
                    Array.Copy(e.bytes, sizeof(int), contentBytes, 0,contentBytes.Length);
                }
                message.ID = id;
                message.Client = e.client;
                message.Message = contentBytes;

                m_ReceivedMessages.Add(message);
            }
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            m_serverSocket = null;
        }
    }
}
