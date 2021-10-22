using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class DatabaseLoader : ALoader
    {
        protected override UnityObject LoadAsset(string assetPath)
        {
            return AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityObject));
        }

        protected override void OnInitialize(params object[] values)
        {
            
        }

        protected override bool OnInitializeUpdate(float deltaTime)
        {
            return true;
        }

        protected override void OnUnloadUnusedAssets()
        {
            
        }

        protected override bool OnUnloadUnusedAssetsUpdate()
        {
            return true;
        }

        protected override void OnDestroyAssetNode(AssetNode node)
        {
            
        }
    }
}
