using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DotEngine.Assets
{
    public class BundleDetailConfig
    {
        public static BundleDetailConfig ReadFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }
            string content = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<BundleDetailConfig>(content);
        }

        public static bool WriteToFile(BundleDetailConfig config, string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || config == null)
            {
                return false;
            }

            string jsonContent = JsonConvert.SerializeObject(config);
            File.WriteAllText(filePath, jsonContent);
            return true;
        }

        public static BundleDetailConfig ReadFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<BundleDetailConfig>(text);
        }

        public static string WriteToText(BundleDetailConfig config)
        {
            return JsonConvert.SerializeObject(config);
        }

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
        public string Hash;
        public string CRC;
        public string MD5;
        public string[] Dependencies = new string[0];

        public bool IsPreload = false;
        public bool IsNeverDestroy = false;
    }
}
