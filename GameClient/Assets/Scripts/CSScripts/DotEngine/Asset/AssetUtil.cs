//using System;
//using SystemObject = System.Object;
//using UnityObject = UnityEngine.Object;

//namespace DotEngine.Asset
//{
//    public static class AssetUtil
//    {
//        #region Init Mgr
//        private static AssetManager assetMgr = null;
//        public static void InitDatabaseLoader(Action<bool> initCallback)
//        {
//            assetMgr = AssetManager.GetInstance();
//            assetMgr.InitManager(AssetLoaderMode.AssetDatabase, initCallback);
//        }

//        public static void InitBundleLoader(Action<bool> initCallback,string bundleRootDir)
//        {
//            assetMgr = AssetManager.GetInstance();
//            assetMgr.InitManager(AssetLoaderMode.AssetBundle, initCallback,bundleRootDir);
//        }

//        #endregion

//        public static void ChangeMaxLoadingCount(int count)
//        {
//            assetMgr.ChangeMaxLoadingCount(count);
//        }

//        #region LoadAssetAsync

//        public static AssetHandler LoadAssetAsync(
//            string address,
//            OnAssetLoadProgress progress,
//            OnAssetLoadComplete complete,
//            AssetLoaderPriority priority,
//            SystemObject userData)
//        {
//            return assetMgr.LoadAssetAsync(address, progress, complete, priority, userData);
//        }

//        public static AssetHandler LoadAssetAsync(
//            string address,
//            OnAssetLoadProgress progress,
//            OnAssetLoadComplete complete)
//        {
//            return assetMgr.LoadAssetAsync(address, progress, complete, AssetLoaderPriority.Default, null);
//        }

//        public static AssetHandler LoadAssetAsync(
//            string address,
//            OnAssetLoadComplete complete)
//        {
//            return assetMgr.LoadAssetAsync(address, null, complete, AssetLoaderPriority.Default, null);
//        }

//        public static AssetHandler LoadAssetAsync(
//            string address,
//            OnAssetLoadComplete complete,
//            SystemObject userData)
//        {
//            return assetMgr.LoadAssetAsync(address, null, complete, AssetLoaderPriority.Default, userData);
//        }

//        #endregion

//        #region InstanceAssetAsync
//        public static AssetHandler InstanceAssetAsync(
//            string address,
//            OnAssetLoadProgress progress,
//            OnAssetLoadComplete complete,
//            AssetLoaderPriority priority,
//            SystemObject userData)
//        {
//            return assetMgr.InstanceAssetAsync(address, progress, complete,  priority, userData);
//        }

//        public static AssetHandler InstanceAssetAsync(
//            string address,
//            OnAssetLoadProgress progress,
//            OnAssetLoadComplete complete)
//        {
//            return assetMgr.InstanceAssetAsync(address, progress, complete, AssetLoaderPriority.Default, null);
//        }

//        public static AssetHandler InstanceAssetAsync(
//            string address,
//            OnAssetLoadComplete complete)
//        {
//            return assetMgr.InstanceAssetAsync(address, null, complete, AssetLoaderPriority.Default, null);
//        }

//        public static AssetHandler InstanceAssetAsync(
//            string address,
//            OnAssetLoadComplete complete,
//            SystemObject userData)
//        {
//            return assetMgr.InstanceAssetAsync(address, null, complete, AssetLoaderPriority.Default, userData);
//        }

//        #endregion

//        #region LoadBatchAssetAsync
//        public static AssetHandler LoadBatchAssetAsync(
//             string[] addresses,
//            OnAssetLoadProgress progress,
//            OnAssetLoadComplete complete,
//            OnBatchAssetsLoadProgress batchProgress,
//            OnBatchAssetLoadComplete batchComplete,
//            AssetLoaderPriority priority,
//            SystemObject userData)
//        {
//            return assetMgr.LoadBatchAssetAsync(addresses, progress, complete, batchProgress, batchComplete, priority, userData);
//        }

//        public static AssetHandler LoadBatchAssetAsync(
//             string[] addresses,
//            OnAssetLoadComplete complete,
//            OnBatchAssetLoadComplete batchComplete,
//            AssetLoaderPriority priority,
//            SystemObject userData)
//        {
//            return assetMgr.LoadBatchAssetAsync(addresses, null, complete, null, batchComplete, priority, userData);
//        }

