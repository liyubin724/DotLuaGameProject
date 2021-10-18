using DotEditor.Utilities;
using DotEngine.NativeDrawer.Decorator;
using System;

namespace DotEditor.Asset.Group
{
    [Serializable]
    public class AssetFilter
    {
        [Help("是否包含子目录")]
        public bool IncludeSubfolder = true;
        [Help("使正则表达式对目录中的资源进行过滤")]
        public string FileRegex = string.Empty;

        public string[] GetAssets(string rootFolder)
        {
            return DirectoryUtility.GetAsset(rootFolder, IncludeSubfolder, FileRegex);
        }
    }
}
