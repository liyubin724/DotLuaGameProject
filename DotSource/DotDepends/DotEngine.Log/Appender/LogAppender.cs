﻿using System;

namespace DotEngine.Log
{
    public abstract class LogAppender : ILogAppender
    {
        public string Name { get; private set; }
        public LogLevel Level { get; set; } = LogLevelConst.All;
        public ILogFormatter Formatter { get; set; }

        public LogAppender(string name, ILogFormatter formatter = null)
        {
            Name = name;
            Formatter = formatter ?? new DefaultLogFormatter();
        }

        public virtual void DoInitialize()
        {
        }

        public void DoReceive(string tag, LogLevel level,string message)
        {
            if((Level & level) > 0)
            {
                string fMessage = Formatter.FormatMessage(tag, level, message);
                OutputMessage(tag, level, fMessage);
            }
        }

        protected abstract void OutputMessage(string tag, LogLevel level, string formattedMessage);

        public virtual void DoDestroy()
        {
        }
    }
}
