#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class DatabaseAssetAsyncOperation : AAsyncOperation
    {
        public override bool IsFinished
        {
            get
            {
                if(!isRunning)
                {
                    return true;
                }else
                {
                    return false;
                }
            }
        }

        public override float Progress
        {
            get
            {
                if(!isRunning)
                {
                    return 0.0f;
                }else
                {
                    return 1.0f;
                }
            }
        }

        public override UnityObject GetAsset()
        {
            return AssetDatabase.LoadAssetAtPath(Path, typeof(UnityObject));
        }

        protected override AsyncOperation CreateOperation()
        {
            return null;
        }

        protected override void DestroyOperation()
        {
        }
    }
}
#endif