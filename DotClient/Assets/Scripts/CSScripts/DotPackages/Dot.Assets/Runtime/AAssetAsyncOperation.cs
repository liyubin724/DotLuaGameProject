using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public abstract class AAssetAsyncOperation : AOperation
    {
        protected AsyncOperation operation = null;

        public override bool IsFinished
        {
            get
            {
                if(operation!=null)
                {
                    return operation.isDone;
                }
                return false;
            }
        }

        public override float GetProgress()
        {
            if(operation!=null)
            {
                return operation.progress;
            }

            return 0;
        }

        public override UnityObject GetResult()
        {
            if(!IsFinished)
            {
                Debug.LogError("The asyncOperation has finished.");
                return null;
            }
            return GetResultInOperation();
        }

        public override void DoStart(string path)
        {
            operation = CreateOperation(path);
            operation.completed += OnComplete;
        }

        private void OnComplete(AsyncOperation ao)
        {
            operation.completed -= OnComplete;
            FinishOperation();
        }

        public override void DoEnd()
        {
            DisposeOperation();
            operation = null;
        }

        protected abstract AsyncOperation CreateOperation(string path);
        protected abstract UnityObject GetResultInOperation();
        protected abstract void FinishOperation();
        protected abstract void DisposeOperation();
    }
}
