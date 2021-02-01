using System;

namespace DotEngine
{
    public static class DebugLog
    {
        private static readonly string DEFAULT_TAG = "debug-log";

        private static Action<string, string> sm_LogInfoAction = (tag, message) => { UnityEngine.Debug.Log($"{tag} {message}"); };
        private static Action<string, string> sm_LogWarningAction = (tag, message) => { UnityEngine.Debug.LogWarning($"{tag} {message}"); };
        private static Action<string, string> sm_LogErrorAction = (tag, message) => { UnityEngine.Debug.LogError($"{tag} {message}"); };

    public static bool IsDebugging { get; set; } = true;

        public static void SetLogAction(Action<string,string> logInfo, Action<string,string> logWarning, Action<string,string> logError)
        {
            sm_LogInfoAction = logInfo;
            sm_LogWarningAction = logWarning;
            sm_LogErrorAction = logError;
        }

        public static void Error(string message)
        {
            sm_LogErrorAction?.Invoke(DEFAULT_TAG, message);
        }

        public static void Error(string tag,string message)
        {
            sm_LogErrorAction?.Invoke(tag,message);
        }

        public static void Warning(string message)
        {
            sm_LogWarningAction?.Invoke(DEFAULT_TAG, message);
        }

        public static void Warning(string tag,string message)
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
                sm_LogInfoAction(DEFAULT_TAG,message);
            }
        }

        public static void Debug(string tag,string message)
        {
            if (IsDebugging && sm_LogInfoAction != null)
            {
                sm_LogInfoAction(tag, message);
            }
        }

    }
}
