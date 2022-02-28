using System;

namespace DotEngine.Log
{
    public class WatchLogAppender : LogAppender
    {
        public const string NAME = "WatchLogAppender";

        private Action<string, LogLevel, string, string> handlerAction;

        public WatchLogAppender(
            Action<string, LogLevel, string, string> handler,
            string name = NAME,
            ILogFormatter formatter = null) : base(NAME, formatter)
        {
            handlerAction = handler;
        }

        protected override void OutputMessage(string tag, LogLevel level, string formattedMessage, string stacktrace)
        {
            handlerAction?.Invoke(tag, level, formattedMessage, stacktrace);
        }
    }
}
