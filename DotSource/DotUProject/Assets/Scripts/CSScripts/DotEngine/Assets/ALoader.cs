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
        private ElementPool<AsyncResult> resultPool = new ElementPool<AsyncResult>();
        private ElementPool<AsyncRequest> requestPool = new ElementPool<AsyncRequest>();
        protected ElementPool<AssetNode> assetNodePool = new ElementPool<AssetNode>();
        private IntIDCreator uniqueIDCreator = new IntIDCreator();

        public int OperationMaxCount { get; set; } = 10;
        public LoaderState State { get; protected set; } = LoaderState.None;

        protected AssetDetailConfig assetDetailConfig = null;
        protected OnInitFinished initializedCallback = null;

        private Dictionary<int, AsyncResult> resultDic = new Dictionary<int, AsyncResult>();

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
        public UnityObject LoadAssetSync(string address)
        {
            string assetPath = assetDetailConfig.GetPathByAddress(address);
            AssetNode assetNode = GetAssetNodeSync(assetPath);

            return assetNode.GetAsset();
        }

        public UnityObject InstanceAssetSync(string address)
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

        public UnityObject[] LoadAssetsSyncByLabel(string label)
        {
            string[] addresses = assetDetailConfig.GetAddressesByLabel(label);
            if(addresses == null || addresses.Length == 0)
            {
                Debug.LogWarning("");
                return new UnityObject[0];
            }
            return LoadAssetsSync(addresses);
        }

        public UnityObject[] InstanceAssetsSyncByLabel(string label)
        {
            string[] addresses = assetDetailConfig.GetAddressesByLabel(label);
            if (addresses == null || addresses.Length == 0)
            {
                Debug.LogWarning("");
                return new UnityObject[0];
            }
            return InstanceAssetsSync(addresses);
        }

        #endregion

        #region Load Asset Async

        public int LoadAssetAsync(
            string address,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string path = assetDetailConfig.GetPathByAddress(address);
            return RequestAssetsAsync(new string[] { address }, new string[] { path }, false, progressCallback, completeCallback, null, null, priority, userdata);
        }

        public int InstanceAssetAsync(
            string address,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string path = assetDetailConfig.GetPathByAddress(address);
            return RequestAssetsAsync(new string[] { address }, new string[] { path }, true, progressCallback, completeCallback, null, null, priority, userdata);
        }
        public int LoadAssetsAsync(
            string[] addresses,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            OnAssetsProgress progressesCallback,
            OnAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string[] paths = assetDetailConfig.GetPathsByAddresses(addresses);
            return RequestAssetsAsync(addresses, paths, false, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
        }
        public int InstanceAssetsAsync(
            string[] addresses,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            OnAssetsProgress progressesCallback,
            OnAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string[] paths = assetDetailConfig.GetPathsByAddresses(addresses);
            return RequestAssetsAsync(addresses, paths, true, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
        }
        public int LoadAssetsAsyncByLabel(
            string label,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            OnAssetsProgress progressesCallback,
            OnAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string[] addresses = assetDetailConfig.GetAddressesByLabel(label);
            if (addresses == null || addresses.Length == 0)
            {
                throw new Exception();
            }
            string[] paths = assetDetailConfig.GetPathsByAddresses(addresses);
            return RequestAssetsAsync(addresses, paths, false, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
        }
        public int InstanceAssetsAsyncByLabel(
            string label,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            OnAssetsProgress progressesCallback,
            OnAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            string[] addresses = assetDetailConfig.GetAddressesByLabel(label);
            if(addresses == null || addresses.Length == 0)
            {
                throw new Exception();
            }
            string[] paths = assetDetailConfig.GetPathsByAddresses(addresses);
            return RequestAssetsAsync(addresses, paths, true, progressCallback, completeCallback, progressesCallback, completesCallback, priority, userdata);
        }

        #endregion

        #region cancel asset Async
        public void CancelAssetsAsync(int index)
        {
            if(!resultDic.TryGetValue(index,out var result))
            {
                return;
            }
            if (result.IsDone())
            {
                return;
            }
            if (requestDic.TryGetValue(result.ID, out var request))
            {
                if (waitingRequestQueue.Contains(request))
                {
                    waitingRequestQueue.Remove(request);
                }
                else if (runningRequestList.Contains(request))
                {
                    runningRequestList.Remove(request);
                    foreach (var assetPath in request.paths)
                    {
                        if (assetNodeDic.TryGetValue(assetPath, out var assetNode))
                        {
                            assetNode.ReleaseRef();
                        }
                        else
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

            for (int i = runningRequestList.Count - 1; i >= 0; --i)
            {
                AsyncRequest request = runningRequestList[i];
                if(request.state == RequestState.Instancing || request.state == RequestState.Loading)
                {
                    OnAsyncRequestUpdate(request);
                }
                if(request.state == RequestState.WaitingForInstance)
                {
                    request.state = RequestState.Instancing;
                }else if(request.state == RequestState.LoadFinished && request.isInstance)
                {
                    request.state = RequestState.WaitingForInstance;
                }else if((request.state == RequestState.LoadFinished && !request.isInstance) 
                    || (request.state == RequestState.InstanceFinished && request.isInstance))
                {
                    OnAsyncRequestEnd(request);
                    requestDic.Remove(request.id);
                    runningRequestList.RemoveAt(i);
                    resultDic.Remove(request.id);
                    resultPool.Release(request.result);
                    requestPool.Release(request);
                }
            }

            while (waitingRequestQueue.Count > 0 && (operationCount < OperationMaxCount))
            {
                AsyncRequest request = waitingRequestQueue.Dequeue();
                request.state = RequestState.Loading;

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

        private int RequestAssetsAsync(
            string[] addresses,
            string[] paths,
            bool isInstance,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            OnAssetsProgress progressesCallback,
            OnAssetsComplete completesCallback,
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
            request.state = RequestState.WaitingForStart;

            AsyncResult result = resultPool.Get();
            result.DoInitialize(id, addresses);
            resultDic.Add(id, result);

            request.result = result;

            requestDic.Add(id, request);
            waitingRequestQueue.Enqueue(request, request.priority);

            return id;
        }
        #endregion

        #region asset node
        private AssetNode GetAssetNodeSync(string assetPath)
        {
            if (assetNodeDic.TryGetValue(assetPath, out var assetNode))
            {
                if (assetNode.IsLoaded())
                {
                    return assetNode;
                }
                else
                {
                    if(operationLDic.TryGetValue(assetPath,out var operation))
                    {
                        throw new Exception();
                    }else
                    {
                        assetNode.SetAsset(RequestAssetSync(assetPath));
                        return assetNode;
                    }
                }
            }
            assetNode = assetNodePool.Get();
            assetNode.DoInitialize(assetPath);
            assetNode.SetAsset(RequestAssetSync(assetPath));

            OnCreateAssetNodeSync(assetNode);

            assetNodeDic.Add(assetPath, assetNode);
            return assetNode;
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

        #region async request
        protected abstract void OnAsyncRequestStart(AsyncRequest request);
        protected abstract void OnAsyncRequestUpdate(AsyncRequest request);
        protected abstract void OnAsyncRequestEnd(AsyncRequest request);
        protected abstract void OnAsyncRequestCancel(AsyncRequest request);
        #endregion
    }
}
