using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public class BundleLoadOperation : AAssetOperation
    {
        private AssetBundle assetBundle = null;
        public void SetBundle(AssetBundle bundle)
        {
            assetBundle = bundle;
        }

        public override UnityObject GetResult()
        {
            return assetBundle.LoadAsset(assetPath);
        }
    }
}
