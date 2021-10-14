using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public abstract class AAsyncOperation
    {
        protected AsyncOperation operation = null;
        protected bool isRunning = false;
        protected string assetPath = null;

        public bool IsFinished
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

        public float GetProgress()
        {
            if(operation!=null)
            {
                return operation.progress;
            }

            return 0;
        }

        public UnityObject GetResult()
        {
            if(!IsFinished)
            {
                Debug.LogError("The asyncOperation has finished.");
                return null;
            }
            return GetResultInOperation();
        }

        public void DoInitilize(string path)
        {
            assetPath = path;
        }

        public void DoStart()
        {
            operation = CreateOperation(assetPath);
            operation.completed += OnComplete;
        }

        private void OnComplete(AsyncOperation ao)
        {
            operation.completed -= OnComplete;
            FinishOperation();
        }

        public void DoEnd()
        {
            DisposeOperation();
            operation = null;
            assetPath = null;
            isRunning = false;
        }

        protected abstract AsyncOperation CreateOperation(string path);
        protected abstract UnityObject GetResultInOperation();
        protected abstract void FinishOperation();
        protected abstract void DisposeOperation();
    }
}
