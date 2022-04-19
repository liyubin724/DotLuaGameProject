using System.Collections.Generic;
using DotEngine.Core.Serialization;

namespace DotEngine.NAssets
{
    public class BundleConfig : ISerialization
    {
        public BundleData[] Datas = new BundleData[0];

        private Dictionary<string, BundleData> m_PathToDataDic = null;

        public void DoDeserialize()
        {
            m_PathToDataDic = new Dictionary<string, BundleData>();
            foreach (var data in Datas)
            {
                m_PathToDataDic.Add(data.Path, data);
            }
        }

        public void DoSerialize()
        {
        }
    }

    public class BundleData
    {
        public string Path { get; set; }
        public string Hash { get; set; }
        public string CRC { get; set; }
        public string MD5 { get; set; }
        public string[] Dependencies { get; set; } = new string[0];
        public bool IsPreload { get; set; } = false;
        public bool IsNeverDestroy { get; set; } = false;
    }
}
