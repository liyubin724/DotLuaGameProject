using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public class ResourceLoadAsyncOperation : AAsyncOperation
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

        protected override UnityObject GetResultInOperation()
        {
            ResourceRequest request = (ResourceRequest)operation;
            return request.asset;
        }
    }
}
