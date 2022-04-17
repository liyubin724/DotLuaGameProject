using DotEngine.Pool;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class AsyncRequest : IElement
    {
        internal int id = -1;
        internal RequestState state = RequestState.None;
        internal string[] addresses = null;
        internal string[] paths = null;
        internal bool isInstance = false;
        internal OnAssetProgress progressCallback;
        internal OnAssetComplete completeCallback;
        internal OnAssetsProgress progressesCallback;
        internal OnAssetsComplete completesCallback;
        internal AsyncPriority priority = AsyncPriority.Default;
        internal SystemObject userdata = null;

        internal AsyncResult result = null;

        public bool IsDone()
        {
            return result.IsDone();
        }

        public void SetProgress(int index, float progress)
        {
            float preProgress = result.GetProgressAt(index);
            if (preProgress != progress)
            {
                result.SetProgressAt(index, progress);

                progressCallback?.Invoke(id, addresses[index], progress, userdata);
                progressesCallback?.Invoke(id, addresses, result.GetProgresses(), userdata);
            }
        }

        public void SetUObject(int index, UnityObject uObject)
        {
            result.SetUObjectAt(index, uObject);
            completeCallback?.Invoke(id, addresses[index], uObject, userdata);

            if (result.IsDone())
            {
                completesCallback?.Invoke(id, addresses, result.GetUObjects(), userdata);
            }
        }

        public void OnGetFromPool()
        {
        }

        public void OnReleaseToPool()
        {
            id = -1;
            state = RequestState.None;
            addresses = null;
            paths = null;
            isInstance = false;
            progressCallback = null;
            completeCallback = null;
            progressesCallback = null;
            completesCallback = null;
            priority = AsyncPriority.Default;
            userdata = null;

            result = null;
        }
    }
}
