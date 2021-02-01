using System;

namespace DotEngine
{
    public static class DebugLog
    {
        private static Action<string> sm_LogInfoAction = UnityEngine.Debug.Log;
        private static Action<string> sm_LogWarningAction = UnityEngine.Debug.LogWarning;
        private static Action<string> sm_LogErrorAction = UnityEngine.Debug.LogError;

        public static bool IsDebugging { get; set; } = true;

        public static void SetLogAction(Action<string> logInfo, Action<string> logWarning, Action<string> logError)
        {
            sm_LogInfoAction = logInfo;
            sm_LogWarningAction = logWarning;
            sm_LogErrorAction = logError;
        }

        public static void Error(string message)
        {
            sm_LogErrorAction?.Invoke(message);
        }

        public static void Warning(string message)
        {
            sm_LogWarningAction?.Invoke(message);
        }

        public static void Info(string message)
        {
            sm_LogInfoAction?.Invoke(message);
        }

        public static void Debug(string message)
        {
            if (IsDebugging && sm_LogInfoAction != null)
            {
                sm_LogInfoAction(message);
            }
        }

    }
}
