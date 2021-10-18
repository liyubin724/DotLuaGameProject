using DotEngine.Core.Extensions;
using DotEngine.NativeDrawer.Decorator;
using DotEngine.NativeDrawer.Property;
using System;
using System.IO;

namespace DotEditor.Asset.AssetAddress
{
    /// <summary>
    /// 
    /// </summary>
    public enum AssetAddressMode
    {
        FullPath,
        FileName,
        FileNameWithoutExtension,
    }

    [Serializable]
    public class AssetAddressOperation
    {
        [Help("资源地址生成方式\nFullPath:使用资源的完整路径作为地址\nFileName:使用带后缀的文件名做为地址\nFileNameWithoutExtension:使用不带后缀的文件名作为地址")]
        [EnumButton]
        public AssetAddressMode addressMode = AssetAddressMode.FullPath;
        [Help("资源标签，加载时可以使用标签对所有的资源进行加载")]
        public string[] labels = new string[0];

        public string GetAddressName(string assetPath)
        {
            if (addressMode == AssetAddressMode.FullPath)
                return assetPath;
            else if (addressMode == AssetAddressMode.FileName)
                return Path.GetFileName(assetPath);
            else if (addressMode == AssetAddressMode.FileNameWithoutExtension)
                return Path.GetFileNameWithoutExtension(assetPath);
            else
                return assetPath;
        }

        public string GetAddress(string assetPath)
        {
            if (addressMode == AssetAddressMode.FullPath)
                return assetPath;
            else if (addressMode == AssetAddressMode.FileName)
                return Path.GetFileName(assetPath);
            else if (addressMode == AssetAddressMode.FileNameWithoutExtension)
                return Path.GetFileNameWithoutExtension(assetPath);
            else
                return assetPath;
        }

        public string[] GetLabels()
        {
            return labels;
        }
    }
}
