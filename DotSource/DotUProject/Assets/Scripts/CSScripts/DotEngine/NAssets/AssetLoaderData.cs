using UnityObject = UnityEngine.Object;

namespace DotEngine.NAssets
{
    internal class AssetLoaderData
    {
        public AssetAsyncRequest Request { get; set; }
        public AssetAsyncResult Result { get; set; }

        public AssetLoaderData()
        {
        }

        internal void SetProgress(int index,float progress)
        {
            float preProgress = Result.Progresses[index];
            if(preProgress!=progress)
            {
                Result.SetProgress(index, progress);

                Request.OnProgress?.Invoke(Request.Id, Request.Addresses[index], progress, Request.Userdata);
                Request.OnBatchProgress?.Invoke(Request.Id, Request.Addresses, Result.Progresses, Request.Userdata);
            }
        }

        internal void SetUObject(int index,UnityObject uObject)
        {
            Result.SetUObject(index, uObject);

            Request.OnComplete?.Invoke(Request.Id, Request.Addresses[index], uObject, Request.Userdata);
            if(Result.IsDone)
            {
                Request.OnBatchComplete?.Invoke(Request.Id, Request.Addresses, Result.UObjects, Request.Userdata);
            }
        }

        internal void DoRelease()
        {
            Request = null;
            Result = null;
        }
    }
}
