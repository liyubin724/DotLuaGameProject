﻿using System;

namespace DotEngine.Log
{
    public class DefaultLogFormatter : ILogFormatter
    {
        public string FormatMessage(LogLevel level, DateTime dateTime, string tag, string message, string stacktrace)
        {
            return $"{DateTime.Now.ToString("MM-dd HH:mm:ss-fff")} {level.ToString().ToUpper()} {tag} {message} {stacktrace}";
        }
    }
}