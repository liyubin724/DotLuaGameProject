using System;

namespace DotEngine.Log
{
    public abstract class ALogAppender : ILogAppender
    {
        public string Name { get; private set; }
        public LogLevel AliveLevel { get; set; } = LogLevelConst.All;

        public ALogAppender(string name)
        {
            Name = name;
        }

        public virtual void DoInitialize()
        {
        }

        public void DoReceive(DateTime time, string tag, LogLevel level, string message, string stacktrace)
        {
            if ((AliveLevel & level) > 0)
            {
                OutputMessage(time, tag, level, message, stacktrace);
            }
        }

        protected abstract void OutputMessage(DateTime time, string tag, LogLevel level, string message, string stacktrace);

        public virtual void DoDestroy()
        {
        }
    }
}
