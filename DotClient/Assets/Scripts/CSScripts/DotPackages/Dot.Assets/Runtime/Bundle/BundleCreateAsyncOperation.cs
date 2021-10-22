using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public class BundleCreateAsyncOperation : AAsyncOperation
    {
        protected override AsyncOperation CreateOperation(string path)
        {
            return AssetBundle.LoadFromFileAsync(path);
        }

        protected override void DestroyOperation()
        {
        }

        protected override UnityObject GetFromOperation()
        {
            AssetBundleCreateRequest request = (AssetBundleCreateRequest)operation;
            return request.assetBundle;
        }
    }
}
