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

        protected override void DestroyOperation()
        {
        }

        protected override UnityObject GetFromOperation()
        {
            ResourceRequest request = (ResourceRequest)operation;
            return request.asset;
        }
    }
}
