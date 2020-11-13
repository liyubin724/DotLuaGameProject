using DotEngine.Log;
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

    public class LogClientSocket
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
            lock(logDataLocker)
            {
                string jsonStr = Encoding.UTF8.GetString(eventArgs.bytes);
                JObject jsonObj = JObject.Parse(jsonStr);

                LogData logData = new LogData();
                logData.Level = (LogLevel)jsonObj["level"].Value<int>();
                logData.Time = new DateTime(jsonObj["time"].Value<long>());
                logData.Tag = jsonObj["tag"].Value<string>();
                logData.Message = jsonObj["message"].Value<string>();
                logData.StackTrace = jsonObj["stacktrace"].Value<string>();

                logDataList.Add(logData);
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
    }
}
