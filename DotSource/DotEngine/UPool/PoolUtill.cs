using DotEngine.Log;
using System;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UPool
{
    public static class PoolUtill
    {
        #region Logger
        private const string LOGGER_TAG = "UPool";
        internal static void Error(string message)
        {
            LogUtil.GetLogger(LOGGER_TAG).Error(message);
        }

        internal static void Warning(string message)
        {
            LogUtil.GetLogger(LOGGER_TAG).Warning(message);
        }

        internal static void Info(string message)
        {
            LogUtil.GetLogger(LOGGER_TAG).Info(message);
        }
        #endregion

        public static bool IsDebug { get; set; } = true;

        #region asset
        internal static Func<string, UnityObject, UnityObject> InstantiateProvider { get; set; } = (assetPath, uObj) =>
        {
            return UnityObject.Instantiate(uObj);
        };

        internal static Action<string, UnityObject> DestroyProvider { get; set; } = (assetPath, uObj) =>
         {
             UnityObject.Destroy(uObj);
         };

        public static void SetAssetProvider(Func<string, UnityObject, UnityObject> instantiateProvider, Action<string, UnityObject> destroyProvider)
        {
            if (instantiateProvider != null)
            {
                InstantiateProvider = instantiateProvider;
            }
            if (destroyProvider != null)
            {
                DestroyProvider = destroyProvider;
            }
        }
        #endregion

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
