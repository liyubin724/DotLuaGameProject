using DotEngine.Generic;
using DotEngine.Pool;
using Priority_Queue;
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
        private AssetConfig m_AssetConfig = null;
        private ObjectPool<AssetAsyncRequest> m_RequestPool = null;
        private ObjectPool<AssetLoaderData> m_DataPool = null;

        public int MaxAsyncCount { get; set; } = 10;

        private SimplePriorityQueue<AssetLoaderData, AssetAsyncPriority> m_WaitingRequestQueue = new SimplePriorityQueue<AssetLoaderData, AssetAsyncPriority>();
        private List<AssetLoaderData> m_RunningRequestList = new List<AssetLoaderData>();

        public AssetLoader()
        {
            m_RequestPool = new ObjectPool<AssetAsyncRequest>(
                () =>
                {
                    return new AssetAsyncRequest();
                }, 
                null, 
                (request) =>
                {
                    request.DoRelease();
                });
            m_DataPool = new ObjectPool<AssetLoaderData>(
                () =>
                {
                    return new AssetLoaderData();
                },
                null,
                (data) =>
                {
                    data.DoRelease();
                });
        }

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
            return RequestAssetAsync(new string[] { address }, false, onProgress, onComplete, null, null, priority, userdata);
        }

        public int InstanceAssetAsync(
            string address,
            AssetAsyncProgress onProgress,
            AssetAsyncComplete onComplete,
            AssetAsyncPriority priority,
            SystemObject userdata)
        {
            return RequestAssetAsync(new string[] { address }, true, onProgress, onComplete, null, null, priority, userdata);
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
            return RequestAssetAsync(addresses, false, onProgress, onComplete, onBatchProgress, onBatchComplete, priority, userdata);
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
            return RequestAssetAsync(addresses, true, onProgress, onComplete, onBatchProgress, onBatchComplete, priority, userdata);
        }

        private int RequestAssetAsync(
            string[] addresses,
            bool isInstance,
            AssetAsyncProgress onProgress,
            AssetAsyncComplete onComplete,
            AssetBatchAsyncProgress onBatchProgress,
            AssetBatchAsyncComplete onBatchComplete,
            AssetAsyncPriority priority,
            SystemObject userdata)
        {
            int id = m_IdCreator.GetNextID();

            var data = m_DataPool.Get();

            var request = m_RequestPool.Get();
            request.Id = id;
            request.Addresses = addresses;
            request.Paths = new string[addresses.Length];
            for(int i =0;i<addresses.Length;i++)
            {
                request.Paths[i] = m_AssetConfig.GetPathByAddress(addresses[i]);
            }
            request.IsInstance = isInstance;
            request.OnProgress = onProgress;
            request.OnComplete = onComplete;
            request.OnBatchProgress = onBatchProgress;
            request.OnBatchComplete = onBatchComplete;
            request.Priority = priority;
            request.Userdata = userdata;

            data.Request = request;

            var result = new AssetAsyncResult(id,addresses);
            data.Result = result;

            m_WaitingRequestQueue.Enqueue(data,priority);
            return id;
        }

        public void UnloadAsset(string address)
        {

        }

        public void CancelAssetAsync(int id)
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

        private AssetNode GetAssetNodeSync(string path)
        {

        }
    }
}
