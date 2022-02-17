using DotEngine.Log;
using System;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UPool
{
    public static class PoolUtill
    {
        internal static ILogger Logger = LogUtil.GetLogger("UPoolMgr");

        public static bool IsDebug { get; set; } = true;

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
