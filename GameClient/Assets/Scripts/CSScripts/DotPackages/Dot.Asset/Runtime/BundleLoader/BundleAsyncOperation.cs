using UnityEngine;

namespace DotEngine.Asset
{
    /// <summary>
    /// AssetBundle异步加载操作
    /// </summary>
    public class BundleAsyncOperation : AAsyncOperation
    {
        private AssetBundleCreateRequest asyncOperation = null;

        public BundleAsyncOperation(string assetPath, GetAssetFilePath getFilePath) : base(assetPath, getFilePath)
        {
        }

        protected override void OnOperationLoading()
        {
            if (State == OperationState.Loading)
            {
                if (asyncOperation.isDone)
                {
                    State = OperationState.Finished;
                }
            }
        }

        protected override void OnOperationStart()
        {
            if (getFilePath == null)
            {
                DebugLog.Error(AssetConst.LOGGER_NAME, "BundleAsyncOperation::CreateOperation->getFilePath is Null");
                State = OperationState.Finished;
                return;
            }

            bool isUsedOffset = getFilePath(AssetPath, out string bundlePath, out ulong offset);
            if (isUsedOffset)
            {
                asyncOperation = AssetBundle.LoadFromFileAsync(bundlePath, 0, offset);
            }
            else
            {
                asyncOperation = AssetBundle.LoadFromFileAsync(bundlePath);
            }
            State = OperationState.Loading;
        }

        protected internal override Object GetAsset()
        {
            if (State == OperationState.Finished)
            {
                return asyncOperation?.assetBundle;
            }
            else
            {
                DebugLog.Error(AssetConst.LOGGER_NAME, "BundleAsyncOperation::GetAsset->bundle is not loaded");
                return null;
            }
        }

        protected internal override float GetProgress()
        {
            if (State == OperationState.None)
            {
                return 0.0f;
            }
            else if (State == OperationState.Loading)
            {
                return asyncOperation.progress;
            }
            else
            {
                return 1.0f;
            }
        }
    }
}
