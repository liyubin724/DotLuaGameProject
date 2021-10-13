using UnityEngine;

namespace DotEngine.Assets.Operations
{
    public class ResourceLoadAsyncOperation : AAssetAsyncOperation
    {
        protected override AsyncOperation CreateOperation(string path)
        {
            return Resources.LoadAsync(path);
        }

        protected override void DisposeOperation()
        {
        }

        protected override void FinishOperation()
        {
        }

        protected override UnityEngine.Object GetResultInOperation()
        {
            ResourceRequest request = (ResourceRequest)operation;
            return request.asset;
        }
    }
}
