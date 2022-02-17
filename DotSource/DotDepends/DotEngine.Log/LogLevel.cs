using System;

namespace DotEngine.Log
{
    [Flags]
    public enum LogLevel
    {
        Off = 0,
        Trace = 1 << 0,
        Debug = 1 << 1,
        Info = 1 << 2,
        Warning = 1 << 3,
        Error = 1 << 4,
        Fatal = 1 << 5,
    }

    public static class LogLevelConst
    {
        public const LogLevel None = LogLevel.Off;

        public const LogLevel All = LogLevel.Trace | LogLevel.Debug | LogLevel.Info | LogLevel.Warning | LogLevel.Error | LogLevel.Fatal;
        public const LogLevel Default = LogLevel.Info | LogLevel.Warning | LogLevel.Error | LogLevel.Fatal;
        public const LogLevel Serious = LogLevel.Error | LogLevel.Fatal;
    }
}
