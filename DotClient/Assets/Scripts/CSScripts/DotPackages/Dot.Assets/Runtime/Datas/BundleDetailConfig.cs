using System.Collections.Generic;

namespace DotEngine.Assets
{
    public class BundleDetailConfig
    {
        public int Version = 1;
        public BundleDetail[] Details = new BundleDetail[0];

        private Dictionary<string, BundleDetail> detailDic = null;
        public void InitConfig()
        {
            detailDic = new Dictionary<string, BundleDetail>();
            foreach (var detail in Details)
            {
                detailDic.Add(detail.Path, detail);
            }
        }

        public string[] GetDependencies(string bundlePath)
        {
            if (detailDic.TryGetValue(bundlePath, out var detail))
            {
                return detail.Dependencies;
            }
            return null;
        }

        public string[] GetPreloadPaths()
        {
            List<string> paths = new List<string>();
            foreach (var detail in Details)
            {
                if (detail.IsPreload)
                {
                    paths.Add(detail.Path);
                }
            }
            return paths.ToArray();
        }

        public bool IsNeverDestroy(string bundlePath)
        {
            if (detailDic.TryGetValue(bundlePath, out var detail))
            {
                return detail.IsNeverDestroy;
            }

            return false;
        }
    }

    public class BundleDetail
    {
        public string Path;
        public string MD5;
        public string[] Dependencies = new string[0];

        public bool IsPreload = false;
        public bool IsNeverDestroy = false;
    }
}
