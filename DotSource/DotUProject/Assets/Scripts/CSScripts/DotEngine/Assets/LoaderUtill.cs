using DotEngine.Core;
using DotEngine.Serialization;
using System;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public static class LoaderUtill
    {
        private static ILoader loader = null;
        private static LoaderBehaviour loaderBehaviour = null;
        public static void Init(OnInitFinished finishedCallback)
        {
#if UNITY_EDITOR
            string assetFilePath = AssetConst.GetAssetDetailConfigFullPathForDatabase();
            AssetDetailConfig assetDetailConfig = JSONSerializer.ReadFromFile<AssetDetailConfig>(assetFilePath);
            assetDetailConfig.InitConfig();

            loader = new DatabaseLoader();
            loader.DoInitialize(assetDetailConfig, (result) =>
            {
                Debug.Log("LoaderUtill::Init->init finished result = " + result);
                finishedCallback?.Invoke(result);
            });
#endif

//#if LOADER_BUNDLE
//            string assetFilePath = AssetConst.GetAssetDetailConfigFullPathForBundle();
//            AssetDetailConfig assetDetailConfig = JSONReader.ReadFromFile<AssetDetailConfig>(assetFilePath);
//            assetDetailConfig.InitConfig();

//            string bundleFilePath = AssetConst.GetBundleDetailConfigFullPath();
//            BundleDetailConfig bundleDetailConfig = JSONReader.ReadFromFile<BundleDetailConfig>(bundleFilePath);
//            bundleDetailConfig.InitConfig();

//            loader = new BundleLoader();
//            loader.DoInitialize(assetDetailConfig, (result) =>
//            {
//                Debug.Log("LoaderUtill::Init->init finished result = " + result);
//                finishedCallback?.Invoke(result);
//            }, AssetConst.GetRootFullDirForBundle(), bundleDetailConfig);
//#elif LOADER_RESOURCE

//#else
//            string assetFilePath = AssetConst.GetAssetDetailConfigFullPathForDatabase();
//            AssetDetailConfig assetDetailConfig = JSONSerializer.ReadFromFile<AssetDetailConfig>(assetFilePath);
//            assetDetailConfig.InitConfig();

//            loader = new DatabaseLoader();
//            loader.DoInitialize(assetDetailConfig, (result) =>
//            {
//                Debug.Log("LoaderUtill::Init->init finished result = " + result);
//                finishedCallback?.Invoke(result);
//            });
//#endif
            loaderBehaviour = PersistentUObjectHelper.CreateComponent<LoaderBehaviour>();
            loaderBehaviour.SetLoader(loader);
        }

        public static void SetOperationMaxCount(int maxCount)
        {
            if (loader != null)
            {
                loader.OperationMaxCount = maxCount <= 0 ? 10 : maxCount;
            }
        }

        public static UnityObject LoadAssetSync(string address)
        {
            return loader?.LoadAssetSync(address);
        }
        public static UnityObject InstanceAssetSync(string address)
        {
            return loader?.InstanceAssetSync(address);
        }
        public static UnityObject[] LoadAssetsSync(string[] addresses)
        {
            return loader?.LoadAssetsSync(addresses);
        }
        public static UnityObject[] InstanceAssetsSync(string[] addresses)
        {
            return loader?.InstanceAssetsSync(addresses);
        }
        public static UnityObject[] LoadAssetsSyncByLabel(string label)
        {
            return loader?.LoadAssetsSyncByLabel(label);
        }
        public static UnityObject[] InstanceAssetsSyncByLabel(string label)
        {
            return loader?.InstanceAssetsSyncByLabel(label);
        }

        public static int LoadAssetAsync(
            string address,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            AsyncPriority priority = AsyncPriority.Default,
            SystemObject userdata = null)
        {
            if (loader != null)
            {
                return loader.LoadAssetAsync(address, progressCallback, completeCallback, priority, userdata);
            }
            return -1;
        }
        public static int InstanceAssetAsync(
            string address,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            AsyncPriority priority = AsyncPriority.Default,
            SystemObject userdata = null)
        {
            if (loader != null)
            {
                return loader.InstanceAssetAsync(address, progressCallback, completeCallback, priority, userdata);
            }
            return -1;
        }
        public static int LoadAssetsAsync(
            string[] addresses,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            OnAssetsProgress progressesCallback,
            OnAssetsComplete completesCallback,
            AsyncPriority priority = AsyncPriority.Default,
            SystemObject userdata = null)
        {
            if (loader != null)
            {
                return loader.LoadAssetsAsync(addresses, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
            }
            return -1;
        }
        public static int InstanceAssetsAsync(
            string[] addresses,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            OnAssetsProgress progressesCallback,
            OnAssetsComplete completesCallback,
            AsyncPriority priority = AsyncPriority.Default,
            SystemObject userdata = null)
        {
            if (loader != null)
            {
                return loader.InstanceAssetsAsync(addresses, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
            }
            return -1;
        }
        public static int LoadAssetsAsyncByLabel(
            string label,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            OnAssetsProgress progressesCallback,
            OnAssetsComplete completesCallback,
            AsyncPriority priority = AsyncPriority.Default,
            SystemObject userdata = null)
        {
            if (loader != null)
            {
                return loader.LoadAssetsAsyncByLabel(label, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
            }
            return -1;
        }
        public static int InstanceAssetsAsyncByLabel(
            string label,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            OnAssetsProgress progressesCallback,
            OnAssetsComplete completesCallback,
            AsyncPriority priority = AsyncPriority.Default,
            SystemObject userdata = null)
        {
            if (loader != null)
            {
                return loader.InstanceAssetsAsyncByLabel(label, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
            }
            return -1;
        }

        public static void CancelAssetsAsync(int index)
        {
            if (loader != null)
            {
                loader.CancelAssetsAsync(index);
            }
        }

        public static UnityObject InstanceUObject(string address, UnityObject uObject)
        {
            if (loader != null)
            {
                return loader.InstanceUObject(address, uObject);
            }
            return null;
        }

        public static void DestroyUObject(string address, UnityObject uObject)
        {
            if (loader != null)
            {
                loader.DestroyUObject(address, uObject);
            }
        }

        public static void UnloadUnusedAssets()
        {
            if (loader != null)
            {
                loader.UnloadUnusedAssets();
            }
        }
        public static void UnloadAssets(OnUnloadFinished finishedCallback)
        {
            if (loader != null)
            {
                loader.UnloadAssets(finishedCallback);
            }
        }

        public static void DestroyLoader()
        {
            if (loader != null)
            {
                loader.DoDestroy();
                loader = null;
            }
            if (loaderBehaviour != null)
            {
                GameObject.Destroy(loaderBehaviour.gameObject);
                loaderBehaviour = null;
            }
        }
    }
}
