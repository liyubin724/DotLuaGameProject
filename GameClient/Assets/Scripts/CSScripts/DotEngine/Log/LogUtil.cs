namespace DotEngine.Log
{
    public static class LogUtil
    {
        public static LogLevelType LimitLevel { get; set; } = LogLevelType.Debug;

        private static bool isEnable = true;
        public static bool IsEnable
        {
            get
            {
                return logger != null && isEnable;
            }
            set
            {
                isEnable = value;
            }
        }

        private static ILogger logger = null;
        public static void SetLogger(ILogger logger)
        {
            LogUtil.logger = logger;
        }

        public static void DisposeLogger()
        {
            logger?.Close();
            logger = null;
        }
        
        public static void LogDebug(string tag,string message)
        {
            if(IsEnable && LimitLevel <= LogLevelType.Debug)
            {
                logger.LogDebug(tag, message);
            }
        }

        public static void LogInfo(string tag,string message)
        {
            if (IsEnable && LimitLevel <= LogLevelType.Info)
            {
                logger.LogInfo(tag, message);
            }
        }

        public static void LogWarning(string tag,string message)
        {
            if(IsEnable && LimitLevel <= LogLevelType.Warning)
            {
                logger.LogWarning(tag, message);
            }
        }

        public static void LogError(string tag,string message)
        {
            if(IsEnable && LimitLevel <= LogLevelType.Error)
            {
                logger.LogError(tag, message);
            }
        }

        public static void LogFatal(string tag,string message)
        {
            if(IsEnable && LimitLevel<=LogLevelType.Fatal)
            {
                logger.LogFatal(tag, message);
            }
        }
    }
}
