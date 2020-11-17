using DotEngine.Log;
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
        public byte[] Message { get; set; }
    }

    public class ServerNetwork : IUpdate
    {
        private string m_Name;
        private int m_Port = 0;
        private TcpServerSocket m_serverSocket;

        private Logger m_Logger = null;

        private readonly object m_MessageLocker = new object();
        private GenericObjectPool<ServerLogMessage> m_MessagePool = null;
        private List<ServerLogMessage> m_ReceivedMessages = new List<ServerLogMessage>();

        private Dictionary<int, Action<Socket, byte[]>> m_MessageHandlerDic = new Dictionary<int, Action<Socket, byte[]>>();

        public ServerNetwork(string name)
        {
            m_Name = name;

            m_Logger = LogUtil.GetLogger(name, LogLevel.Error, LogLevel.Error);

            m_MessagePool = new GenericObjectPool<ServerLogMessage>(
                () => new ServerLogMessage() { Client = null, Message = null },
                null,
                (message) => { message.Client = null; message.Message = null; });
        }

        public void Listen(int port)
        {
            if(m_Port!=port && m_serverSocket!=null)
            {
                m_Logger.Warning("");
                Disconnect();
            }

            m_Port = port;

            m_serverSocket = new TcpServerSocket();
            
            m_serverSocket.OnClientConnect += OnClientConnected;
            m_serverSocket.OnClientDisconnect += OnClientDisconnected;

            m_serverSocket.OnReceive += OnReceived;
            m_serverSocket.OnDisconnect += OnDisconnected;

            m_serverSocket.Listen(m_Port);

            UpdateBehaviour.Updater.AddUpdate(this);
        }

        public void RegistAllMessageHandler(object instance)
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
                    Action<Socket, byte[]> messageHandler = Delegate.CreateDelegate(typeof(Action<Socket, byte[]>), instance, method) as Action<Socket, byte[]>;
                    if (messageHandler != null)
                    {
                        RegistMessageHandler(attr.ID, messageHandler);
                    }
                    else
                    {
                        m_Logger.Warning("");
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

        public void RegistMessageHandler(int id,Action<Socket,byte[]> handler)
        {
            if(!m_MessageHandlerDic.ContainsKey(id))
            {
                m_MessageHandlerDic.Add(id, handler);
            }else
            {
                m_Logger.Error("");
            }
        }

        public void UnregistMessageHandler(int id)
        {
            if(m_MessageHandlerDic.ContainsKey(id))
            {
                m_MessageHandlerDic.Remove(id);
            }else
            {
                m_Logger.Error("");
            }
        }

        public void Disconnect()
        {
            if(m_serverSocket!=null)
            {
                UpdateBehaviour.Updater.RemoveUpdate(this);

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
                        int id = BitConverter.ToInt32(message.Message, 0);
                        if(m_MessageHandlerDic.TryGetValue(id,out var callback))
                        {
                            byte[] contentBytes = new byte[0];
                            if (message.Message.Length > sizeof(int))
                            {
                                contentBytes = new byte[message.Message.Length - sizeof(int)];
                            }
                            callback(message.Client, contentBytes);
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
                message.Client = e.client;
                message.Message = e.bytes;
                m_ReceivedMessages.Add(message);
            }
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            m_serverSocket = null;
        }
    }
}
