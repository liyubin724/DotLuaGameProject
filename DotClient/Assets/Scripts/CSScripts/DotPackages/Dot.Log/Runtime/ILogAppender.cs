using System;

namespace DotEngine.Log
{
    public interface ILogAppender
    {
        string Name { get; }
        LogLevel ValidLevel { get; set; }

        void DoInitialize();
        void OnLogReceived(string tag, LogLevel level, DateTime dateTime, string message, string stacktrace);
        void DoDestroy();
    }
}
