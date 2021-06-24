using System;

namespace DotEngine.NetCore
{
    public enum NetLogType
    {
        Info = 0,
        Warning,
        Error,
    }

    public static class NetLogger
    {
        public static Action<NetLogType, string, string> LogHandler = null;

        public static void Log(string tag, string message)
        {
            LogHandler?.Invoke(NetLogType.Info, tag, message);
        }

        public static void Warning(string tag, string message)
        {
            LogHandler?.Invoke(NetLogType.Warning, tag, message);
        }

        public static void Error(string tag, string message)
        {
            LogHandler?.Invoke(NetLogType.Error, tag, message);
        }
    }
}
