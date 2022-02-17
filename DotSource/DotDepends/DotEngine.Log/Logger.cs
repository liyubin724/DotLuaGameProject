namespace DotEngine.Log
{
    public class Logger : ILogger
    {
        public string Tag { get; private set; }
        public ILogHandler Handler { get; set; }

        internal Logger(string tag)
        {
            Tag = tag;
        }

        public void Trace(string message)
        {
            Handler?.OnLogReceived(Tag, LogLevel.Trace, message);
        }

        public void Debug(string message)
        {
            Handler?.OnLogReceived(Tag, LogLevel.Debug, message);
        }

        public void Error(string message)
        {
            Handler?.OnLogReceived(Tag, LogLevel.Error, message);
        }

        public void Info(string message)
        {
            Handler?.OnLogReceived(Tag, LogLevel.Info, message);
        }

        public void Warning(string message)
        {
            Handler?.OnLogReceived(Tag, LogLevel.Warning, message);
        }

        public void Fatal(string message)
        {
            Handler?.OnLogReceived(Tag, LogLevel.Fatal, message);
        }
    }
}
