using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public class DatabaseLoadAsyncOperation : AOperation
    {
        public override bool IsFinished
        {
            get
            {
                if(isRunning && !string.IsNullOrEmpty(assetPath))
                {
                    return true;
                }
                return false;
            }
        }

        public override float Progress
        {
            get
            {
                if (isRunning && !string.IsNullOrEmpty(assetPath))
                {
                    return 1.0f;
                }
                return 0.0f;
            }
        }

        public override UnityObject GetAsset()
        {
            return AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityObject));
        }
    }
}
