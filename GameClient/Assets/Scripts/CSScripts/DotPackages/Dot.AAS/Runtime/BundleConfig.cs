namespace DotEngine.AAS
{
    public class BundleConfig
    {
        public BundleDetail[] bundleDetails = new BundleDetail[0];

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
