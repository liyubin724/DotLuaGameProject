using System;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UPool
{
    public static class GOPoolUtil
    {
        internal static readonly string LOGGER_NAME = "GOPool";

        public static Func<string, UnityObject, UnityObject> InstantiateAsset;

        public static Action<string,string> LogInfoAction = null;
        public static Action<string, string> LogWarningAction = null;
        public static Action<string, string> LogErrorAction = null;

        internal static void LogInfo(string message)
        {
            LogInfoAction?.Invoke(LOGGER_NAME, message);
        }

        internal static void LogWarning(string message)
        {
            LogWarningAction?.Invoke(LOGGER_NAME, message);
        }

        internal static void LogError(string message)
        {
            LogErrorAction?.Invoke(LOGGER_NAME, message);
        }

        internal static bool IsNull(this UnityObject obj)
        {
            if (obj == null || obj.Equals(null))
            {
                return true;
            }
            return false;
        }
    }
}
