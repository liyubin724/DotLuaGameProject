using System.Collections.Generic;

namespace DotEngine.Assets
{
    public class AssetDetailConfig
    {
        public int Version = 1;
        public AssetDetail[] Details = new AssetDetail[0];

        private Dictionary<string, AssetDetail> addressToDetailDic = null;
        private Dictionary<string, AssetDetail> pathToDetailDic = null;
        private Dictionary<string, List<string>> labelToAddressesDic = null;

        public void InitConfig()
        {
            addressToDetailDic = new Dictionary<string, AssetDetail>();
            pathToDetailDic = new Dictionary<string, AssetDetail>();
            labelToAddressesDic = new Dictionary<string, List<string>>();
            foreach (var detail in Details)
            {
                addressToDetailDic.Add(detail.Address, detail);
                pathToDetailDic.Add(detail.Path, detail);
                if(detail.Labels!=null && detail.Labels.Length>0)
                {
                    foreach (var label in detail.Labels)
                    {
                        if(!labelToAddressesDic.TryGetValue(label,out var addressList))
                        {
                            addressList = new List<string>();
                            labelToAddressesDic.Add(label, addressList);
                        }
                        addressList.Add(detail.Address);
                    }
                }
            }
        }

        public bool IsSceneByPath(string path)
        {
            if(pathToDetailDic.TryGetValue(path,out var detail))
            {
                return detail.IsScene;
            }

            return false;
        }

        public bool IsSceneByAddress(string address)
        {
            if(addressToDetailDic.TryGetValue(address,out var detail))
            {
                return detail.IsScene;
            }

            return false;
        }

        public string GetPathByAddress(string address)
        {
            if(addressToDetailDic.TryGetValue(address,out var detail))
            {
                return detail.Path;
            }
            return null;
        }

        public string[] GetPathsByAddresses(string[] addresses)
        {
            string[] paths = new string[addresses.Length];
            for (int i = 0; i < addresses.Length; i++)
            {
                paths[i] = GetPathByAddress(addresses[i]);
            }
            return paths;
        }

        public string GetAddressByPath(string path)
        {
            if(pathToDetailDic.TryGetValue(path,out var detail))
            {
                return detail.Address;
            }
            return null;
        }

        public string[] GetAddressesByLabel(string label)
        {
            if(labelToAddressesDic.TryGetValue(label,out var list))
            {
                return list.ToArray();
            }
            return new string[0];
        }

        public string[] GetPathsByLabel(string label)
        {
            string[] addresses = GetAddressesByLabel(label);
            return GetPathsByAddresses(addresses);
        }

        public string GetBundleByAddress(string address)
        {
            if(addressToDetailDic.TryGetValue(address,out var detail))
            {
                return detail.Bundle;
            }
            return null;
        }

        public string GetBundleByPath(string path)
        {
            if(pathToDetailDic.TryGetValue(path,out var detail))
            {
                return detail.Bundle;
            }
            return null;
        }
    }

    public class AssetDetail
    {
        public string Address;
        public string Path;
        public string Bundle = null;
        public bool IsScene = false;
        public string[] Labels = new string[0];
    }
}
