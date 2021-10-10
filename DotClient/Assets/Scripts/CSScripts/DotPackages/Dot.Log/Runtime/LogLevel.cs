using System;

namespace DotEngine.Log
{
    [Flags]
    public enum LogLevel
    {
        Off = 0,
        Debug = 1 << 0,
        Info = 1 << 1,
        Warning = 1 << 2,
        Error = 1 << 3,
    }

    public static class LogLevelConst
    {
        public const LogLevel None = LogLevel.Off;

        public const LogLevel All = LogLevel.Debug | LogLevel.Info | LogLevel.Warning | LogLevel.Error;
        public const LogLevel Default = LogLevel.Info | LogLevel.Warning | LogLevel.Error;
        public const LogLevel Serious = LogLevel.Error;
    }
}
