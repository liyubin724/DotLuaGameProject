using DotEngine.Pool;
using Priority_Queue;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Asset
{
    /// <summary>
    /// 资源加载状态
    /// </summary>
    public enum AssetLoaderDataState
    {
        None = 0,
        Waiting,
        Loading,
        Finished,
        Canceled,
        Error,
    }

    /// <summary>
    /// 由于资源的加载是异步的，所以需要存储加载时数据，用于加载完成及进度更新
    /// </summary>
    public class AssetLoaderData : StablePriorityQueueNode, IObjectPoolItem
    {
        private string label = string.Empty;
        private string[] addresses = new string[0];
        private string[] paths = new string[0];
        private bool isInstance = false;
        private OnAssetLoadComplete completeCallback;
        private OnAssetLoadProgress progressCallback;
        private OnBatchAssetLoadComplete batchCompleteCallback;
        private OnBatchAssetsLoadProgress batchProgressCallback;
        private SystemObject userData = null;

        internal AssetHandler Handler { get; set; }
        internal AssetLoaderDataState State { get; set; }

        /// <summary>
        /// 返回资源路径
        /// </summary>
        public string[] Paths { get => paths; }

        public void InitData(string label,string[] addresses,string[] paths,bool isInstance,
            OnAssetLoadComplete complete,OnAssetLoadProgress progress,
            OnBatchAssetLoadComplete batchComplete,OnBatchAssetsLoadProgress batchProgress,
            SystemObject userData)
        {
            this.addresses = addresses;
            this.paths = paths;
            this.isInstance = isInstance;
            this.completeCallback = complete;
            this.progressCallback = progress;
            this.batchCompleteCallback = batchComplete;
            this.batchProgressCallback = batchProgress;
            this.userData = userData;

            Handler = new AssetHandler(label, addresses, userData);
        }
        
        /// <summary>
        /// 某个资源加载完毕
        /// </summary>
        /// <param name="index"></param>
        /// <param name="assetNode"></param>
        internal void DoComplete(int index,AAssetNode assetNode)
        {
            paths[index] = null;

            UnityObject uObj;
            if (isInstance)
            {
                uObj = assetNode.GetInstance();
            }
            else
            {
                uObj = assetNode.GetAsset();
            }
            Handler.UObjects[index] = uObj;

            progressCallback?.Invoke(addresses[index], 1.0f, userData);
            completeCallback?.Invoke(addresses[index], uObj, userData);
        }

        private bool isProgressChanged = false;
        /// <summary>
        /// 某个资源加载进度发生变化
        /// </summary>
        /// <param name="index"></param>
        /// <param name="progress"></param>
        internal void DoProgress(int index,float progress)
        {
            if(progress!=Handler.Progresses[index])
            {
                isProgressChanged = true;

                Handler.Progresses[index] = progress;
                progressCallback?.Invoke(addresses[index], progress,userData);
            }
        }
        /// <summary>
        /// 所有的资源加载完毕
        /// </summary>
        internal void DoBatchComplete()
        {
            Handler.IsDone = true;
            batchProgressCallback?.Invoke(addresses, Handler.Progresses, userData);
            batchCompleteCallback?.Invoke(addresses, Handler.UObjects, userData);
            State = AssetLoaderDataState.Finished;
        }

        internal void DoBatchProgress()
        {
            if(isProgressChanged)
            {
                batchProgressCallback?.Invoke(addresses, Handler.Progresses, userData);
                isProgressChanged = false;
            }
        }

        internal void DoCancel(bool destroyIfIsInstnace)
        {
            Handler.DoCancel(isInstance, destroyIfIsInstnace);
            Handler = null;

            completeCallback = null;
            progressCallback = null;
            batchCompleteCallback = null;
            batchProgressCallback = null;
            userData = null;

            State = AssetLoaderDataState.Canceled;
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            label = string.Empty;
            addresses = new string[0];
            paths = new string[0];
            isInstance = false;
            completeCallback = null;
            progressCallback = null;
            batchCompleteCallback = null;
            batchProgressCallback = null;
            userData = null;

            Handler = null;

            State = AssetLoaderDataState.None;
        }

        public void OnNew()
        {
        }
    }
}
