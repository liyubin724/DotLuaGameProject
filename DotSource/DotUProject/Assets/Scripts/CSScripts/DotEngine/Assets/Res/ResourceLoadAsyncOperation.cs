using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public class ResourceLoadAsyncOperation : AAsyncOperation
    {
        //protected override AsyncOperation CreateOperation(string path)
        //{
        //    return Resources.LoadAsync(path);
        //}

        //protected override void DestroyOperation()
        //{
        //}

        //protected override UnityObject GetFromOperation()
        //{
        //    ResourceRequest request = (ResourceRequest)operation;
        //    return request.asset;
        //}
        public override bool IsFinished => throw new System.NotImplementedException();

        public override float Progress => throw new System.NotImplementedException();

        public override UnityObject GetAsset()
        {
            throw new System.NotImplementedException();
        }

        protected override AsyncOperation CreateOperation()
        {
            throw new System.NotImplementedException();
        }

        protected override void DestroyOperation()
        {
            throw new System.NotImplementedException();
        }
    }
}
