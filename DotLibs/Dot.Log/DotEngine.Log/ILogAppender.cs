using System;

namespace DotEngine.Log
{
    public interface ILogAppender
    {
        string Name { get; }
        LogLevel ValidLevel { get; set; }

        void DoStart();
        void OnLogReceived(LogLevel level, DateTime dateTime, string tag, string message, string stacktrace);
        void DoEnd();
    }
}
