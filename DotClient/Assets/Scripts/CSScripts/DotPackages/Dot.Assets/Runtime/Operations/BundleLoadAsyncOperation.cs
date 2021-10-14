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

        protected override AsyncOperation CreateOperation(string path)
        {
            if (assetBundle != null)
            {
                return assetBundle.LoadAssetAsync(path);
            }
            return null;
        }

        protected override void DestroyOperation()
        {
            assetBundle = null;
        }

        protected override UnityObject GetFromOperation()
        {
            AssetBundleRequest request = (AssetBundleRequest)operation;
            return request.asset;
        }
    }
}
