using System;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UPool
{
    public static class UGOPoolUtill
    {
        internal static string LOG_TAG = "UGOPool";

        public static bool IsDebug { get; set; } = false;

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
    }
}
