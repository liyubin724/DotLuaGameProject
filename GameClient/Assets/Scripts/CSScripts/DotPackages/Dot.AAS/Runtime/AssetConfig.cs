using System.Collections.Generic;

namespace DotEngine.AAS
{
    public class AssetConfig
    {
        public AssetDetail[] assetDetails = new AssetDetail[0];

        private List<string> preloadAssets = null;
        public string[] GetPreloadAssets()
        {
            if(preloadAssets == null)
            {
                preloadAssets = new List<string>();
                foreach(var data in assetDetails)
                {
                    preloadAssets.Add(data.path);
                }
            }
            return preloadAssets.ToArray();
        }

        public class AssetDetail
        {
            public string path = string.Empty;
            public string bundle = string.Empty;
            public string address = string.Empty;
            public string[] labels = new string[0];
            public bool isPreload = false;
            public bool isNeverDestroy = false;
        }
    }
}
