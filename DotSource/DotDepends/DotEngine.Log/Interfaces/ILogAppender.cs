using System;

namespace DotEngine.Log
{
    public interface ILogAppender
    {
        string Name { get; }
        LogLevel Level { get; set; }
        ILogFormatter Formatter { get; set; }

        void DoInitialize();
        void DoReceive(string tag, LogLevel level, string message);
        void DoDestroy();
    }
}
