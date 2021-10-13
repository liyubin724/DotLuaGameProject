using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public class BundleLoadAsyncOperation : AAssetAsyncOperation
    {
        private AssetBundle assetBundle = null;
        public void SetBundle(AssetBundle bundle)
        {
            assetBundle = bundle;   
        }

        protected override AsyncOperation CreateOperation(string path)
        {
            if(assetBundle == null)
            {
                return null;
            }
            return assetBundle.LoadAssetAsync(path);
        }

        protected override void DisposeOperation()
        {
            assetBundle = null;
        }

        protected override void FinishOperation()
        {
        }

        protected override UnityObject GetResultInOperation()
        {
            AssetBundleRequest request = (AssetBundleRequest)operation;
            return request.asset;
        }
    }
}