//        public static AssetHandler LoadBatchAssetAsync(
//             string[] addresses,
//            OnAssetLoadComplete complete,
//            OnBatchAssetLoadComplete batchComplete,
//            SystemObject userData)
//        {
//            return assetMgr.LoadBatchAssetAsync(addresses, null, complete, null, batchComplete, AssetLoaderPriority.Default, userData);
//        }

//        public static AssetHandler LoadBatchAssetAsync(
//             string[] addresses,
//            OnAssetLoadComplete complete,
//            OnBatchAssetLoadComplete batchComplete)
//        {
//            return assetMgr.LoadBatchAssetAsync(addresses, null, complete, null, batchComplete, AssetLoaderPriority.Default, null);
//        }

//        public static AssetHandler LoadBatchAssetAsync(
//             string[] addresses,
//            OnBatchAssetLoadComplete batchComplete)
//        {
//            return assetMgr.LoadBatchAssetAsync(addresses, null, null, null, batchComplete, AssetLoaderPriority.Default, null);
//        }

//        public static AssetHandler LoadBatchAssetAsync(
//             string[] addresses,
//            OnBatchAssetLoadComplete batchComplete,
//            SystemObject userData)
//        {
//            return assetMgr.LoadBatchAssetAsync(addresses, null, null, null, batchComplete, AssetLoaderPriority.Default, userData);
//        }
//        #endregion

//        #region LoadBatchAssetAsyncByLabel

//        public static AssetHandler LoadBatchAssetAsyncByLabel(
//            string label,
//            OnAssetLoadProgress progress,
//            OnAssetLoadComplete complete,
//            OnBatchAssetsLoadProgress batchProgress,
//            OnBatchAssetLoadComplete batchComplete,
//            AssetLoaderPriority priority,
//            SystemObject userData
//            )
//        {
//            return assetMgr.LoadBatchAssetAsyncByLabel(label, progress, complete, batchProgress, batchComplete, priority, userData);
//        }

//        public static AssetHandler LoadBatchAssetAsyncByLabel(
//            string label,
//            OnAssetLoadComplete complete,
//            OnBatchAssetLoadComplete batchComplete,
//            AssetLoaderPriority priority,
//            SystemObject userData
//            )
//        {
//            return assetMgr.LoadBatchAssetAsyncByLabel(label, null, complete, null, batchComplete, priority, userData);
//        }

//        public static AssetHandler LoadBatchAssetAsyncByLabel(
//            string label,
//            OnAssetLoadComplete complete,
//            OnBatchAssetLoadComplete batchComplete,
//            SystemObject userData
//            )
//        {
//            return assetMgr.LoadBatchAssetAsyncByLabel(label, null, complete, null, batchComplete, AssetLoaderPriority.Default, userData);
//        }

//        public static AssetHandler LoadBatchAssetAsyncByLabel(
//            string label,
//            OnBatchAssetLoadComplete batchComplete,
//            SystemObject userData
//            )
//        {
//            return assetMgr.LoadBatchAssetAsyncByLabel(label, null, null, null, batchComplete, AssetLoaderPriority.Default, userData);
//        }

//        public static AssetHandler LoadBatchAssetAsyncByLabel(
//            string label,
//            OnBatchAssetLoadComplete batchComplete
//            )
//        {
//            return assetMgr.LoadBatchAssetAsyncByLabel(label, null, null, null, batchComplete, AssetLoaderPriority.Default, null);
//        }

//        #endregion

//        #region InstanceBatchAssetAsync
//        public static AssetHandler InstanceBatchAssetAsync(
//            string[] addresses,
//           OnAssetLoadProgress progress,
//           OnAssetLoadComplete complete,
//           OnBatchAssetsLoadProgress batchProgress,
//           OnBatchAssetLoadComplete batchComplete,
//           AssetLoaderPriority priority,
//           SystemObject userData)
//        {
//            return assetMgr.InstanceBatchAssetAsync(addresses, progress, complete, batchProgress, batchComplete, priority, userData);
//        }

//        public static AssetHandler InstanceBatchAssetAsync(
//             string[] addresses,
//            OnAssetLoadComplete complete,
//            OnBatchAssetLoadComplete batchComplete,
//            AssetLoaderPriority priority,
//            SystemObject userData)
//        {
//            return assetMgr.InstanceBatchAssetAsync(addresses, null, complete, null, batchComplete, priority, userData);
//        }

