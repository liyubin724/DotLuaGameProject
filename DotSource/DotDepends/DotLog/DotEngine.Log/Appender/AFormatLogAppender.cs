using System;

namespace DotEngine.Log
{
    public abstract class AFormatLogAppender : ALogAppender
    {
        public ILogFormatter Formatter { get; set; }
        protected AFormatLogAppender(string name,ILogFormatter formatter = null) : base(name)
        {
            Formatter = formatter ?? new DefaultLogFormatter();
        }

        protected override void OutputMessage(DateTime time, string tag, LogLevel level, string message, string stacktrace)
        {
            string formattedMessage = Formatter.FormatMessage(time, tag, level, message, stacktrace);
            OutputFormattedMessage(formattedMessage);
        }

        protected abstract void OutputFormattedMessage(string message);
    }
}
