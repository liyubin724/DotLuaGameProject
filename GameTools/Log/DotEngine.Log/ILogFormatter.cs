using System;

namespace DotEngine.Log
{
    public interface ILogFormatter
    {
        string FormatMessage(LogLevel level, string tag, DateTime time, string message, string stacktrace);
    }
}