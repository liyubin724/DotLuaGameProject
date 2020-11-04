namespace DotEngine.Log
{
    public class CombineLogger : Logger
    {
        private FileLogger m_FileLogger = null;
        private UnityLogger m_UnityLogger = null;

        public CombineLogger()
        {
            m_FileLogger = new FileLogger();
            m_UnityLogger = new UnityLogger();
        }

        public override void Close()
        {
            m_FileLogger?.Close();
            m_UnityLogger?.Close();
        }

        public override void LogDebug(string tag, string message)
        {
            m_FileLogger?.LogDebug(tag,message);
            m_UnityLogger?.LogDebug(tag, message);
        }

        public override void LogError(string tag, string message)
        {
            m_FileLogger?.LogError(tag, message);
            m_UnityLogger?.LogError(tag, message);
        }

        public override void LogFatal(string tag, string message)
        {
            m_FileLogger?.LogFatal(tag, message);
            m_UnityLogger?.LogFatal(tag, message);
        }

        public override void LogInfo(string tag, string message)
        {
            m_FileLogger?.LogInfo(tag, message);
            m_UnityLogger?.LogInfo(tag, message);
        }

        public override void LogWarning(string tag, string message)
        {
            m_FileLogger?.LogWarning(tag, message);
            m_UnityLogger?.LogWarning(tag, message);
        }
    }
}
