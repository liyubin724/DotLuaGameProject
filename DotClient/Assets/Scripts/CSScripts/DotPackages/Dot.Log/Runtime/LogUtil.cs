namespace DotEngine.Log
{
    public static class LogUtil
    {
        private static LogManager manager = null;
        public static void Init()
        {
            manager = LogManager.InitMgr();
        }

        public static void SetLogLevel(LogLevel validLogLevel,LogLevel stacktraceLogLevel = LogLevelConst.Serious)
        {
            if(manager!=null)
            {
                manager.ValidLogLevel = validLogLevel;
                manager.StacktraceLogLevel = stacktraceLogLevel;
            }
        }

        public static void Dispose()
        {
            LogManager.DisposeMgr();
            manager = null;
        }

        public static void AddAppender(ILogAppender appender)
        {
            manager?.AddAppender(appender);
        }

        public static void RemoveAppender(string tag)
        {
            manager?.RemoveAppender(tag);
        }

        public static ILogger GetLogger(string tag)
        {
            if(manager!=null)
            {
                return manager.GetLogger(tag, true);
            }
            return null;
        }

        public static void Debug(string message)
        {
            GetLogger(LogManager.DEFAULT_LOGGER_TAG).Debug(message);
        }

        public static void Debug(string tag,string message)
        {
            GetLogger(tag).Debug(message);
        }

        public static void Info(string message)
        {
            GetLogger(LogManager.DEFAULT_LOGGER_TAG)?.Info(message);
        }

        public static void Info(string tag,string message)
        {
            GetLogger(tag).Info(message);
        }

        public static void Warning(string message)
        {
            GetLogger(LogManager.DEFAULT_LOGGER_TAG).Warning(message);
        }

        public static void Warning(string tag,string message)
        {
            GetLogger(tag).Warning(message);
        }

        public static void Error(string message)
        {
            GetLogger(LogManager.DEFAULT_LOGGER_TAG).Error(message);
        }

        public static void Error(string tag,string message)
        {
            GetLogger(tag)?.Error(message);
        }
    }
}
