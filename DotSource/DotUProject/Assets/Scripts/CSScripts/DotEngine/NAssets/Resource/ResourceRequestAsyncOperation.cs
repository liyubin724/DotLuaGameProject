using UnityEngine;

namespace DotEngine.NAssets
{
    internal class ResourceRequestAsyncOperation : AAsyncOperation
    {
        public override UnityEngine.Object GetAsset()
        {
            if (State == AsyncOperationState.Finished)
            {
                return (m_Operation as ResourceRequest).asset;
            }
            return null;
        }

        protected override AsyncOperation CreateOperation()
        {
            return Resources.LoadAsync(Path);
        }
    }
}
