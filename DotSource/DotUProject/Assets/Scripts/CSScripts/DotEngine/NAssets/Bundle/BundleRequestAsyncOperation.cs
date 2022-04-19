using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.NAssets
{
    internal class BundleRequestAsyncOperation : AAsyncOperation
    {
        private AssetBundle m_Bundle;

        public override void DoActivate(string path, AsyncOperationComplete complete, object[] extendedParameters)
        {
            base.DoActivate(path, complete, extendedParameters);
            m_Bundle = extendedParameters[0] as AssetBundle;
        }

        public override UnityObject GetAsset()
        {
            if (State == AsyncOperationState.Finished)
            {
                return (m_Operation as AssetBundleRequest).asset;
            }
            return null;
        }

        public override void DoDeactivate()
        {
            base.DoDeactivate();
            m_Bundle = null;
        }

        protected override AsyncOperation CreateOperation()
        {
            return m_Bundle.LoadAssetAsync(Path);
        }
    }
}
