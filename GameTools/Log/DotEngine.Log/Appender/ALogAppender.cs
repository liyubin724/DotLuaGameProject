namespace DotEngine.Log
{
    public abstract class ALogAppender : ILogAppender
    {
        public string Name { get; private set; }

        public LogLevel MinLogLevel { get; set; } = LogLevel.On;
        public ILogFormatter Formatter { get; set; } = new DefaultLogFormatter();

        public ALogAppender(string name)
        { }

        public void OnLogReceived(LogLevel level, string tag, string message, string stackTrace)
        {
            if (level > MinLogLevel)
            {
                string logMessage = Formatter.FormatMessage(level, tag, message, stackTrace);
                OutputLogMessage(level, logMessage);
            }
        }

        protected abstract void OutputLogMessage(LogLevel level, string message);

        public virtual void DoStart()
        {
        }

        public virtual void DoEnd()
        {
        }
    }
}
