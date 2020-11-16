using DotEngine;
using DotEngine.Log;
using DotEngine.Log.Appender;
using DotEngine.Network;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DotEditor.Log
{
    public enum LogClientStatus
    {
        None,
        Connecting,
        Connected,
        Disconnecting,
        Disconnected,
    }

    public class LogClientSocket : IUpdate
    {
        private TcpClientSocket clientSocket = null;
        
        private object logDataLocker = new object();
        private List<LogData> logDataList = new List<LogData>();
        public LogData[] LogDatas
        {
            get
            {
                lock (logDataLocker)
                {
                    LogData[] result = logDataList.ToArray();
                    logDataList.Clear();

                    return result;
                }
            }
        }

        private object logStatusLocker = new object();
        private LogClientStatus status = LogClientStatus.None;
        public LogClientStatus Status
        {
            get
            {
                lock(logStatusLocker)
                {
                    return status;
                }
            }

            private set
            {
                lock(logStatusLocker)
                {
                    status = value;
                }
            }
        }

        public LogClientSocket()
        {
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {

        }

        public bool Connect(string ipString,int port)
        {
            if(clientSocket!=null && (status == LogClientStatus.Connecting||status == LogClientStatus.Connected))
            {
                return false;
            }
            clientSocket = new TcpClientSocket();
            clientSocket.OnConnect += OnConnected;
            clientSocket.OnReceive += OnReceived;
            clientSocket.OnDisconnect += OnDisconnected;
            clientSocket.Connect(IPAddress.Parse(ipString), port);

            Status = LogClientStatus.Connecting;

            return true;
        }

        public bool Disconnect()
        {
            if(clientSocket!=null && (Status == LogClientStatus.Connecting || Status == LogClientStatus.Connected))
            {
                Status = LogClientStatus.Disconnecting;
                clientSocket.Disconnect();
                return true;
            }

            return false;
        }
        
        private void OnConnected(object sender, EventArgs eventArgs)
        {
            Status = LogClientStatus.Connected;
        }

        private void OnReceived(object sender, ReceiveEventArgs eventArgs)
        {
            int id = BitConverter.ToInt32(eventArgs.bytes, 0);
            if (id == LogSocketUtil.S2C_RECEIVE_LOG_REQUEST)
            {
                lock (logDataLocker)
                {
                    string jsonStr = Encoding.UTF8.GetString(eventArgs.bytes,sizeof(int),eventArgs.bytes.Length-sizeof(int));
                    JObject jsonObj = JObject.Parse(jsonStr);

                    LogData logData = new LogData();
                    logData.Level = (LogLevel)jsonObj["level"].Value<int>();
                    logData.Time = new DateTime(jsonObj["time"].Value<long>());
                    logData.Tag = jsonObj["tag"].Value<string>();
                    logData.Message = jsonObj["message"].Value<string>();
                    logData.StackTrace = jsonObj["stacktrace"].Value<string>();

                    logDataList.Add(logData);
                }
            }else if(id == LogSocketUtil.S2C_GET_LOG_LEVEL_RESPONSE)
            {
                LogViewerSetting setting = LogViewer.Viewer.viewerSetting;
                string jsonStr = Encoding.UTF8.GetString(eventArgs.bytes, sizeof(int), eventArgs.bytes.Length - sizeof(int));
                JObject messJObj = JObject.Parse(jsonStr);

                setting.GlobalLogLevel = (LogLevel)messJObj["global_log_level"].Value<int>();

                setting.LoggerLogLevelDic.Clear();

                JArray loggerSettings = (JArray)messJObj["loggers"];
                for(int i =0;i<loggerSettings.Count;++i)
                {
                    JObject loggerJObj = loggerSettings.GetItem(i).Value<JObject>();
                    LogViewerSetting.LoggerSetting loggerSetting = new LogViewerSetting.LoggerSetting();
                    loggerSetting.Name = loggerJObj["name"].Value<string>();
                    loggerSetting.MinLogLevel = (LogLevel)loggerJObj["min_log_level"].Value<int>();
                    loggerSetting.StackTraceLogLevel = (LogLevel)loggerJObj["stacktrace_log_level"].Value<int>();

                    setting.LoggerLogLevelDic.Add(loggerSetting.Name, loggerSetting);
                }
            }
        }

        private void OnDisconnected(object sender, EventArgs eventArgs)
        {
            Status = LogClientStatus.Disconnected;

            clientSocket.OnConnect -= OnConnected;
            clientSocket.OnReceive -= OnReceived;
            clientSocket.OnDisconnect -= OnDisconnected;
            clientSocket = null;
        }

        public void SendMessage(int id,string message)
        {
            if(clientSocket!=null && clientSocket.IsConnected)
            {
                byte[] idBytes = BitConverter.GetBytes(id);
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                byte[] bytes = new byte[sizeof(int) + messageBytes.Length];
                Array.Copy(idBytes, 0, bytes, 0, idBytes.Length);
                Array.Copy(messageBytes, 0, bytes, idBytes.Length, messageBytes.Length);

                clientSocket.Send(bytes);
            }
        }
        
    }
}
