using DotEngine.Pool;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public delegate void OnAsyncOperationComplete(AAsyncOperation operation);

    public abstract class AAsyncOperation : IPoolItem
    {
        public string Path { get; set; }

        protected bool isRunning = false;
        protected AsyncOperation operation = null;

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }
        public abstract bool IsFinished { get;}
        public abstract float Progress { get; }

        public OnAsyncOperationComplete OnOperationComplete = null;

        public virtual void DoInitilize(string path,params SystemObject[] values)
        {
            this.Path = path;
        }

        public virtual void DoStart()
        {
            isRunning = true;
            operation = CreateOperation();
            if(operation!=null)
            {
                operation.completed += OnComplete;
            }
        }

        public virtual void DoEnd()
        {
            DestroyOperation();

            isRunning = false;
            Path = null;
            operation = null;
        }

        public virtual void OnGet()
        {
        }

        public virtual void OnRelease()
        {
        }

        private void OnComplete(AsyncOperation ao)
        {
            operation.completed -= OnComplete;
            OnOperationComplete?.Invoke(this);
        }

        public abstract UnityObject GetAsset();
        protected abstract AsyncOperation CreateOperation();
        protected abstract void DestroyOperation();
    }
}
