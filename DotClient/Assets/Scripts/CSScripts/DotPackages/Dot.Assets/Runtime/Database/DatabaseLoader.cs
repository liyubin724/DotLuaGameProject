using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class DatabaseLoader : ALoader
    {
        protected override void OnInitialize(params object[] values)
        {
        }

        protected override bool OnInitializeUpdate(float deltaTime)
        {
            State = LoaderState.Initialized;
            return true;
        }

        protected override void OnUnloadUnusedAssets()
        {
            
        }

        protected override bool OnUnloadUnusedAssetsUpdate()
        {
            return true;
        }

        protected override UnityObject LoadAsset(string assetPath)
        {
            return AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityObject));
        }

        protected override bool CanStartRequest()
        {
            return true;
        }

        protected override void StartRequest(AsyncRequest request)
        {
            
        }

        protected override void UpdateRequest(AsyncRequest request)
        {
            
        }

        protected override void EndRequest(AsyncRequest request)
        {
            
        }
    }
}
