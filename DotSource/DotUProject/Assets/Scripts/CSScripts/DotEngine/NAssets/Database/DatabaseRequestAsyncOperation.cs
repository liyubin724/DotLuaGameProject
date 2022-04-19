#if UNITY_EDITOR
using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.NAssets
{
    internal class DatabaseRequestAsyncOperation : AAsyncOperation
    {
        public override void DoStart()
        {
            if (State != AsyncOperationState.Waiting)
            {
                return;
            }

            State = AsyncOperationState.Running;
            WaitingNextFrame();
        }

        private async void WaitingNextFrame()
        {
            await Task.Yield();

            State = AsyncOperationState.Finished;
            OnComplete?.Invoke(this);
        }

        public override UnityObject GetAsset()
        {
            if (State == AsyncOperationState.Finished)
            {
                return AssetDatabase.LoadAssetAtPath(Path, typeof(UnityObject));
            }
            return null;
        }

        protected override AsyncOperation CreateOperation()
        {
            throw new NotImplementedException();
        }
    }
}
#endif