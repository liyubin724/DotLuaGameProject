using System;

namespace DotEngine.Log
{
    public delegate void WatcherHandler(DateTime time, string tag, LogLevel level, string message, string stacktrace);

    public class WatcherAppender : ALogAppender
    {
        public const string NAME = "WatchLogAppender";

        private WatcherHandler logHandler;
        public WatcherAppender(WatcherHandler handler, string name = NAME) : base(name)
        {
            logHandler = handler;
        }

        protected override void OutputMessage(DateTime time, string tag, LogLevel level, string message, string stacktrace)
        {
            logHandler?.Invoke(time, tag, level, message, stacktrace);
        }
    }
}
