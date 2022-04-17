using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class BundleAsyncOperation : AAsyncOperation
    {
        private string bundleRootDir = null;
        public override bool IsFinished
        {
            get
            {
                if(!IsRunning)
                {
                    return false;
                }else
                {
                    return operation.isDone;
                }
            }
        }

        public override float Progress
        {
            get
            {
                if(!IsRunning)
                {
                    return 0.0f;
                }else
                {
                    return operation.progress;
                }
            }
        }

        public override UnityObject GetAsset()
        {
            if(IsFinished)
            {
                AssetBundleCreateRequest request = (AssetBundleCreateRequest)operation;
                return request.assetBundle;
            }
            return null;
        }

        public override void DoInitilize(string path, params object[] values)
        {
            base.DoInitilize(path, values);
            bundleRootDir = (string)(values[0]);
        }

        protected override AsyncOperation CreateOperation()
        {
            return AssetBundle.LoadFromFileAsync($"{bundleRootDir}/{Path}");
        }

        protected override void DestroyOperation()
        {
        }
    }
}
