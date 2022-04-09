using System;

namespace DotEngine.Log
{
    public delegate void WatcherHandler(DateTime time, string tag, LogLevel level, string message, string stacktrace);

    public class WatcherAppender : ALogAppender
    {
        public WatcherHandler LogHandler { get; set; }

        public WatcherAppender(string name) : base(name)
        {
        }

        protected override void OutputMessage(DateTime time, string tag, LogLevel level, string message, string stacktrace)
        {
            LogHandler?.Invoke(time, tag, level, message, stacktrace);
        }
    }
}
