using DotEngine.Generic;
using DotEngine.Pool;
using Priority_Queue;
using System;
using System.Collections.Generic;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public enum NodeState
    {
        None = 0,
        Loading,
        Loaded,
        LoadError,
    }

    public abstract class ALoader : ILoader
    {
        private ItemPool<AsyncRequest> asyncRequestPool = new ItemPool<AsyncRequest>();
        protected ItemPool<AssetNode> assetNodePool = new ItemPool<AssetNode>();
        private UniqueIntID uniqueIDCreator = new UniqueIntID();

        public int AsyncOperationMaxCount { get; set; } = 10;
        public LoaderState State { get; protected set; } = LoaderState.None;

        protected AssetDetailConfig assetDetailConfig = null;
        protected OnInitFinished initializedCallback = null;

        protected Dictionary<int, AsyncRequest> asyncRequestDic = new Dictionary<int, AsyncRequest>();
        protected SimplePriorityQueue<AsyncRequest, AsyncPriority> waitingAsyncRequestQueue = new SimplePriorityQueue<AsyncRequest, AsyncPriority>();
        protected List<AsyncRequest> runningRequestList = new List<AsyncRequest>();

        protected Dictionary<string, AssetNode> assetNodeDic = new Dictionary<string, AssetNode>();

        private AsyncOperation unloadUnusedOperation = null;
        protected OnUnloadUnusedFinished unloadUnusedCallback = null;

        #region Initialize
        public void DoInitialize(AssetDetailConfig detailConfig, OnInitFinished initCallback, params SystemObject[] values)
        {
            State = LoaderState.Initializing;

            assetDetailConfig = detailConfig;
            initializedCallback = initCallback;

            OnInitialize(values);
        }
        protected abstract void OnInitialize(params SystemObject[] values);
        protected abstract bool OnInitializeUpdate(float deltaTime);

        #endregion

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

        #region instance or destroy 
        public UnityObject InstanceUObject(string address, UnityObject uObject)
        {
            string path = assetDetailConfig.GetPathByAddress(address);
            if (assetNodeDic.TryGetValue(path, out var node))
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
            }
            else
            {
                UnityObject.Destroy(uObject);
            }
        }

        #endregion

        #region unload unused
        public void UnloadUnusedAssets(OnUnloadUnusedFinished unloadCallback)
        {
            if (unloadUnusedCallback != null)
            {
                Debug.LogError("");
                return;
            }
            if (unloadUnusedOperation != null)
            {
                return;
            }

            unloadUnusedCallback = unloadCallback;

            GC.Collect();
            GC.Collect();
            unloadUnusedOperation = Resources.UnloadUnusedAssets();

            OnUnloadUnusedAssets();
        }

        protected abstract void OnUnloadUnusedAssets();
        protected abstract bool OnUnloadUnusedAssetsUpdate();

        #endregion

        public void DoUdpate(float deltaTime, float unscaleDeltaTime)
        {
            if (State == LoaderState.Initializing)
            {
                if (OnInitializeUpdate(deltaTime))
                {
                    if (State == LoaderState.Initialized)
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

            if (State != LoaderState.Running)
            {
                return;
            }

            for (int i = runningRequestList.Count - 1; i > 0; --i)
            {
                AsyncRequest request = runningRequestList[i];
                UpdateRequest(request);

                if (request.state == AsyncState.LoadFinished)
                {
                    if (request.isInstance)
                    {
                        request.state = AsyncState.WaitingForInstance;
                    }
                    else
                    {
                        EndRequest(request);
                        runningRequestList.RemoveAt(i);
                        asyncRequestPool.Release(request);
                    }
                }
                else if (request.state == AsyncState.InstanceFinished)
                {
                    EndRequest(request);
                    runningRequestList.RemoveAt(i);
                    asyncRequestPool.Release(request);
                }
                else if (request.state == AsyncState.WaitingForInstance)
                {
                    request.state = AsyncState.Instancing;
                }
            }

            while (waitingAsyncRequestQueue.Count > 0 && CanStartRequest())
            {
                AsyncRequest request = waitingAsyncRequestQueue.Dequeue();
                request.state = AsyncState.Loading;
                StartRequest(request);
                runningRequestList.Add(request);
            }

            if (unloadUnusedOperation != null)
            {
                if (unloadUnusedOperation.isDone)
                {
                    if (OnUnloadUnusedAssetsUpdate())
                    {
                        unloadUnusedCallback.Invoke();
                        unloadUnusedCallback = null;

                        unloadUnusedOperation = null;
                    }
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
            result.DoInitialize(id, addresses);

            request.result = result;

            asyncRequestDic.Add(id, request);
            waitingAsyncRequestQueue.Enqueue(request, request.priority);

            return result;
        }

        private AssetNode GetAssetNode(string assetPath)
        {
            if(assetNodeDic.TryGetValue(assetPath,out var node))
            {
                if(node.IsAssetValid())
                {
                    return node;
                }else
                {
                    throw new Exception();
                }
            }

            node = assetNodePool.Get();
            node.DoInitialize(assetPath);
            UnityObject uObject = LoadAsset(assetPath);
            node.SetAsset(uObject);

            assetNodeDic.Add(assetPath, node);

            return node;
        }

        protected abstract UnityObject LoadAsset(string assetPath);

        protected abstract bool CanStartRequest();
        protected abstract void StartRequest(AsyncRequest request);
        protected abstract void UpdateRequest(AsyncRequest request);
        protected abstract void EndRequest(AsyncRequest request);
    }
}
