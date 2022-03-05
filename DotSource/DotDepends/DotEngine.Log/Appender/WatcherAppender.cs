using System;

namespace DotEngine.Log
{
    public delegate void WatcherHandler(DateTime time, string tag, LogLevel level, string message, string stacktree);

    public class WatcherAppender : ALogAppender
    {
        public const string NAME = "WatchLogAppender";

        public event WatcherHandler HandLogEvent;
        private WatcherHandler logHandler = null;

        public WatcherAppender(WatcherHandler handler, string name = NAME) : base(name)
        {
            logHandler = handler;
        }

        protected override void OutputMessage(DateTime time, string tag, LogLevel level, string message, string stacktree)
        {
            logHandler?.Invoke(time, tag, level, message, stacktree);
        }
    }
}
