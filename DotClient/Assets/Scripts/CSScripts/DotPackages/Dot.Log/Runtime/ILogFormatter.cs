using System;

namespace DotEngine.Log
{
    public interface ILogFormatter
    {
        string FormatMessage(string tag, LogLevel level, DateTime dateTime, string message, string stacktrace);
    }
}