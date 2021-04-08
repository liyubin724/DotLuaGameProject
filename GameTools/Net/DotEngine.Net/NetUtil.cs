using System;

namespace DotEngine.Net
{
    public static class NetUtil
    {
        public static readonly string CLIENT_LOG_TAG = "ClientNet";
        public static readonly string SERVER_LOG_TAG = "ServerNet";

        public static bool EnableLog { get; set; } = false;
        public static void LogInfo(string tag,string message)
        {
            if(EnableLog)
            {
                UnityEngine.Debug.Log($"{DateTime.Now.ToString("yy-MM-dd HH: mm:ss-fff")} INFO {tag} {message}");
            }
        }

        public static void LogError(string tag,string message)
        {
            UnityEngine.Debug.LogError($"{DateTime.Now.ToString("yy-MM-dd HH: mm:ss-fff")} INFO {tag} {message}");
        }

        public static void LogWarning(string tag,string message)
        {
            UnityEngine.Debug.LogWarning($"{DateTime.Now.ToString("yy-MM-dd HH: mm:ss-fff")} INFO {tag} {message}");
        }
    }
}
