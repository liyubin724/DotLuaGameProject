using DotEngine.Log.Formatter;
using DotEngine.NetworkEx;
using Newtonsoft.Json.Linq;
using System;
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

            m_ServerNetwork.SendMessage(message.Client, LogNetUtill.S2C_GET_LOG_LEVEL_RESPONSE,Encoding.UTF8.GetBytes(jObj.ToString()));
        }

        [ServerNetworkMessageHandler(LogNetUtill.C2S_SET_GLOBAL_LOG_LEVEL_REQUEST)]
        private void OnC2SSetGlobalLogLevelRequest(ServerLogMessage message)
        {
            string jsonStr = Encoding.UTF8.GetString(message.Message);
            JObject jObj = JObject.Parse(jsonStr);
            LogLevel globalLogLevel = (LogLevel)jObj["global_log_level"].Value<int>();

            LogUtil.GlobalLogLevel = globalLogLevel;

            m_ServerNetwork.SendMessage(LogNetUtill.S2C_SET_GLOBAL_LOG_LEVEL_RESPONSE, message.Message);
        }

        [ServerNetworkMessageHandler(LogNetUtill.C2S_SET_LOGGER_LOG_LEVEL_REQUEST)]
        private void OnC2SSetLoggerLogLevelRequest(ServerLogMessage message)
        {
            string jsonStr = Encoding.UTF8.GetString(message.Message);
            JObject messJObj = JObject.Parse(jsonStr);

            string name = messJObj["name"].Value<string>();
            LogLevel minLogLevel = (LogLevel)messJObj["min_log_level"].Value<int>();
            LogLevel stacktraceLogLevel = (LogLevel)messJObj["stacktrace_log_level"].Value<int>();

            foreach (var kvp in LogUtil.Loggers)
            {
                if (kvp.Value.Name == name)
                {
                    kvp.Value.MinLogLevel = minLogLevel;
                    kvp.Value.StackTraceLogLevel = stacktraceLogLevel;
                    break;
                }
            }

            m_ServerNetwork.SendMessage(LogNetUtill.S2C_SET_LOGGER_LOG_LEVEL_RESPONSE, message.Message);
        }

        protected override void DoLogMessage(LogLevel level, string message)
        {
            m_ServerNetwork.SendMessage(LogNetUtill.S2C_RECEIVE_LOG_REQUEST, Encoding.UTF8.GetBytes(message));
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
