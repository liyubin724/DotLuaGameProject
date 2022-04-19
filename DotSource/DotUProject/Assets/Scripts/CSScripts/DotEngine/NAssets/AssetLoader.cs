using DotEngine.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.NAssets
{
    public class AssetLoader
    {


        private IntIDCreator m_IdCreator = new IntIDCreator(0);
        protected AssetConfig assetConfig = null;

        public UnityObject LoadAssetSync(string address)
        {
            return null;
        }

        public UnityObject InstanceAssetSync(string address)
        {
            return null;
        }

        public UnityObject[] LoadAssetBatchSync(string[] addresses)
        {
            return null;
        }

        public UnityObject[] InstanceAssetBatchSync(string[] addresses)
        {
            return null;
        }

        public int LoadAssetAsync(
            string address,
            AssetAsyncProgress onProgress,
            AssetAsyncComplete onComplete,
            AssetAsyncPriority priority,
            SystemObject userdata)
        {
            return 0;
        }

        public int InstanceAssetAsync(
            string address,
            AssetAsyncProgress onProgress,
            AssetAsyncComplete onComplete,
            AssetAsyncPriority priority,
            SystemObject userdata)
        {
            return 0;
        }

        public int LoadAssetBatchAsync(
            string[] addresses,
            AssetAsyncProgress onProgress,
            AssetAsyncComplete onComplete,
            AssetBatchAsyncProgress onBatchProgress,
            AssetBatchAsyncComplete onBatchComplete,
            AssetAsyncPriority priority,
            SystemObject userdata)
        {
            return 0;
        }

        public int InstanceAssetBatchAsync(
            string[] addresses,
            AssetAsyncProgress onProgress,
            AssetAsyncComplete onComplete,
            AssetBatchAsyncProgress onBatchProgress,
            AssetBatchAsyncComplete onBatchComplete,
            AssetAsyncPriority priority,
            SystemObject userdata)
        {
            return 0;
        }

        public void UnloadAsset(string address)
        {

        }

        public void CancelAssetAsync(int requestIndex)
        {

        }

        public UnityObject InstanceUObject(string address, UnityObject uObject)
        {
            return null;
        }

        public void DestroyUObject(string address, UnityObject uObject)
        {

        }

        public void UnloadUnusedAssets()
        {

        }
    }
}