//        public static AssetHandler InstanceBatchAssetAsync(
//             string[] addresses,
//            OnAssetLoadComplete complete,
//            OnBatchAssetLoadComplete batchComplete,
//            SystemObject userData)
//        {
//            return assetMgr.InstanceBatchAssetAsync(addresses, null, complete, null, batchComplete, AssetLoaderPriority.Default, userData);
//        }

//        public static AssetHandler InstanceBatchAssetAsync(
//             string[] addresses,
//            OnAssetLoadComplete complete,
//            OnBatchAssetLoadComplete batchComplete)
//        {
//            return assetMgr.InstanceBatchAssetAsync(addresses, null, complete, null, batchComplete, AssetLoaderPriority.Default, null);
//        }

//        public static AssetHandler InstanceBatchAssetAsync(
//             string[] addresses,
//            OnBatchAssetLoadComplete batchComplete)
//        {
//            return assetMgr.InstanceBatchAssetAsync(addresses, null, null, null, batchComplete, AssetLoaderPriority.Default, null);
//        }

//        public static AssetHandler InstanceBatchAssetAsync(
//             string[] addresses,
//            OnBatchAssetLoadComplete batchComplete,
//            SystemObject userData)
//        {
//            return assetMgr.InstanceBatchAssetAsync(addresses, null, null, null, batchComplete, AssetLoaderPriority.Default, userData);
//        }

//        #endregion

//        #region InstanceBatchAssetAsyncByLabel
//        public static AssetHandler InstanceBatchAssetAsyncByLabel(
//            string label,
//            OnAssetLoadProgress progress,
//            OnAssetLoadComplete complete,
//            OnBatchAssetsLoadProgress batchProgress,
//            OnBatchAssetLoadComplete batchComplete,
//            AssetLoaderPriority priority,
//            SystemObject userData
//            )
//        {
//            return assetMgr.InstanceBatchAssetAsyncByLabel(label, progress, complete, batchProgress, batchComplete, priority, userData);
//        }

//        public static AssetHandler InstanceBatchAssetAsyncByLabel(
//            string label,
//            OnAssetLoadComplete complete,
//            OnBatchAssetLoadComplete batchComplete,
//            AssetLoaderPriority priority,
//            SystemObject userData
//            )
//        {
//            return assetMgr.InstanceBatchAssetAsyncByLabel(label, null, complete, null, batchComplete, priority, userData);
//        }

//        public static AssetHandler InstanceBatchAssetAsyncByLabel(
//            string label,
//            OnAssetLoadComplete complete,
//            OnBatchAssetLoadComplete batchComplete,
//            SystemObject userData
//            )
//        {
//            return assetMgr.InstanceBatchAssetAsyncByLabel(label, null, complete, null, batchComplete, AssetLoaderPriority.Default, userData);
//        }

//        public static AssetHandler InstanceBatchAssetAsyncByLabel(
//            string label,
//            OnBatchAssetLoadComplete batchComplete,
//            SystemObject userData
//            )
//        {
//            return assetMgr.InstanceBatchAssetAsyncByLabel(label, null, null, null, batchComplete, AssetLoaderPriority.Default, userData);
//        }

//        public static AssetHandler InstanceBatchAssetAsyncByLabel(
//            string label,
//            OnBatchAssetLoadComplete batchComplete
//            )
//        {
//            return assetMgr.InstanceBatchAssetAsyncByLabel(label, null, null, null, batchComplete, AssetLoaderPriority.Default, null);
//        }

//        #endregion

//        #region UnloadAssetAsync
//        public static void UnloadAssetAsync(AssetHandler handler)
//        {
//            assetMgr.UnloadAssetAsync(handler,false);
//        }

//        public static void UnloadAssetAsync(AssetHandler handler, bool destroyIfIsInstnace)
//        {
//            assetMgr.UnloadAssetAsync(handler, destroyIfIsInstnace);
//        }
//        #endregion

//        public static UnityObject InstantiateAsset(string address, UnityObject asset)
//        {
//            return assetMgr.InstantiateAsset(address, asset);
//        }

//        #region UnloadUnusedAsset
//        public static void UnloadUnusedAsset(Action callback)
//        {
//            assetMgr.UnloadUnusedAsset(callback);
//        }

//        public static void UnloadUnusedAsset()
//        {
//            assetMgr.UnloadUnusedAsset(null);
//        }
//        #endregion
//    }
//}
