namespace DotEngine.Log
{
    public delegate void LogHandler(LogLevel level, string tag, string message);

    public class Logger
    {
        public string Tag { get; private set; }
        public LogLevel ValidLevel { get; set; } = LogLevelConst.All;
        public LogHandler Handler;

        internal Logger(string tag)
        {
            Tag = tag;
        }

        public void Trace(string message)
        {
            if ((ValidLevel & LogLevel.Trace) > 0)
            {
                Handler?.Invoke(LogLevel.Trace, Tag, message);
            }
        }

        public void Debug(string message)
        {
            if ((ValidLevel & LogLevel.Debug) > 0)
            {
                Handler?.Invoke(LogLevel.Debug, Tag, message);
            }
        }

        public void Info(string message)
        {
            if ((ValidLevel & LogLevel.Info) > 0)
            {
                Handler?.Invoke(LogLevel.Info, Tag, message);
            }
        }

        public void Warning(string message)
        {
            if ((ValidLevel & LogLevel.Warning) > 0)
            {
                Handler?.Invoke(LogLevel.Warning, Tag, message);
            }
        }

        public void Error(string message)
        {
            if ((ValidLevel & LogLevel.Error) > 0)
            {
                Handler?.Invoke(LogLevel.Error, Tag, message);
            }
        }

        public void Fatal(string message)
        {
            if ((ValidLevel & LogLevel.Fatal) > 0)
            {
                Handler?.Invoke(LogLevel.Fatal, Tag, message);
            }
        }
    }
}
