using System;

namespace DotEngine.Log
{
    public interface ILogAppender
    {
        string Name { get; }
        LogLevel AliveLevel { get; set; }

        void DoInitialize();
        void DoReceive(DateTime time, string tag, LogLevel level, string message, string stacktree);
        void DoDestroy();
    }
}
