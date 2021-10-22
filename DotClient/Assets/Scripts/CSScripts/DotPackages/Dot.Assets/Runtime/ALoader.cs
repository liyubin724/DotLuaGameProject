using DotEngine.Generic;
using DotEngine.Pool;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public abstract class ALoader : ILoader
    {
        private ItemPool<AsyncRequest> asyncRequestPool = new ItemPool<AsyncRequest>();
        private ItemPool<AssetNode> assetNodePool = new ItemPool<AssetNode>();
        private UniqueIntID uniqueIDCreator = new UniqueIntID();

        public LoaderState State { get; protected set; } = LoaderState.None;
        public int AsyncOperationMaxCount { get; set; } = 10;
        public int InstanceMaxCount { get; set; } = 10;

        protected AssetDetailConfig assetDetailConfig = null;
        protected OnInitFinished initializedCallback = null;

        protected Dictionary<int, AsyncRequest> asyncRequestDic = new Dictionary<int, AsyncRequest>();
        protected SimplePriorityQueue<AsyncRequest, AsyncPriority> waitingAsyncRequestQueue = new SimplePriorityQueue<AsyncRequest, AsyncPriority>();
        protected List<AsyncRequest> runningRequestList = new List<AsyncRequest>();

        protected List<AOperation> assetAsyncOperations = new List<AOperation>();
        protected Dictionary<string, AssetNode> assetNodeDic = new Dictionary<string, AssetNode>();

        protected OnUnloadUnusedFinished unloadUnusedCallback = null;

        public void DoInitialize(AssetDetailConfig detailConfig, OnInitFinished initCallback, params SystemObject[] values)
        {
            State = LoaderState.Initializing;

            assetDetailConfig = detailConfig;
            initializedCallback = initCallback;

            OnInitialize(values);
        }

        #region Load Asset
        public UnityObject LoadAssetByAddress(string address)
        {
            string assetPath = assetDetailConfig.GetPathByAddress(address);
            AssetNode assetNode = GetAssetNode(assetPath);

            return assetNode.GetAsset();
        }

        public UnityObject InstanceAssetByAddress(string address)
        {
            string assetPath = assetDetailConfig.GetPathByAddress(address);
            AssetNode assetNode = GetAssetNode(assetPath);

            return assetNode.CreateInstance();
        }

        public UnityObject[] LoadAssetsByAddress(string[] addresses)
        {
            UnityObject[] results = new UnityObject[addresses.Length];
            for (int i = 0; i < addresses.Length; i++)
            {
                results[i] = LoadAssetByAddress(addresses[i]);
            }
            return results;
        }

        public UnityObject[] InstanceAssetsByAdress(string[] addresses)
        {
            UnityObject[] results = new UnityObject[addresses.Length];
            for (int i = 0; i < addresses.Length; i++)
            {
                results[i] = InstanceAssetByAddress(addresses[i]);
            }
            return results;
        }

        public UnityObject[] LoadAssetsByLabel(string label)
        {
            string[] addresses = assetDetailConfig.GetAddressesByLabel(label);
            return LoadAssetsByAddress(addresses);
        }

        public UnityObject[] InstanceAssetsByLabel(string label)
        {
            string[] addresses = assetDetailConfig.GetAddressesByLabel(label);
            return InstanceAssetsByAdress(addresses);
        }

        #endregion

        #region Load Asset Async
        public AsyncResult LoadAssetAsyncByAddress(
            string address,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string[] addresses = new string[] { address };
            string[] paths = new string[] { assetDetailConfig.GetPathByAddress(address) };

            return RequestAssetsAsync(null, addresses, paths, false, progressCallback, completeCallback, null, null, priority, userdata);
        }

        public AsyncResult InstanceAssetAsyncByAddress(
            string address,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string[] addresses = new string[] { address };
            string[] paths = new string[] { assetDetailConfig.GetPathByAddress(address) };

            return RequestAssetsAsync(null, addresses, paths, true, progressCallback, completeCallback, null, null, priority, userdata);
        }

        public AsyncResult LoadAssetsAsyncByAddress(
            string[] addresses,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string[] paths = assetDetailConfig.GetPathsByAddresses(addresses);
            return RequestAssetsAsync(null, addresses, paths, false, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
        }

        public AsyncResult InstanceAssetsAsyncByAdress(
            string[] addresses,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string[] paths = assetDetailConfig.GetPathsByAddresses(addresses);
            return RequestAssetsAsync(null, addresses, paths, false, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
        }

        public AsyncResult LoadAssetsAsyncByLabel(
            string label,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string[] addresses = assetDetailConfig.GetAddressesByLabel(label);
            string[] paths = assetDetailConfig.GetPathsByAddresses(addresses);
            return RequestAssetsAsync(label, addresses, paths, false, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
        }

        public AsyncResult InstanceAssetsAsyncByLabel(
            string label,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string[] addresses = assetDetailConfig.GetAddressesByLabel(label);
            string[] paths = assetDetailConfig.GetPathsByAddresses(addresses);
            return RequestAssetsAsync(label, addresses, paths, true, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
        }

        #endregion

        public UnityObject InstanceUObject(string address, UnityObject uObject)
        {
            string path = assetDetailConfig.GetPathByAddress(address);
            if(assetNodeDic.TryGetValue(path,out var node))
            {
                return node.CreateInstance();
            }
            return null;
        }

        public void DestroyUObject(string address, UnityObject uObject)
        {
            string path = assetDetailConfig.GetPathByAddress(address);
            if (assetNodeDic.TryGetValue(path, out var node))
            {
                node.DestroyInstance(uObject);
                if(!node.IsInUnused())
                {
                    assetNodeDic.Remove(path);
                    OnDestroyAssetNode(node);
                    assetNodePool.Release(node);
                }
            }else
            {
                UnityObject.Destroy(uObject);
            }
        }

        public void UnloadUnusedAssets(OnUnloadUnusedFinished unloadCallback)
        {
            if(unloadUnusedCallback !=null)
            {
                Debug.LogError("");
                return;
            }

            unloadUnusedCallback = unloadCallback;

            GC.Collect();
            GC.Collect();
            Resources.UnloadUnusedAssets();

            OnUnloadUnusedAssets();
        }

        public void DoUdpate(float deltaTime,float unscaleDeltaTime)
        {
            if(State == LoaderState.Initializing)
            {
                if(OnInitializeUpdate(deltaTime))
                {
                    if(State == LoaderState.Initialized)
                    {
                        State = LoaderState.Running;
                        initializedCallback?.Invoke(true);
                    }
                    else
                    {
                        initializedCallback?.Invoke(false);
                    }
                }
                return;
            }

            if(State != LoaderState.Running)
            {
                return;
            }

            for (int i = 0; i < assetAsyncOperations.Count; )
            {
                var operation = assetAsyncOperations[i];
                if (operation.IsFinished)
                {

                }
                else
                {
                    ++i;
                }
            }

            if (unloadUnusedCallback!=null)
            {
                if(OnUnloadUnusedAssetsUpdate())
                {
                    unloadUnusedCallback.Invoke();
                    unloadUnusedCallback = null;
                }
            }
        }

        public void DoDestroy()
        {

        }

        private AsyncResult RequestAssetsAsync(
            string label,
            string[] addresses,
            string[] paths,
            bool isInstance,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            int id = uniqueIDCreator.GetNextID();
            AsyncRequest request = asyncRequestPool.Get();
            request.id = id;
            request.label = label;
            request.addresses = addresses;
            request.paths = paths;
            request.isInstance = isInstance;
            request.progressCallback = progressCallback;
            request.completeCallback = completeCallback;
            request.progressesCallback = progressesCallback;
            request.completesCallback = completesCallback;
            request.priority = priority;
            request.userdata = userdata;
            request.state = AsyncState.WaitingForStart;

            AsyncResult result = new AsyncResult();
            result.id = id;
            result.SetAddress(addresses);

            request.result = result;

            asyncRequestDic.Add(id, request);
            waitingAsyncRequestQueue.Enqueue(request, request.priority);

            return result;
        }

        private AssetNode GetAssetNode(string assetPath)
        {
            if (assetNodeDic.TryGetValue(assetPath, out var assetNode))
            {
                if (assetNode != null && assetNode.IsAssetValid())
                {
                    return assetNode;
                }
            }

            UnityObject uObject = LoadAsset(assetPath);
            if (assetNode == null)
            {
                assetNode = new AssetNode
                {
                    Path = assetPath
                };

                assetNodeDic.Add(assetPath, assetNode);
            }
            assetNode.SetAsset(uObject);

            return assetNode;
        }

        protected abstract void OnInitialize(params SystemObject[] values);
        protected abstract bool OnInitializeUpdate(float deltaTime);

        protected abstract UnityObject LoadAsset(string assetPath);

        protected abstract void OnUnloadUnusedAssets();
        protected abstract bool OnUnloadUnusedAssetsUpdate();

        protected abstract void OnCreateAssetNode(AssetNode node);
        protected abstract void OnDestroyAssetNode(AssetNode node);

        protected abstract AOperation GetAssetLoadOperation(string assetPath);

        protected abstract bool TryToStartReqeust(AsyncRequest request);

    }
}
