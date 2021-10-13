using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public class BundleCreateOperation : AAssetOperation
    {
        public override UnityObject GetResult()
        {
            return AssetBundle.LoadFromFile(assetPath);
        }
    }
}
