using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public class BundleLoadAsyncOperation : AAsyncOperation
    {
        private AssetBundle assetBundle = null;
        public void SetBundle(AssetBundle bundle)
        {
            assetBundle = bundle;
        }

        public override bool IsFinished
        {
            get
            {
                if (!isRunning)
                {
                    return false;
                }
                else
                {
                    return operation.isDone;
                }
            }
        }

        public override float Progress
        {
            get
            {
                if (!isRunning)
                {
                    return 0.0f;
                }
                else
                {
                    return operation.progress;
                }
            }
        }

        public override UnityObject GetAsset()
        {
            if (IsFinished)
            {
                AssetBundleRequest request = (AssetBundleRequest)operation;
                return request.asset;
            }
            return null;
        }

        protected override AsyncOperation CreateOperation()
        {
            if (assetBundle != null)
            {
                return assetBundle.LoadAssetAsync(Path);
            }
            return null;
        }

        protected override void DestroyOperation()
        {
            assetBundle = null;
        }
    }
}
