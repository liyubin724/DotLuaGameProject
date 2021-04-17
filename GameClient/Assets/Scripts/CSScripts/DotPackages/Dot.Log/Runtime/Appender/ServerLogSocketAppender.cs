using DotEngine.Log;
using DotEngine.NetworkEx;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotEngine.Log.Appender
{
    public class LogNetUtill
    {
        public static readonly int PORT = 8899;

        public const int C2S_GET_LOG_LEVEL_REQUEST = 1;
        public const int C2S_SET_GLOBAL_LOG_LEVEL_REQUEST = 2;
        public const int C2S_SET_LOGGER_LOG_LEVEL_REQUEST = 3;

        public const int S2C_GET_LOG_LEVEL_RESPONSE = 101;
        public const int S2C_SET_GLOBAL_LOG_LEVEL_RESPONSE = 102;
        public const int S2C_SET_LOGGER_LOG_LEVEL_RESPONSE = 103;
        public const int S2C_RECEIVE_LOG_REQUEST = 104;

    }

    [Serializable]
    public class LogNetLoggerSetting
    {
        public string Name = string.Empty;
        public LogLevel MinLogLevel = LogLevel.Off;
        public LogLevel StackTraceLogLevel = LogLevel.Error;
    }

    [Serializable]
    public class LogNetGlobalSetting
    {
        public LogLevel GlobalLogLevel = LogLevel.Off;
    }

    [Serializable]
    public class LogNetSetting
    {
        public LogNetGlobalSetting GlobalSetting { get; set; } = null;
        public List<LogNetLoggerSetting> LoggerSettings { get; set; } = null;
    }

    public class ServerLogSocketAppender : ALogAppender
    {
        public static readonly string NAME = "SocketServerLog";

        private ServerNetwork m_ServerNetwork = null;
        private bool m_IsDisposed = false;

        public ServerLogSocketAppender() : this(new JsonLogFormatter())
        {
        }

        protected ServerLogSocketAppender(ILogFormatter formatter) : base(NAME, formatter)
        {
            m_ServerNetwork = new ServerNetwork("LogServer");
            m_ServerNetwork.RegistAllMessageHandler(this);
            m_ServerNetwork.Listen(LogNetUtill.PORT);
        }

        ~ServerLogSocketAppender()
        {
            Dispose(false);
        }


        [ServerNetworkMessageHandler(LogNetUtill.C2S_GET_LOG_LEVEL_REQUEST)]
        private void OnC2SGetLogLevelRequest(ServerLogMessage message)
        {
            LogNetSetting setting = new LogNetSetting();
            setting.GlobalSetting = new LogNetGlobalSetting()
            {
                GlobalLogLevel = LogUtil.GlobalValidLevel,
            };
            setting.LoggerSettings = new List<LogNetLoggerSetting>();
            foreach (var kvp in LogUtil.loggerDic)
            {
                LogNetLoggerSetting loggerSetting = new LogNetLoggerSetting()
                {
                    Name = kvp.Value.Tag,
                    MinLogLevel = kvp.Value.ValidLevel,
                    StackTraceLogLevel = kvp.Value.StacktraceLevel,
                };
                setting.LoggerSettings.Add(loggerSetting);
            }

            string settingJsonStr = JsonConvert.SerializeObject(setting);

            m_ServerNetwork.SendMessage(message.Client, LogNetUtill.S2C_GET_LOG_LEVEL_RESPONSE,Encoding.UTF8.GetBytes(settingJsonStr));
        }

        [ServerNetworkMessageHandler(LogNetUtill.C2S_SET_GLOBAL_LOG_LEVEL_REQUEST)]
        private void OnC2SSetGlobalLogLevelRequest(ServerLogMessage message)
        {
            string jsonStr = Encoding.UTF8.GetString(message.Message);
            LogNetGlobalSetting globalSetting = JsonConvert.DeserializeObject<LogNetGlobalSetting>(jsonStr);
            if(globalSetting!=null)
            {
                LogUtil.GlobalValidLevel = globalSetting.GlobalLogLevel;
            }
            //m_ServerNetwork.SendMessage(message.Client,LogNetUtill.S2C_SET_GLOBAL_LOG_LEVEL_RESPONSE, message.Message);
        }

        [ServerNetworkMessageHandler(LogNetUtill.C2S_SET_LOGGER_LOG_LEVEL_REQUEST)]
        private void OnC2SSetLoggerLogLevelRequest(ServerLogMessage message)
        {
            string jsonStr = Encoding.UTF8.GetString(message.Message);
            LogNetLoggerSetting loggerSetting = JsonConvert.DeserializeObject<LogNetLoggerSetting>(jsonStr);

            if(loggerSetting!=null)
            {
                if(LogUtil.loggerDic.TryGetValue(loggerSetting.Name,out var logger))
                {
                    logger.ValidLevel = loggerSetting.MinLogLevel;
                    logger.StacktraceLevel = loggerSetting.StackTraceLogLevel;
                }
            }

            //m_ServerNetwork.SendMessage(LogNetUtill.S2C_SET_LOGGER_LOG_LEVEL_RESPONSE, message.Message);
        }

        protected override void OutputLogMessage(LogLevel level, string message)
        {
            m_ServerNetwork.SendMessage(LogNetUtill.S2C_RECEIVE_LOG_REQUEST, Encoding.UTF8.GetBytes(message));
        }

        private void Dispose(bool disposing)
        {
            if (m_IsDisposed) return;

            if(m_ServerNetwork!=null)
            {
                m_ServerNetwork.Disconnect();
                m_ServerNetwork = null;
            }

            m_IsDisposed = true;
        }
    }
}
