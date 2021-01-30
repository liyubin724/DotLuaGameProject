#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DotEngine.Asset
{
    public class DatabaseAsyncOperation : AAsyncOperation
    {
        public DatabaseAsyncOperation(string assetPath) : base(assetPath)
        {
        }

        protected override void OnOperationLoading()
        {
            State = OperationState.Finished;
        }

        protected override void OnOperationStart()
        {
            State = OperationState.Loading;
        }

        protected internal override Object GetAsset()
        {
            return AssetDatabase.LoadAssetAtPath(AssetPath, typeof(UnityEngine.Object));
        }

        protected internal override float GetProgress()
        {
            if (State == OperationState.Finished)
            {
                return 1.0f;
            }
            return 0.0f;
        }
    }
}
#endif 