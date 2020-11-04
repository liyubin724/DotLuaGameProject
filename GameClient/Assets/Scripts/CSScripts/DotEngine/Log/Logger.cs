namespace DotEngine.Log
{
    public abstract class Logger : ILogger
    {
        public void Log(LogLevelType levelType, string tag, string message)
        {
            if (levelType == LogLevelType.Debug)
            {
                LogDebug(tag, message);
            }
            else if (levelType == LogLevelType.Info)
            {
                LogInfo(tag, message);
            }
            else if (levelType == LogLevelType.Warning)
            {
                LogWarning(tag, message);
            }
            else if (levelType == LogLevelType.Error)
            {
                LogError(tag, message);
            }
            else if (levelType == LogLevelType.Fatal)
            {
                LogFatal(tag, message);
            }
        }

        public abstract void Close();
        public abstract void LogDebug(string tag, string message);
        public abstract void LogError(string tag, string message);
        public abstract void LogFatal(string tag, string message);
        public abstract void LogInfo(string tag, string message);
        public abstract void LogWarning(string tag, string message);
    }
}
