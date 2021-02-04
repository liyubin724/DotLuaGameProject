using System;
using UnityObject = UnityEngine.Object;

namespace DotEngine.GOP
{
    public static class GOPoolUtil
    {
        internal static readonly string LOGGER_NAME = "GOPool";

        public static Func<string, UnityObject, UnityObject> InstantiateAsset;

        public static bool EnableLog { get; set; } = true;
        public static Action<string, string> LogInfoAction = null;
        public static Action<string, string> LogWarningAction = null;
        public static Action<string, string> LogErrorAction = null;

        public static void LogInfo(string message)
        {
            if(EnableLog && LogInfoAction!=null)
            {
                LogInfoAction(LOGGER_NAME, message);
            }
        }

        public static void LogWarning(string message)
        {
            if(EnableLog && LogWarningAction !=null)
            {
                LogWarningAction(LOGGER_NAME, message);
            }
        }

        public static void LogError(string message)
        {
            if(EnableLog && LogErrorAction!=null)
            {
                LogErrorAction(LOGGER_NAME, message);
            }
        }
    }
}
