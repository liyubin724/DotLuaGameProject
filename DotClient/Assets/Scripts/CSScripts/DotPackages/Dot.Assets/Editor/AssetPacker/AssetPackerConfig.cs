using System.Collections.Generic;

namespace DotEditor.Asset.AssetPacker
{
    public class AssetPackerConfig 
    {
        public List<AssetPackerGroupData> groupDatas = new List<AssetPackerGroupData>();
        public void Sort()
        {
            groupDatas.Sort((item1, item2) =>
            {
                return item1.groupName.CompareTo(item2.groupName);
            });
        }
    }

    public class AssetPackerGroupData
    {
        public string groupName = string.Empty;

        public bool isMain = false;
        public bool isPreload = false;
        public bool isNeverDestroy = false;

        public List<AssetPackerAddressData> assetFiles = new List<AssetPackerAddressData>();
    }

    public class AssetPackerAddressData
    {
        public string assetAddress;
        public string assetPath;
        public string bundlePath;
        public string bundlePathMd5;
        public string[] labels = new string[0];
    }
}
