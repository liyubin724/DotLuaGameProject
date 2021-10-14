using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public class DatabaseLoadAsyncOperation : AAsyncOperation
    {
        private string assetPath = null;

        public override bool IsFinished
        {
            get
            {
                if(string.IsNullOrEmpty(assetPath))
                {
                    return false;
                }
                return true;
            }
        }

        protected override AsyncOperation CreateOperation(string path)
        {
            assetPath = path;
            return null;
        }

        protected override void DisposeOperation()
        {
        }

        protected override void FinishOperation()
        {
            assetPath = null;
        }

        protected override UnityObject GetResultInOperation()
        {
            return AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityObject));
        }
    }
}
