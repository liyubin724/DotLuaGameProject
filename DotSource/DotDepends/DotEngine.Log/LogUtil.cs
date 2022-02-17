namespace DotEngine.Log
{
    public static class LogUtil
    {
        private static string DEFAULT_LOGGER_TAG = "Default Logger";

        public static ILogger DefaultLogger
        {
            get
            {
                return GetLogger(DEFAULT_LOGGER_TAG);
            }
        }

        public static ILogger GetLogger(string tag)
        {
            LogManager manager = LogManager.GetInstance();
            if(manager == null)
            {
                manager = LogManager.CreateMgr();
            }
            return manager.GetLogger(tag, true);
        }

        public static void Trace(string message)
        {
            GetLogger(DEFAULT_LOGGER_TAG).Trace(message);
        }

        public static void Trace(string tag,string message)
        {
            GetLogger(tag).Trace(message);
        }

        public static void Debug(string message)
        {
            GetLogger(DEFAULT_LOGGER_TAG).Debug(message);
        }

        public static void Debug(string tag,string message)
        {
            GetLogger(tag).Debug(message);
        }

        public static void Info(string message)
        {
            GetLogger(DEFAULT_LOGGER_TAG).Info(message);
        }

        public static void Info(string tag,string message)
        {
            GetLogger(tag).Info(message);
        }

        public static void Warning(string message)
        {
            GetLogger(DEFAULT_LOGGER_TAG).Warning(message);
        }

        public static void Warning(string tag,string message)
        {
            GetLogger(tag).Warning(message);
        }

        public static void Error(string message)
        {
            GetLogger(DEFAULT_LOGGER_TAG).Error(message);
        }

        public static void Error(string tag,string message)
        {
            GetLogger(tag).Error(message);
        }

        public static void Fatal(string message)
        {
            GetLogger(DEFAULT_LOGGER_TAG).Fatal(message);
        }

        public static void Fatal(string tag,string message)
        {
            GetLogger(tag).Fatal(message);
        }
    }
}
