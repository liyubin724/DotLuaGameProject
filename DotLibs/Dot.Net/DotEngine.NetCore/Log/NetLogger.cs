using System;

namespace DotEngine.NetCore
{
    public static class NetLogger
    {
        public static Action<string, string> LogHandler { get; set; }
        public static Action<string, string> WarningHandler { get; set; }
        public static Action<string, string> ErrorHandler { get; set; }

        public static void Log(string tag, string message)
        {
            LogHandler?.Invoke(tag, message);
        }

        public static void Warning(string tag, string message)
        {
            WarningHandler?.Invoke(tag, message);
        }

        public static void Error(string tag, string message)
        {
            ErrorHandler?.Invoke(tag, message);
        }
    }
}
