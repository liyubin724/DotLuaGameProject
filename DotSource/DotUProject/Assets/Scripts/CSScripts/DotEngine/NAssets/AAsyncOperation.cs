using SystemObject = System.Object;
using UAsyncOperation = UnityEngine.AsyncOperation;
using UnityObject = UnityEngine.Object;

namespace DotEngine.NAssets
{
    internal enum AsyncOperationState
    {
        None = 0,
        Waiting,
        Running,
        Finished,
    }

    internal delegate void AsyncOperationComplete(AAsyncOperation operation);
    internal abstract class AAsyncOperation
    {
        public string Path { get; set; }
        public AsyncOperationComplete OnComplete { get; private set; }

        public AsyncOperationState State { get; protected set; } = AsyncOperationState.None;
        public bool IsDone => State == AsyncOperationState.Finished;
        public float Progress
        {
            get
            {
                if(State == AsyncOperationState.Finished)
                {
                    return 1.0f;
                }else if(State == AsyncOperationState.Running)
                {
                    return m_Operation.progress;
                }
                else
                {
                    return 0.0f;
                }
            }
        }

        protected UAsyncOperation m_Operation = null;

        public virtual void DoActivate(string path, AsyncOperationComplete complete, SystemObject[] extendedParameters)
        {
            Path = path;
            State = AsyncOperationState.Waiting;
            OnComplete = complete;
        }

        public virtual void DoStart()
        {
            if(State != AsyncOperationState.Waiting)
            {
                return;
            }

            State = AsyncOperationState.Running;
            m_Operation = CreateOperation();
            m_Operation.completed += OnOperationComplete;
        }

        public virtual void DoDeactivate()
        {
            Path = null;
            OnComplete = null;
            m_Operation = null;
            State = AsyncOperationState.None;
        }

        public abstract UnityObject GetAsset();
        protected abstract UAsyncOperation CreateOperation();

        private void OnOperationComplete(UAsyncOperation operation)
        {
            operation.completed -= OnOperationComplete;
            State = AsyncOperationState.Finished;

            OnComplete?.Invoke(this);
        }
    }
}
