namespace DotEngine.Log
{
    public abstract class ALogAppender : ILogAppender
    {
        public string Name { get; private set; }
        public ILogFormatter Formatter { get; private set; }

        public ALogAppender(string name) : this(name, new DefaultLogFormatter())
        { }

        protected ALogAppender(string name,ILogFormatter formatter)
        {
            Name = name;
            Formatter = formatter;
        }

        public void OnLogReceived(LogLevel level, string tag, string message,string stackTrace)
        {
            string logMessage = Formatter.FormatMessage(level, tag, message, stackTrace);
            OutputLogMessage(level, logMessage);
        }

        protected abstract void OutputLogMessage(LogLevel level,string message);

        public virtual void OnAppended()
        {
        }

        public virtual void OnRemoved()
        {
        }
    }
}
