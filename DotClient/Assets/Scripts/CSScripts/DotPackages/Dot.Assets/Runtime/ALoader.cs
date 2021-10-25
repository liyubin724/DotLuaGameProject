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
        private ItemPool<AsyncRequest> requestPool = new ItemPool<AsyncRequest>();
        protected ItemPool<AssetNode> assetNodePool = new ItemPool<AssetNode>();
        private UniqueIntID uniqueIDCreator = new UniqueIntID();

        public int OperationMaxCount { get; set; } = 10;
        public LoaderState State { get; protected set; } = LoaderState.None;

        protected AssetDetailConfig assetDetailConfig = null;
        protected OnInitFinished initializedCallback = null;

        protected Dictionary<int, AsyncRequest> requestDic = new Dictionary<int, AsyncRequest>();
        protected SimplePriorityQueue<AsyncRequest, AsyncPriority> waitingRequestQueue = new SimplePriorityQueue<AsyncRequest, AsyncPriority>();
        protected List<AsyncRequest> runningRequestList = new List<AsyncRequest>();

        protected int operationCount = 0;
        protected ListDictionary<string, AAsyncOperation> operationLDic = new ListDictionary<string, AAsyncOperation>();
        protected Dictionary<string, AssetNode> assetNodeDic = new Dictionary<string, AssetNode>();

        private AsyncOperation unloadOperation = null;
        private List<OnUnloadFinished> unloadFinishedCallbacks = new List<OnUnloadFinished>();

        #region Initialize
        public virtual void DoInitialize(AssetDetailConfig detailConfig, OnInitFinished initCallback, params SystemObject[] values)
        {
            State = LoaderState.Initializing;

            assetDetailConfig = detailConfig;
            initializedCallback = initCallback;
        }

        protected abstract bool OnInitializeUpdate(float deltaTime);

        #endregion

        #region Load Asset Sync
        private UnityObject LoadAssetSync(string address)
        {
            string assetPath = assetDetailConfig.GetPathByAddress(address);
            AssetNode assetNode = GetAssetNodeSync(assetPath);

            return assetNode.GetAsset();
        }

        private UnityObject InstanceAssetSync(string address)
        {
            string assetPath = assetDetailConfig.GetPathByAddress(address);
            AssetNode assetNode = GetAssetNodeSync(assetPath);

            return assetNode.CreateInstance();
        }

        public UnityObject[] LoadAssetsSync(string[] addresses)
        {
            UnityObject[] results = new UnityObject[addresses.Length];
            for (int i = 0; i < addresses.Length; i++)
            {
                results[i] = LoadAssetSync(addresses[i]);
            }
            return results;
        }

        public UnityObject[] InstanceAssetsSync(string[] addresses)
        {
            UnityObject[] results = new UnityObject[addresses.Length];
            for (int i = 0; i < addresses.Length; i++)
            {
                results[i] = InstanceAssetSync(addresses[i]);
            }
            return results;
        }
        #endregion

        #region Load Asset Async
        public AsyncResult LoadAssetsAsync(
            string[] addresses,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string[] paths = assetDetailConfig.GetPathsByAddresses(addresses);
            return RequestAssetsAsync(addresses, paths, false, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
        }

        public AsyncResult InstanceAssetsAsync(
            string[] addresses,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string[] paths = assetDetailConfig.GetPathsByAddresses(addresses);
            return RequestAssetsAsync(addresses, paths, false, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
        }

        public void CancelAssetsAsync(AsyncResult result)
        {
            if(result.IsDone())
            {
                return;
            }
            if(requestDic.TryGetValue(result.ID,out var request))
            {
                if(waitingRequestQueue.Contains(request))
                {
                    waitingRequestQueue.Remove(request);
                }else if(runningRequestList.Contains(request))
                {
                    runningRequestList.Remove(request);
                    foreach (var assetPath in request.paths)
                    {
                        if (assetNodeDic.TryGetValue(assetPath, out var assetNode))
                        {
                            assetNode.ReleaseRef();
                        }else
                        {
                            Debug.LogError("");
                        }
                    }
                    OnAsyncRequestCancel(request);
                }

                requestDic.Remove(request.id);
            }
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
        public void UnloadUnusedAssets()
        {
            var keys = assetNodeDic.Keys;
            foreach(var key in keys)
            {
                AssetNode node = assetNodeDic[key];
                if(!node.IsInUsing())
                {
                    OnReleaseAssetNode(node);
                    assetNodeDic.Remove(key);
                    assetNodePool.Release(node);
                }
            }
        }

        public void UnloadAssets(OnUnloadFinished finishedCallback)
        {
            unloadFinishedCallbacks.Add(finishedCallback);
            if(unloadOperation!=null)
            {
                return;
            }

            GC.Collect();
            GC.Collect();
            unloadOperation = Resources.UnloadUnusedAssets();

            UnloadUnusedAssets();
        }

        protected abstract bool OnUnloadAssetsUpdate();

        #endregion

        public void DoUdpate(float deltaTime, float unscaleDeltaTime)
        {
            if (State == LoaderState.Initializing)
            {
                if (OnInitializeUpdate(deltaTime))
                {
                    if (State == LoaderState.Initialized)
                    {
                        initializedCallback?.Invoke(true);
                        State = LoaderState.Running;
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

            if (operationLDic.Count > 0 && operationCount < OperationMaxCount)
            {
                int diffCount = OperationMaxCount - operationCount;
                for (int i = 0; i < operationLDic.Count; i++)
                {
                    if (!operationLDic[i].IsRunning)
                    {
                        operationLDic[i].DoStart();
                        diffCount--;
                    }
                    if (diffCount <= 0)
                    {
                        break;
                    }
                }
            }

            for (int i = runningRequestList.Count - 1; i > 0; --i)
            {
                AsyncRequest request = runningRequestList[i];
                OnAsyncRequestUpdate(request);

                if (request.state == AsyncState.LoadFinished)
                {
                    if (request.isInstance)
                    {
                        request.state = AsyncState.WaitingForInstance;
                    }
                    else
                    {
                        OnAsyncRequestEnd(request);
                        requestDic.Remove(request.id);
                        runningRequestList.RemoveAt(i);
                        requestPool.Release(request);
                    }
                }
                else if (request.state == AsyncState.InstanceFinished)
                {
                    OnAsyncRequestEnd(request);
                    requestDic.Remove(request.id);
                    runningRequestList.RemoveAt(i);
                    requestPool.Release(request);
                }
                else if (request.state == AsyncState.WaitingForInstance)
                {
                    request.state = AsyncState.Instancing;
                }
            }

            while (waitingRequestQueue.Count > 0 && CanStartRequest())
            {
                AsyncRequest request = waitingRequestQueue.Dequeue();
                request.state = AsyncState.Loading;

                string[] assetPaths = request.paths;
                for (int i = 0; i < assetPaths.Length; i++)
                {
                    string assetPath = assetPaths[i];
                    AssetNode assetNode = GetAssetNodeAsync(assetPath);
                    assetNode.RetainRef();
                }

                OnAsyncRequestStart(request);

                runningRequestList.Add(request);
            }

            if (unloadOperation != null)
            {
                if (unloadOperation.isDone)
                {
                    if (OnUnloadAssetsUpdate())
                    {
                        unloadFinishedCallbacks.Clear();
                        unloadOperation = null;
                    }
                }
            }
        }

        public void DoDestroy()
        {

        }

        #region request asset
        protected abstract UnityObject RequestAssetSync(string assetPath);

        private AsyncResult RequestAssetsAsync(
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
            AsyncRequest request = requestPool.Get();
            request.id = id;
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

            requestDic.Add(id, request);
            waitingRequestQueue.Enqueue(request, request.priority);

            return result;
        }
        #endregion

        #region get asset node
        private AssetNode GetAssetNodeSync(string assetPath)
        {
            if (assetNodeDic.TryGetValue(assetPath, out var node))
            {
                if (node.IsLoaded())
                {
                    return node;
                }
                else
                {
                    if(operationLDic.TryGetValue(assetPath,out var operation))
                    {
                        throw new Exception();
                    }else
                    {
                        node.SetAsset(RequestAssetSync(assetPath));
                        return node;
                    }
                }
            }
            node = assetNodePool.Get();
            node.DoInitialize(assetPath);
            node.SetAsset(RequestAssetSync(assetPath));

            OnCreateAssetNodeSync(node);

            assetNodeDic.Add(assetPath, node);
            return node;
        }
        private AssetNode GetAssetNodeAsync(string assetPath)
        {
            if (assetNodeDic.TryGetValue(assetPath, out var node))
            {
                return node;
            }

            node = assetNodePool.Get();
            node.DoInitialize(assetPath);
            node.State = NodeState.Loading;

            OnCreateAssetNodeAsync(node);

            assetNodeDic.Add(assetPath, node);

            return node;
        }
        protected abstract void OnCreateAssetNodeSync(AssetNode assetNode);
        protected abstract void OnCreateAssetNodeAsync(AssetNode assetNode);
        protected abstract void OnReleaseAssetNode(AssetNode assetNode);
        #endregion

        protected abstract bool CanStartRequest();
        protected abstract void OnAsyncRequestStart(AsyncRequest request);
        protected abstract void OnAsyncRequestUpdate(AsyncRequest request);
        protected abstract void OnAsyncRequestEnd(AsyncRequest request);
        protected abstract void OnAsyncRequestCancel(AsyncRequest request);

    }
}
