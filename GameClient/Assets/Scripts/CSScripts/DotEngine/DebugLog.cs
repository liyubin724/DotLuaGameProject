using System;
using UnityEngine;

namespace DotEngine
{
    public static class DebugLog
    {
        private static Action<string> sm_LogInfoAction = Debug.Log;
        private static Action<string> sm_LogWarningAction = Debug.LogWarning;
        private static Action<string> sm_LogErrorAction = Debug.LogError;

        public static bool IsDebug { get; set; } = true;

        public static void SetLogAction(Action<string> logInfo, Action<string> logWarning, Action<string> logError)
        {
            if (logInfo != null)
            {
                sm_LogInfoAction = logInfo;
            }

            if (logWarning != null)
            {
                sm_LogWarningAction = logWarning;
            }

            if (logError != null)
            {
                sm_LogErrorAction = logError;
            }
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
            if (IsDebug && sm_LogInfoAction != null)
            {
                sm_LogInfoAction(message);
            }
        }
    }
}
