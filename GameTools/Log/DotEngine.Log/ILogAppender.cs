using System;

namespace DotEngine.Log
{
    public interface ILogAppender
    {
        string Name { get; }
        LogLevel ValidLevel { get; }

        void DoStart();
        void OnLogReceived(LogLevel level, string tag, DateTime time, string message, string stacktrace);
        void DoEnd();
    }
}
