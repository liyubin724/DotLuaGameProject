using DotEngine.NativeDrawer.Decorator;
using DotEngine.NativeDrawer.Property;
using DotEditor.Utilities;
using System;

namespace DotEditor.Asset.AssetAddress
{
    [Serializable]
    public class AssetAddressFilter
    {
        [Help("资源的搜索目录")]
        [OpenFolderPath]
        public string assetFolder = "Assets";
        [Help("是否包含子目录")]
        public bool isIncludeSubfolder = true;
        [Help("使正则表达式对目录中的资源进行过滤")]
        public string fileRegex = string.Empty;

        public string[] Filter()
        {
            return DirectoryUtility.GetAsset(assetFolder, isIncludeSubfolder, fileRegex);
        }
    }
}
