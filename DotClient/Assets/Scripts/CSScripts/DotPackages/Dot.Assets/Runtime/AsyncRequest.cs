using DotEngine.Pool;
using SystemObject = System.Object;

namespace DotEngine.Assets
{
    public class AsyncRequest : IPoolItem
    {
        internal int id = -1;
        internal AsyncState state = AsyncState.None;
        internal string label = null;
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

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            id = -1;
            state = AsyncState.None;
            label = null;
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
