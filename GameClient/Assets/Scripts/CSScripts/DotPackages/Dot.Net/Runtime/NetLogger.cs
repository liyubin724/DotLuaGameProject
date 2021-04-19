using System.Diagnostics;

namespace DotEngine.Net
{
    public static class NetLogger
    {
        public static void LogError(string tag, string message)
        {
            UnityEngine.Debug.LogError(GetMessage("ERROR", tag, message));
        }

        [Conditional("DEBUG")]
        public static void LogWarning(string tag, string message)
        {
            UnityEngine.Debug.LogWarning(GetMessage("WARNING", tag, message));
        }

        [Conditional("DEBUG")]
        public static void LogInfo(string tag, string message)
        {
            UnityEngine.Debug.Log(GetMessage("INFO", tag, message));
        }

        private static string GetMessage(string level, string tag, string message)
        {
            return $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss-fff")} {level} {tag} {message}";
        }
    }
}
