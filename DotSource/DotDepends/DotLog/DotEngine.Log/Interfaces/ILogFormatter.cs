using System;

namespace DotEngine.Log
{
    public interface ILogFormatter
    {
        string FormatMessage(DateTime time, string tag, LogLevel level, string message, string stacktree);
    }
}