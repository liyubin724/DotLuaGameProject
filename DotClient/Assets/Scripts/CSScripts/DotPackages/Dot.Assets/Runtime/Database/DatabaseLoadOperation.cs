using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public class DatabaseLoadOperation : AAssetOperation
    {
        public override UnityObject GetResult()
        {
            return AssetDatabase.LoadAssetAtPath(assetPath,typeof(UnityObject));
        }
    }
}