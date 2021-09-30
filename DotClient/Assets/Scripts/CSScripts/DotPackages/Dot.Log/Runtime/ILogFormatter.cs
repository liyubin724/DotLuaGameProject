using System;

namespace DotEngine.Log
{
    public interface ILogFormatter
    {
        string FormatMessage(LogLevel level, DateTime dateTime, string tag, string message, string stacktrace);
    }
}