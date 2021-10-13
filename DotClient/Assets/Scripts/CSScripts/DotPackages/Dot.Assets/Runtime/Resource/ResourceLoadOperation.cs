using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public class ResourceLoadOperation : AAssetOperation
    {
        public override UnityObject GetResult()
        {
            return Resources.Load(assetPath);
        }
    }
}