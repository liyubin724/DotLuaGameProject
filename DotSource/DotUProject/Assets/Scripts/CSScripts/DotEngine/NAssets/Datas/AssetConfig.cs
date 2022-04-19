using System.Collections.Generic;
using DotEngine.Core.Serialization;

namespace DotEngine.NAssets
{
    public class AssetConfig : ISerialization
    {
        public AssetData[] Datas { get; set; } = new AssetData[0];

        private Dictionary<string, AssetData> m_AddressToDataDic = null;
        private Dictionary<string, AssetData> m_PathToDataDic = null;
        private Dictionary<string, List<string>> m_LabelToAddressListDic = null;

        public void DoDeserialize()
        {
            m_AddressToDataDic = new Dictionary<string, AssetData>();
            m_PathToDataDic = new Dictionary<string, AssetData>();
            m_LabelToAddressListDic = new Dictionary<string, List<string>>();
            foreach (var data in Datas)
            {
                m_AddressToDataDic.Add(data.Address, data);
                m_PathToDataDic.Add(data.Path, data);
                if (data.Labels != null && data.Labels.Length > 0)
                {
                    foreach (var label in data.Labels)
                    {
                        if (!m_LabelToAddressListDic.TryGetValue(label, out var addressList))
                        {
                            addressList = new List<string>();
                            m_LabelToAddressListDic.Add(label, addressList);
                        }
                        addressList.Add(data.Address);
                    }
                }
            }
        }

        public void DoSerialize()
        {
        }
    }

    public class AssetData
    {
        public string Address { get; set; }
        public string Path { get; set; }
        public string Bundle { get; set; }
        public bool IsScene { get; set; } = false;
        public string[] Labels { get; set; } = new string[0];
    }
}
