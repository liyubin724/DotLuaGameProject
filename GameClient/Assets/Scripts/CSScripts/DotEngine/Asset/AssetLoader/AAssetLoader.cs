using DotEngine.Asset.Datas;
using DotEngine.Generic;
using DotEngine.Pool;
using DotEngine.Log;
using Priority_Queue;
using System;
using System.Collections.Generic;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Asset
{
    /// <summary>
    /// 资源加载器状态
    /// </summary>
    public enum AssetLoaderState
    {
        None = 0,
        Initing ,
        Error,
        Running,
    }

    public abstract class AAssetLoader
    {
        protected ObjectPool<AssetLoaderData> dataPool = new ObjectPool<AssetLoaderData>(5);

        //等待加载的资源队列，会根据优先级调整加载的先后顺序
        protected StablePriorityQueue<AssetLoaderData> dataWaitingQueue = new StablePriorityQueue<AssetLoaderData>(10);
        //当前正在加载中的资源
        protected List<AssetLoaderData> dataLoadingList = new List<AssetLoaderData>();
        //正在执行加载的加载器
        protected ListDictionary<string, AAsyncOperation> operations = new ListDictionary<string, AAsyncOperation>();
        //缓存的资源
        protected Dictionary<string, AAssetNode> assetNodeDic = new Dictionary<string, AAssetNode>();

        public int MaxLoadingCount { get; set; } = 8;
        protected AssetLoaderState State { get; set; }

        protected Action<bool> initCallback = null;
        protected string assetRootDir = string.Empty;

        protected AssetAddressConfig addressConfig = null;
        public string GetAssetPathByAddress(string address)
        {
            if(addressConfig!=null)
            {
                return addressConfig.GetPathByAddress(address);
            }
            return null;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="callback">初始化回调</param>
        /// <param name="assetDir">资源的根目录</param>
        internal void Initialize(Action<bool> callback,string assetDir)
        {
            initCallback = callback;
            assetRootDir = assetDir;

            State = AssetLoaderState.Initing;
        }

        /// <summary>
        /// 在初始阶段，每帧调用进行初始化,子类中需要重写以完成初始化过程
        /// </summary>
        protected abstract void DoInitUpdate();

        /// <summary>
        /// 按批量方式进行资源的加载
        /// 如果指定使用标签进行资源加载，则会忽略<paramref name="addresses"/>的值
        /// </summary>
        /// <param name="label">加载设定指定标签的资源</param>
        /// <param name="addresses">资源加载地址</param>
        /// <param name="isInstance">是否需要实例化</param>
        /// <param name="complete">单个资源加载完毕后回调</param>
        /// <param name="batchComplete">所有资源加载完毕后回调</param>
        /// <param name="progress">单个资源加载进度回调</param>
        /// <param name="batchProgress">所有资源加载进度回调</param>
        /// <param name="priority">优先级</param>
        /// <param name="userData">自定义参数</param>
        /// <returns></returns>
        internal AssetHandler LoadBatchAssetAsync(
            string label,
            string[] addresses,
            bool isInstance,
            OnAssetLoadComplete complete,
            OnBatchAssetLoadComplete batchComplete,
            OnAssetLoadProgress progress,
            OnBatchAssetsLoadProgress batchProgress,
            AssetLoaderPriority priority,
            SystemObject userData)
        {
            //如果指定按标签加载资源，则查找标记为指定标签的所有资源
            if(!string.IsNullOrEmpty(label))
            {
                addresses = addressConfig.GetAddressesByLabel(label);
                LogUtil.Debug(AssetConst.LOGGER_NAME, $"AssetLoader::LoadBatchAssetAsync->Load asset by label.label = {label},addresses = {string.Join(",",addresses)}");
            }

            if (addresses == null || addresses.Length == 0)
            {
                LogUtil.Error(AssetConst.LOGGER_NAME, "AAssetLoader::LoadBatchAssetAsync->addresses is null");
                return null;
            }
            //获取资源真实的路径
            string[] paths = addressConfig.GetPathsByAddresses(addresses);
            if(paths == null || paths.Length == 0)
            {
                LogUtil.Error(AssetConst.LOGGER_NAME, "AssetLoader::LoadBatchAssetAsync->paths is null");
                return null;
            }else
            {
                LogUtil.Debug(AssetConst.LOGGER_NAME, $"AssetLoader::LoadBatchAssetAsync->find assetPath by address.addresses = {string.Join(",", addresses)},path = {string.Join(",",paths)}");
            }

            if (dataWaitingQueue.Count >= dataWaitingQueue.MaxSize)
            {
                dataWaitingQueue.Resize(dataWaitingQueue.MaxSize * 2);
                LogUtil.Debug(AssetConst.LOGGER_NAME, "AssetLoader::LoadBatchAssetAsync->Reset the queue size.");
            }

            AssetLoaderData data = dataPool.Get();
            data.InitData(label, addresses, paths, isInstance, complete, progress, batchComplete, batchProgress, userData);
            data.State = AssetLoaderDataState.Waiting;
            dataWaitingQueue.Enqueue(data, (float)priority);

            return data.Handler;
        }

        internal void DoUpdate(float deltaTime)
        {
            //检查Loader初始化状态
            if(State == AssetLoaderState.Initing)
            {
                DoInitUpdate();

                if(State == AssetLoaderState.Running)
                {
                    LogUtil.Debug(AssetConst.LOGGER_NAME, "AssetLoader::DoUpdate->Loader init success.");
                    initCallback?.Invoke(true);
                }else if(State == AssetLoaderState.Error)
                {
                    LogUtil.Error(AssetConst.LOGGER_NAME, "AssetLoader::DoUpdate->Loader init failed.");
                    initCallback?.Invoke(false);
                }
                return;
            }else if(State!= AssetLoaderState.Running)
            {
                return;
            }

            DoWaitingDataUpdate();
            DoAsyncOperationUpdate();
            DoLoadingDataUpdate();
            DoUnloadUnsedAssetUpdate();
        }

        private void DoWaitingDataUpdate()
        {
           while(dataWaitingQueue.Count>0 && operations.Count<MaxLoadingCount)
            {
                AssetLoaderData data = dataWaitingQueue.Dequeue();
                StartLoadingData(data);
                data.State = AssetLoaderDataState.Loading;
                dataLoadingList.Add(data);

                LogUtil.Debug(AssetConst.LOGGER_NAME, $"AssetLoader::DoWaitingDataUpdate->Start Load Data.data = {data}");
            }
        }

        /// <summary>
        /// 开始加载指定的资源，子类需要重写
        /// </summary>
        /// <param name="data"></param>
        protected abstract void StartLoadingData(AssetLoaderData data);

        private void DoAsyncOperationUpdate()
        {
            if(operations.Count>0)
            {
                int index = 0;
                while(operations.Count>index && index<MaxLoadingCount)
                {
                    AAsyncOperation operation = operations[index];
                    operation.DoUpdate();

                    if(operation.State >= OperationState.Finished)
                    {
                        operations.RemoveAt(index);
                        OnOperationFinished(operation);
                    }else
                    {
                        ++index;
                    }
                }
            }
        }

        /// <summary>
        /// 资源加载器加载结束，子类需要重写
        /// </summary>
        /// <param name="operation"></param>
        protected abstract void OnOperationFinished(AAsyncOperation operation);

        private void DoLoadingDataUpdate()
        {
            if(dataLoadingList.Count>0)
            {
                for(int i = dataLoadingList.Count-1;i>=0;--i)
                {
                    AssetLoaderData data = dataLoadingList[i];
                    OnDataUpdate(data);
                    if(data.State>= AssetLoaderDataState.Finished)
                    {
                        dataLoadingList.RemoveAt(i);
                        dataPool.Release(data);
                    }
                }
            }
        }

        /// <summary>
        /// 子类需要重写，用于检查资源当前状况
        /// </summary>
        /// <param name="data"></param>
        protected abstract void OnDataUpdate(AssetLoaderData data);

        /// <summary>
        /// 停止资源加载
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="destroyIfIsInstnace"></param>
        internal void UnloadAssetAsync(AssetHandler handler, bool destroyIfIsInstnace)
        {
            //判断需要停止的是否还尚未开始加载
            if(dataWaitingQueue.Count>0)
            {
                foreach(var data in dataWaitingQueue)
                {
                    if(data.Handler == handler)
                    {
                        dataWaitingQueue.Remove(data);
                        dataPool.Release(data);
                        return;
                    }
                }
            }

            if(dataLoadingList.Count>0)
            {
                foreach(var data in dataLoadingList)
                {
                    if(data.Handler == handler)
                    {
                        data.DoCancel(destroyIfIsInstnace);
                        return;
                    }
                }
            }
        }

        internal void UnloadAssetByAddress(string address)
        {
            string assetPath = addressConfig.GetPathByAddress(address);
            UnloadAsset(assetPath);
        }

        protected abstract void UnloadAsset(string assetPath);

        protected internal abstract UnityObject InstantiateAsset(string address, UnityObject asset);

        private Action unloadUnusedCallback = null;
        private AsyncOperation unloadUnusedOperation = null;
        /// <summary>
        /// 深度清理资源
        /// </summary>
        /// <param name="callback">清理完毕后回调</param>
        internal void DeepUnloadUnusedAsset(Action callback)
        {
            if(unloadUnusedCallback!=null)
            {
                LogUtil.Error(AssetConst.LOGGER_NAME, "UnloadUnusedAsset is running!!");
                return;
            }

            unloadUnusedCallback = callback;

            UnloadUnusedAsset();
            
            GC.Collect();
            GC.Collect();
            unloadUnusedOperation = Resources.UnloadUnusedAssets();
        }

        protected internal abstract void UnloadUnusedAsset();

        private void DoUnloadUnsedAssetUpdate()
        {
            if(unloadUnusedOperation!=null && unloadUnusedOperation.isDone)
            {
                unloadUnusedOperation = null;
                unloadUnusedCallback?.Invoke();
                unloadUnusedCallback = null;
            }
        }
        /// <summary>
        /// 清理所有的资源
        /// </summary>
        internal virtual void DoDispose()
        {
            while(dataWaitingQueue.Count>0)
            {
                var loaderData = dataWaitingQueue.Dequeue();
                dataPool.Release(loaderData);
            }
            for(int i =dataLoadingList.Count-1;i>=0;--i)
            {
                var loaderData = dataLoadingList[i];
                dataLoadingList.RemoveAt(i);
                dataPool.Release(loaderData);
            }
            dataPool.Clear();
            operations.Clear();
            assetNodeDic.Clear();
            State = AssetLoaderState.None;
            initCallback = null;
            addressConfig = null;
        }
    }
}
