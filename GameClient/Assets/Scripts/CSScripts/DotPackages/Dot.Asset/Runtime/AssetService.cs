using DotEngine.Services;
using System;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Asset
{
    public class AssetService : Servicer, IUpdate
    {
        public const string NAME = "AssetService";

        private AssetManager assetMgr = null;

        public AssetService() :base(NAME)
        {
        }

        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {
            assetMgr?.DoUpdate(deltaTime);
        }

        public override void DoRemove()
        {
            assetMgr.DoDispose();
            assetMgr = null;
        }

        #region init Mgr
        public void InitDatabaseLoader(Action<bool> initCallback)
        {
            assetMgr = AssetManager.GetInstance();
            assetMgr.InitManager(AssetLoaderMode.AssetDatabase, initCallback);
        }

        public void InitBundleLoader(Action<bool> initCallback, string bundleRootDir)
        {
            assetMgr = AssetManager.GetInstance();
            assetMgr.InitManager(AssetLoaderMode.AssetBundle, initCallback, bundleRootDir);
        }
        #endregion

        public void ChangeMaxLoadingCount(int count)
        {
            assetMgr.ChangeMaxLoadingCount(count);
        }

        #region LoadAssetAsync

        public AssetHandler LoadAssetAsync(
            string address,
            OnAssetLoadProgress progress,
            OnAssetLoadComplete complete,
            AssetLoaderPriority priority,
            SystemObject userData)
        {
            return assetMgr.LoadAssetAsync(address, progress, complete, priority, userData);
        }

        public AssetHandler LoadAssetAsync(
            string address,
            OnAssetLoadProgress progress,
            OnAssetLoadComplete complete)
        {
            return assetMgr.LoadAssetAsync(address, progress, complete, AssetLoaderPriority.Default, null);
        }

        public AssetHandler LoadAssetAsync(
            string address,
            OnAssetLoadComplete complete)
        {
            return assetMgr.LoadAssetAsync(address, null, complete, AssetLoaderPriority.Default, null);
        }

        public AssetHandler LoadAssetAsync(
            string address,
            OnAssetLoadComplete complete,
            SystemObject userData)
        {
            return assetMgr.LoadAssetAsync(address, null, complete, AssetLoaderPriority.Default, userData);
        }

        #endregion

        #region InstanceAssetAsync
        public AssetHandler InstanceAssetAsync(
            string address,
            OnAssetLoadProgress progress,
            OnAssetLoadComplete complete,
            AssetLoaderPriority priority,
            SystemObject userData)
        {
            return assetMgr.InstanceAssetAsync(address, progress, complete, priority, userData);
        }

        public AssetHandler InstanceAssetAsync(
            string address,
            OnAssetLoadProgress progress,
            OnAssetLoadComplete complete)
        {
            return assetMgr.InstanceAssetAsync(address, progress, complete, AssetLoaderPriority.Default, null);
        }

        public AssetHandler InstanceAssetAsync(
            string address,
            OnAssetLoadComplete complete)
        {
            return assetMgr.InstanceAssetAsync(address, null, complete, AssetLoaderPriority.Default, null);
        }

        public AssetHandler InstanceAssetAsync(
            string address,
            OnAssetLoadComplete complete,
            SystemObject userData)
        {
            return assetMgr.InstanceAssetAsync(address, null, complete, AssetLoaderPriority.Default, userData);
        }

        #endregion

        #region LoadBatchAssetAsync
        public AssetHandler LoadBatchAssetAsync(
             string[] addresses,
            OnAssetLoadProgress progress,
            OnAssetLoadComplete complete,
            OnBatchAssetsLoadProgress batchProgress,
            OnBatchAssetLoadComplete batchComplete,
            AssetLoaderPriority priority,
            SystemObject userData)
        {
            return assetMgr.LoadBatchAssetAsync(addresses, progress, complete, batchProgress, batchComplete, priority, userData);
        }

        public AssetHandler LoadBatchAssetAsync(
             string[] addresses,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            AssetLoaderPriority priority,
            SystemObject userData)
        {
            return assetMgr.LoadBatchAssetAsync(addresses, null, complete, null, batchComplete, priority, userData);
        }

        public AssetHandler LoadBatchAssetAsync(
             string[] addresses,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            SystemObject userData)
        {
            return assetMgr.LoadBatchAssetAsync(addresses, null, complete, null, batchComplete, AssetLoaderPriority.Default, userData);
        }

        public AssetHandler LoadBatchAssetAsync(
             string[] addresses,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete)
        {
            return assetMgr.LoadBatchAssetAsync(addresses, null, complete, null, batchComplete, AssetLoaderPriority.Default, null);
        }

        public AssetHandler LoadBatchAssetAsync(
             string[] addresses,
            OnBatchAssetLoadComplete batchComplete)
        {
            return assetMgr.LoadBatchAssetAsync(addresses, null, null, null, batchComplete, AssetLoaderPriority.Default, null);
        }

        public AssetHandler LoadBatchAssetAsync(
             string[] addresses,
            OnBatchAssetLoadComplete batchComplete,
            SystemObject userData)
        {
            return assetMgr.LoadBatchAssetAsync(addresses, null, null, null, batchComplete, AssetLoaderPriority.Default, userData);
        }
        #endregion

        #region LoadBatchAssetAsyncByLabel

        public AssetHandler LoadBatchAssetAsyncByLabel(
            string label,
            OnAssetLoadProgress progress,
            OnAssetLoadComplete complete,
            OnBatchAssetsLoadProgress batchProgress,
            OnBatchAssetLoadComplete batchComplete,
            AssetLoaderPriority priority,
            SystemObject userData
            )
        {
            return assetMgr.LoadBatchAssetAsyncByLabel(label, progress, complete, batchProgress, batchComplete, priority, userData);
        }

        public AssetHandler LoadBatchAssetAsyncByLabel(
            string label,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            AssetLoaderPriority priority,
            SystemObject userData
            )
        {
            return assetMgr.LoadBatchAssetAsyncByLabel(label, null, complete, null, batchComplete, priority, userData);
        }

        public AssetHandler LoadBatchAssetAsyncByLabel(
            string label,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            SystemObject userData
            )
        {
            return assetMgr.LoadBatchAssetAsyncByLabel(label, null, complete, null, batchComplete, AssetLoaderPriority.Default, userData);
        }

        public AssetHandler LoadBatchAssetAsyncByLabel(
            string label,
            OnBatchAssetLoadComplete batchComplete,
            SystemObject userData
            )
        {
            return assetMgr.LoadBatchAssetAsyncByLabel(label, null, null, null, batchComplete, AssetLoaderPriority.Default, userData);
        }

        public AssetHandler LoadBatchAssetAsyncByLabel(
            string label,
            OnBatchAssetLoadComplete batchComplete
            )
        {
            return assetMgr.LoadBatchAssetAsyncByLabel(label, null, null, null, batchComplete, AssetLoaderPriority.Default, null);
        }

        #endregion

        #region InstanceBatchAssetAsync
        public AssetHandler InstanceBatchAssetAsync(
            string[] addresses,
           OnAssetLoadProgress progress,
           OnAssetLoadComplete complete,
           OnBatchAssetsLoadProgress batchProgress,
           OnBatchAssetLoadComplete batchComplete,
           AssetLoaderPriority priority,
           SystemObject userData)
        {
            return assetMgr.InstanceBatchAssetAsync(addresses, progress, complete, batchProgress, batchComplete, priority, userData);
        }

        public AssetHandler InstanceBatchAssetAsync(
             string[] addresses,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            AssetLoaderPriority priority,
            SystemObject userData)
        {
            return assetMgr.InstanceBatchAssetAsync(addresses, null, complete, null, batchComplete, priority, userData);
        }

        public AssetHandler InstanceBatchAssetAsync(
             string[] addresses,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            SystemObject userData)
        {
            return assetMgr.InstanceBatchAssetAsync(addresses, null, complete, null, batchComplete, AssetLoaderPriority.Default, userData);
        }

        public AssetHandler InstanceBatchAssetAsync(
             string[] addresses,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete)
        {
            return assetMgr.InstanceBatchAssetAsync(addresses, null, complete, null, batchComplete, AssetLoaderPriority.Default, null);
        }

        public AssetHandler InstanceBatchAssetAsync(
             string[] addresses,
            OnBatchAssetLoadComplete batchComplete)
        {
            return assetMgr.InstanceBatchAssetAsync(addresses, null, null, null, batchComplete, AssetLoaderPriority.Default, null);
        }

        public AssetHandler InstanceBatchAssetAsync(
             string[] addresses,
            OnBatchAssetLoadComplete batchComplete,
            SystemObject userData)
        {
            return assetMgr.InstanceBatchAssetAsync(addresses, null, null, null, batchComplete, AssetLoaderPriority.Default, userData);
        }

        #endregion

        #region InstanceBatchAssetAsyncByLabel
        public AssetHandler InstanceBatchAssetAsyncByLabel(
            string label,
            OnAssetLoadProgress progress,
            OnAssetLoadComplete complete,
            OnBatchAssetsLoadProgress batchProgress,
            OnBatchAssetLoadComplete batchComplete,
            AssetLoaderPriority priority,
            SystemObject userData
            )
        {
            return assetMgr.InstanceBatchAssetAsyncByLabel(label, progress, complete, batchProgress, batchComplete, priority, userData);
        }

        public AssetHandler InstanceBatchAssetAsyncByLabel(
            string label,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            AssetLoaderPriority priority,
            SystemObject userData
            )
        {
            return assetMgr.InstanceBatchAssetAsyncByLabel(label, null, complete, null, batchComplete, priority, userData);
        }

        public AssetHandler InstanceBatchAssetAsyncByLabel(
            string label,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            SystemObject userData
            )
        {
            return assetMgr.InstanceBatchAssetAsyncByLabel(label, null, complete, null, batchComplete, AssetLoaderPriority.Default, userData);
        }

        public AssetHandler InstanceBatchAssetAsyncByLabel(
            string label,
            OnBatchAssetLoadComplete batchComplete,
            SystemObject userData
            )
        {
            return assetMgr.InstanceBatchAssetAsyncByLabel(label, null, null, null, batchComplete, AssetLoaderPriority.Default, userData);
        }

        public AssetHandler InstanceBatchAssetAsyncByLabel(
            string label,
            OnBatchAssetLoadComplete batchComplete
            )
        {
            return assetMgr.InstanceBatchAssetAsyncByLabel(label, null, null, null, batchComplete, AssetLoaderPriority.Default, null);
        }

        #endregion

        #region UnloadAssetAsync
        public void UnloadAssetAsync(AssetHandler handler)
        {
            assetMgr.UnloadAssetAsync(handler, false);
        }

        public void UnloadAssetAsync(AssetHandler handler, bool destroyIfIsInstnace)
        {
            assetMgr.UnloadAssetAsync(handler, destroyIfIsInstnace);
        }
        #endregion

        public UnityObject InstantiateAsset(string address, UnityObject asset)
        {
            return assetMgr.InstantiateAsset(address, asset);
        }

        #region UnloadUnusedAsset
        public void UnloadUnusedAsset(Action callback = null)
        {
            assetMgr.UnloadUnusedAsset(callback);
        }
        #endregion
    }
}
