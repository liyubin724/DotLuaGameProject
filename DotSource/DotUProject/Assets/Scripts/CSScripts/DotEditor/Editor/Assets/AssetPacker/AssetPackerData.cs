using System.Collections.Generic;

namespace DotEditor.Assets.Packer
{
    public class PackerData
    {
        public List<PackerGroupData> groupDatas = new List<PackerGroupData>();
    }

    public class PackerGroupData
    {
        public string Name;
        public List<PackerBundleData> bundleDatas = new List<PackerBundleData>();
    }

    public class PackerBundleData
    {
        public string Path;

        public List<PackerAssetData> assetDatas = new List<PackerAssetData>();
    }

    public class PackerAssetData
    {
        public string Address;
        public string Path;
        public bool IsMainAsset = false;
        public bool IsScene = false;
        public string[] Labels = new string[0];
    }
}
