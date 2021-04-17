using System;

namespace DotEngine
{
    public static class DebugLog
    {
        private static readonly string DEFAULT_TAG = "DebugLog";

        public static bool IsDebugging { get; set; } = true;

        private static Action<string, string> sm_LogInfoAction = (tag, message) => { UnityEngine.Debug.Log(GetMessage("INFO", tag, message); };
        private static Action<string, string> sm_LogWarningAction = (tag, message) => { UnityEngine.Debug.LogWarning(GetMessage("WARNING", tag, message); };
        private static Action<string, string> sm_LogErrorAction = (tag, message) => { UnityEngine.Debug.LogError(GetMessage("ERROR", tag, message); };

        public static void SetAction(Action<string, string> logInfo, Action<string, string> logWarning, Action<string, string> logError)
        {
            sm_LogInfoAction = logInfo;
            sm_LogWarningAction = logWarning;
            sm_LogErrorAction = logError;
        }

        public static void Error(string message)
        {
            sm_LogErrorAction?.Invoke(DEFAULT_TAG, message);
        }

        public static void Error(string tag, string message)
        {
            sm_LogErrorAction?.Invoke(tag, message);
        }

        public static void Warning(string message)
        {
            sm_LogWarningAction?.Invoke(DEFAULT_TAG, message);
        }

        public static void Warning(string tag, string message)
        {
            sm_LogWarningAction?.Invoke(tag, message);
        }


        public static void Info(string message)
        {
            sm_LogInfoAction?.Invoke(DEFAULT_TAG, message);
        }

        public static void Info(string tag, string message)
        {
            sm_LogInfoAction?.Invoke(tag, message);
        }

        public static void Debug(string message)
        {
            if (IsDebugging && sm_LogInfoAction != null)
            {
                sm_LogInfoAction(DEFAULT_TAG, message);
            }
        }

        public static void Debug(string tag, string message)
        {
            if (IsDebugging && sm_LogInfoAction != null)
            {
                sm_LogInfoAction(tag, message);
            }
        }

        private static string GetMessage(string level, string tag, string message)
        {
            return $"{DateTime.Now.ToString("MM-dd HH:mm:ss-fff")} {level} {tag} {message}";
        }
    }
}
