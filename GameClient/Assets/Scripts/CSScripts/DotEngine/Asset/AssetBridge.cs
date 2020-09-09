using DotEngine.Generic;
using DotEngine.Pool;
using System.Collections.Generic;
using System.Linq;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Asset
{
    public delegate void OnAssetBridgeLoadComplete(AssetHandler handler);

    public class AssetBridge //: IDispose
    {
        private static ObjectPool<AssetBridgeData> bridgeDataPool = new ObjectPool<AssetBridgeData>();

        private UniqueIntID idCreator = new UniqueIntID(0);
        private AssetLoaderPriority loaderPriority = AssetLoaderPriority.Default;
        private Dictionary<int, AssetBridgeData> bridgeDataDic = new Dictionary<int, AssetBridgeData>();

        private AssetService assetService = null;
        public AssetBridge() : this(AssetLoaderPriority.Default)
        {
            assetService = Facade.GetInstance().GetServicer<AssetService>(AssetService.NAME);
        }

        public AssetBridge(AssetLoaderPriority priority)
        {
            loaderPriority = priority;
        }

        public int LoadAsset(string address, OnAssetBridgeLoadComplete complete, SystemObject userData = null)
        {
            AssetBridgeData bridgeData = bridgeDataPool.Get();
            bridgeData.uniqueID = idCreator.GetNextID();
            bridgeData.complete = complete;
            bridgeData.userData = userData;

            AssetHandler handler = assetService.LoadAssetAsync(address, null, OnAssetComplete, loaderPriority, bridgeData.uniqueID);

            bridgeData.handler = handler;
            bridgeDataDic.Add(bridgeData.uniqueID, bridgeData);

            return bridgeData.uniqueID;
        }

        public int InstanceAsset(string address, OnAssetBridgeLoadComplete complete, SystemObject userData = null)
        {
            AssetBridgeData bridgeData = bridgeDataPool.Get();
            bridgeData.uniqueID = idCreator.GetNextID();
            bridgeData.complete = complete;
            bridgeData.userData = userData;

            AssetHandler handler = assetService.InstanceAssetAsync(address, null, OnAssetComplete, loaderPriority, bridgeData.uniqueID);

            bridgeData.handler = handler;
            bridgeDataDic.Add(bridgeData.uniqueID, bridgeData);

            return bridgeData.uniqueID;
        }

        public int LoadAsset(string[] addresses, OnAssetBridgeLoadComplete complete, SystemObject userData = null)
        {
            AssetBridgeData bridgeData = bridgeDataPool.Get();
            bridgeData.uniqueID = idCreator.GetNextID();
            bridgeData.complete = complete;
            bridgeData.userData = userData;

            AssetHandler handler = assetService.LoadBatchAssetAsync(addresses, null, null,null,OnBatchAssetComplete,loaderPriority, bridgeData.uniqueID);

            bridgeData.handler = handler;
            bridgeDataDic.Add(bridgeData.uniqueID, bridgeData);

            return bridgeData.uniqueID;
        }

        public int InstanceAsset(string[] addresses, OnAssetBridgeLoadComplete complete, SystemObject userData = null)
        {
            AssetBridgeData bridgeData = bridgeDataPool.Get();
            bridgeData.uniqueID = idCreator.GetNextID();
            bridgeData.complete = complete;
            bridgeData.userData = userData;

            AssetHandler handler = assetService.InstanceBatchAssetAsync(addresses, null, null, null, OnBatchAssetComplete, loaderPriority, bridgeData.uniqueID);

            bridgeData.handler = handler;
            bridgeDataDic.Add(bridgeData.uniqueID, bridgeData);

            return bridgeData.uniqueID;
        }

        public void OnAssetComplete(string address, UnityObject uObj, SystemObject userData)
        {
            InvokeAssetCompleteCallback((int)userData);
        }

        public void OnBatchAssetComplete(string[] addresses, UnityObject[] uObjs, SystemObject userData)
        {
            InvokeAssetCompleteCallback((int)userData);
        }

        private void InvokeAssetCompleteCallback(int uniqueID)
        {
            if (bridgeDataDic.TryGetValue(uniqueID, out AssetBridgeData bridgeData))
            {
                bridgeDataDic.Remove(uniqueID);
                bridgeData.complete?.Invoke(bridgeData.handler);

                bridgeDataPool.Release(bridgeData);
            }
        }

        public void CancelLoad(int uniqueID)
        {
            if(bridgeDataDic.TryGetValue(uniqueID,out AssetBridgeData bridgeData))
            {
                bridgeDataDic.Remove(uniqueID);
                assetService.UnloadAssetAsync(bridgeData.handler, true);
                bridgeDataPool.Release(bridgeData);
            }
        }

        public void Dispose()
        {
            int[] ids = bridgeDataDic.Keys.ToArray();
            foreach(var id in ids)
            {
                CancelLoad(id);
            }
        }

         class AssetBridgeData : IObjectPoolItem
         {
            public int uniqueID = -1;
            public AssetHandler handler = null;
            public OnAssetBridgeLoadComplete complete;
            public SystemObject userData = null;

            public void OnGet()
            {
            }

            public void OnNew()
            {
            }

            public void OnRelease()
            {
                uniqueID = -1;
                handler = null;
                complete = null;
                userData = null;
            }
        }
    }


}
