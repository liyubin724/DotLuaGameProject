namespace DotEngine.AAS
{
    public class BundleDescriptionConfig
    {
        public BundleDetail[] bundleDetails = new BundleDetail[0];
        public AssetDetail[] assetDetails = new AssetDetail[0];
        
        public class AssetDetail
        {
            public string path = string.Empty;
            public string bundle = string.Empty;
            public string address = string.Empty;
            public string[] labels = new string[0];
            public bool isPreload = false;
            public bool isNeverDestroy = false;
        }

        public class BundleDetail
        {
            public string fileName;
            public string hash;
            public uint crc;
            public string md5;
            public string[] dependencies = new string[0];
        }
    }


}
