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

        All = Trace + Debug + Warning + Error + Fatal,
    }

    public static class LogLevelConst
    {
        public static readonly LogLevel None = LogLevel.Off;
        public static readonly LogLevel All = LogLevel.Trace | LogLevel.Debug | LogLevel.Info | LogLevel.Warning | LogLevel.Error | LogLevel.Fatal;

        public static readonly LogLevel Default = LogLevel.Info | LogLevel.Warning | LogLevel.Error | LogLevel.Fatal;
        public static readonly LogLevel Serious = LogLevel.Error | LogLevel.Fatal;
    }
}
