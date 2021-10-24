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

        protected override void OnCreateAssetNode(AssetNode node)
        {
            throw new System.NotImplementedException();
        }

        protected override bool CanStartRequest()
        {
            throw new System.NotImplementedException();
        }

        protected override void StartRequest(AsyncRequest request)
        {
            throw new System.NotImplementedException();
        }

        protected override void UpdateRequest(AsyncRequest reqest)
        {
            throw new System.NotImplementedException();
        }

        protected override void EndRequest(AsyncRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
