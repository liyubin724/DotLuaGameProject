using UnityEngine;

namespace DotEngine.NAssets
{
    internal class BundleCreateAsyncOperation : AAsyncOperation
    {
        public override UnityEngine.Object GetAsset()
        {
            if (State == AsyncOperationState.Finished)
            {
                return (m_Operation as AssetBundleCreateRequest).assetBundle;
            }
            return null;
        }

        protected override AsyncOperation CreateOperation()
        {
            return AssetBundle.LoadFromFileAsync(Path);
        }
    }
}
