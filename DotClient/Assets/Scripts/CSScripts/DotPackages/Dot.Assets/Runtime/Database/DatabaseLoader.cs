using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class DatabaseLoader : ALoader
    {
        protected override bool CanStartRequest()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnAsyncRequestEnd(AsyncRequest request)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnAsyncRequestStart(AsyncRequest request)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnAsyncRequestUpdate(AsyncRequest request)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCreateAssetNodeAsync(AssetNode assetNode)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCreateAssetNodeSync(AssetNode assetNode)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInitialize(params object[] values)
        {
        }

        protected override bool OnInitializeUpdate(float deltaTime)
        {
            State = LoaderState.Initialized;
            return true;
        }

        protected override void OnReleaseAssetNode(AssetNode assetNode)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnUnloadUnusedAssets()
        {
            
        }

        protected override bool OnUnloadUnusedAssetsUpdate()
        {
            return true;
        }

        protected override UnityObject RequestAssetSync(string assetPath)
        {
            return AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityObject));
        }

    }
}
