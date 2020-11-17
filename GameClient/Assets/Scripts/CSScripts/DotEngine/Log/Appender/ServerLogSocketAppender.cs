using DotEngine.Log.Formatter;
using DotEngine.Network;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace DotEngine.Log.Appender
{
    public class LogSocketUtil
    {
        public const int C2S_GET_LOG_LEVEL_REQUEST = 1;
        public const int C2S_SET_GLOBAL_LOG_LEVEL_REQUEST = 2;
        public const int C2S_SET_LOGGER_LOG_LEVEL_REQUEST = 3;

        public const int S2C_GET_LOG_LEVEL_RESPONSE = 101;
        public const int S2C_SET_GLOBAL_LOG_LEVEL_RESPONSE = 102;
        public const int S2C_SET_LOGGER_LOG_LEVEL_RESPONSE = 103;
        public const int S2C_RECEIVE_LOG_REQUEST = 104;
    }

    public class ServerLogMessage
    {
        public Socket Client { get; set; }
        public byte[] Message { get; set; }
    }

    public class ServerLogSocketAppender : ALogAppender,IUpdate
    {
        public static readonly string NAME = "SocketServerLog";
        public static readonly int PORT = 8899;

        private TcpServerSocket m_serverSocket;
        private bool m_IsDisposed = false;

        private readonly object m_MessageLocker = new object();
        private List<ServerLogMessage> m_ReceivedMessages = new List<ServerLogMessage>();

        protected ServerLogSocketAppender(ILogFormatter formatter) : base(NAME, formatter)
        {
            m_serverSocket = new TcpServerSocket();

            m_serverSocket.OnClientConnect += OnClientConnected;
            m_serverSocket.OnClientDisconnect += OnClientDisconnected;

            m_serverSocket.OnReceive += OnReceived;
            m_serverSocket.OnDisconnect += OnDisconnected;

            m_serverSocket.Listen(PORT);

            UpdateBehaviour.Updater.AddUpdate(this);
        }

        public ServerLogSocketAppender():this(new JsonLogFormatter())
        {
        }

        ~ServerLogSocketAppender()
        {
            Dispose(false);
        }

        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {
            lock(m_MessageLocker)
            {
                foreach(var message in m_ReceivedMessages)
                {
                    if(message.Client!=null && message.Client.Connected)
                    {
                        int id = BitConverter.ToInt32(message.Message, 0);
                        string jsonData = string.Empty;
                        if(message.Message.Length>sizeof(int))
                        {
                            jsonData = Encoding.UTF8.GetString(message.Message, sizeof(int), message.Message.Length - sizeof(int));
                        }

                        OnMessageHandle(message.Client, id, jsonData);
                    }
                }
                m_ReceivedMessages.Clear();
            }
        }

        private void OnMessageHandle(Socket clientSocket,int id,string message)
        {
            if(id == LogSocketUtil.C2S_GET_LOG_LEVEL_REQUEST)
            {
                JObject jObj = new JObject();

                jObj.Add("global_log_level", (int)LogUtil.GlobalLogLevel);

                JArray jArr = new JArray();
                jObj.Add("logger_log_level", jArr);
                foreach (var kvp in LogUtil.Loggers)
                {
                    JObject loggerJObj = new JObject();
                    loggerJObj.Add("name", kvp.Value.Name);
                    loggerJObj.Add("min_log_level", (int)kvp.Value.MinLogLevel);
                    loggerJObj.Add("stacktrace_log_level", (int)kvp.Value.StackTraceLogLevel);

                    jArr.Add(loggerJObj);
                }
                jObj.Add("loggers", jArr);

                SendMessage(clientSocket, LogSocketUtil.S2C_GET_LOG_LEVEL_RESPONSE, jObj.ToString());
            }
            else if(id == LogSocketUtil.C2S_SET_GLOBAL_LOG_LEVEL_REQUEST)
            {
                JObject messJObj = JObject.Parse(message);
                LogLevel globalLogLevel = (LogLevel)messJObj["global_log_level"].Value<int>();

                LogUtil.GlobalLogLevel = globalLogLevel;

                SendMessage(clientSocket, LogSocketUtil.S2C_SET_GLOBAL_LOG_LEVEL_RESPONSE, "{result_code:0}");
            }
            else if(id == LogSocketUtil.C2S_SET_LOGGER_LOG_LEVEL_REQUEST)
            {
                JObject messJObj = JObject.Parse(message);
                string name = messJObj["name"].Value<string>();
                LogLevel minLogLevel = (LogLevel)messJObj["min_log_level"].Value<int>();
                LogLevel stacktraceLogLevel = (LogLevel)messJObj["stacktrace_log_level"].Value<int>();

                foreach (var kvp in LogUtil.Loggers)
                {
                    if(kvp.Value.Name == name)
                    {
                        kvp.Value.MinLogLevel = minLogLevel;
                        kvp.Value.StackTraceLogLevel = stacktraceLogLevel;
                        break;
                    }
                }

                SendMessage(clientSocket, LogSocketUtil.S2C_SET_LOGGER_LOG_LEVEL_RESPONSE, "{result_code:0}");
            }
        }

        bool IsReadyForSend()
        {
            return m_serverSocket != null && m_serverSocket.IsConnected && m_serverSocket.ConnectedClients > 0;
        }

        public void Disconnect()
        {
            m_serverSocket.Disconnect();
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
                ServerLogMessage message = new ServerLogMessage()
                {
                    Client = e.client,
                    Message = e.bytes,
                };
                m_ReceivedMessages.Add(message);
            }
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            m_serverSocket = null;
        }

        protected override void DoLogMessage(LogLevel level, string message)
        {
            SendMessage(LogSocketUtil.S2C_RECEIVE_LOG_REQUEST, message);
        }

        private void SendMessage(int id, string message)
        {
            if (IsReadyForSend())
            {
                byte[] idBytes = BitConverter.GetBytes(id);
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                byte[] bytes = new byte[sizeof(int) + messageBytes.Length];
                Array.Copy(idBytes, 0, bytes, 0, idBytes.Length);
                Array.Copy(messageBytes, 0, bytes, idBytes.Length, messageBytes.Length);

                m_serverSocket.Send(bytes);
            }
        }

        private void SendMessage(Socket clientSocket,int id,string message)
        {
            if(IsReadyForSend() && clientSocket!=null && clientSocket.Connected)
            {
                byte[] idBytes = BitConverter.GetBytes(id);
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                
                byte[] bytes = new byte[sizeof(int) + messageBytes.Length];
                Array.Copy(idBytes, 0, bytes, 0, idBytes.Length);
                Array.Copy(messageBytes, 0, bytes, idBytes.Length, messageBytes.Length);

                m_serverSocket.SendWith(clientSocket, bytes);
            }
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (m_IsDisposed) return;

            if (disposing)
            {
                UpdateBehaviour.Updater.RemoveUpdate(this);
            }
            if(m_serverSocket!=null)
            {
                m_serverSocket.OnClientConnect -= OnClientConnected;
                m_serverSocket.OnClientDisconnect -= OnClientDisconnected;

                m_serverSocket.OnReceive -= OnReceived;
                m_serverSocket.OnDisconnect -= OnDisconnected;

                m_serverSocket.Disconnect();
                m_serverSocket = null;
            }

            m_IsDisposed = true;
        }
    }
}
