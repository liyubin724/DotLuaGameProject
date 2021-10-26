using DotEngine.Pool;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class AsyncRequest : IPoolItem
    {
        internal int id = -1;
        internal RequestState state = RequestState.None;
        internal string[] addresses = null;
        internal string[] paths = null;
        internal bool isInstance = false;
        internal OnLoadAssetProgress progressCallback;
        internal OnLoadAssetComplete completeCallback;
        internal OnLoadAssetsProgress progressesCallback;
        internal OnLoadAssetsComplete completesCallback;
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

                progressCallback?.Invoke(addresses[index], progress, userdata);
                progressesCallback?.Invoke(addresses, result.GetProgresses(), userdata);
            }
        }

        public void SetUObject(int index, UnityObject uObject)
        {
            result.SetUObjectAt(index, uObject);
            completeCallback?.Invoke(addresses[index], uObject, userdata);

            if (result.IsDone())
            {
                completesCallback?.Invoke(addresses, result.GetUObjects(), userdata);
            }
        }

        public void OnGet()
        {
        }

        public void OnRelease()
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
