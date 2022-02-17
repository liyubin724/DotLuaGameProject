using System;

namespace DotEngine.Log
{
    public class WatchLogAppender : LogAppender
    {
        public const string NAME = "WatchLogAppender";

        public event Action<string, LogLevel, string> HandLogEvent;

        public WatchLogAppender(ILogFormatter formatter = null):base(NAME,formatter)
        {
        }

        protected override void OutputMessage(string tag, LogLevel level, string formattedMessage)
        {
            HandLogEvent?.Invoke(tag, level, formattedMessage);
        }
    }
}
