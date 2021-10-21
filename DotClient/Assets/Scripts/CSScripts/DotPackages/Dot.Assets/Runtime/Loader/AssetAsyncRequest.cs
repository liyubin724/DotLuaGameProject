using SystemObject = System.Object;

namespace DotEngine.Assets
{
    internal class AssetAsyncRequest
    {
        internal int id = -1;
        internal string label = null;
        internal string[] addresses = null;
        internal string[] paths = null;
        internal bool isInstance = false;
        internal OnLoadAssetProgress progressCallback;
        internal OnLoadAssetComplete completeCallback;
        internal OnLoadAssetsProgress progressesCallback;
        internal OnLoadAssetsComplete completesCallback;
        internal SystemObject userdata = null;
    }
}
