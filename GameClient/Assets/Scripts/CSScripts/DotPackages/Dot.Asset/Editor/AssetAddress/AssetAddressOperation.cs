using DotEngine.NativeDrawer.Decorator;
using DotEngine.NativeDrawer.Property;
using DotEngine.Utilities;
using System;
using System.IO;
using System.Reflection.Emit;
using static DotEngine.Asset.Datas.AssetAddressConfig;

namespace DotEditor.Asset.AssetAddress
{
    public enum AssetPackMode
    {
        Together,
        Separate,
    }

    public enum AssetAddressMode
    {
        FullPath,
        FileName,
        FileNameWithoutExtension,
    }

    [Serializable]
    public class AssetAddressOperation
    {
        [Help("AB打包方式.\nTogether:将筛选出来的所有的资源打到一个包内\nSeparate:将筛选出来的资源每个都单独打包")]
        [EnumButton]
        public AssetPackMode packMode = AssetPackMode.Together;
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

        public string GetBundleName(string assetPath)
        {
            string bundleName = string.Empty;
            if (packMode == AssetPackMode.Separate)
            {
                bundleName = assetPath.ReplaceSpecialCharacter("_");
            }
            else if (packMode == AssetPackMode.Together)
            {
                string rootFolder = Path.GetDirectoryName(assetPath).Replace("\\", "/");
                bundleName = rootFolder.ReplaceSpecialCharacter("_");
            }
            return bundleName.ToLower();
        }

        public string[] GetLabels()
        {
            //if(string.IsNullOrEmpty(labels))
            //{
            //    return new string[0];
            //}else
            //{
            //    return labels.SplitToNotEmptyArray('|');
            //}
            return labels;
        }

        private bool IsScene(string assetPath)
        {
            if(Path.GetExtension(assetPath).ToLower() == ".unity")
            {
                return true;
            }
            return false;
        }

        public AssetAddressData GetAddressData(string assetPath)
        {
            AssetAddressData addressData = new AssetAddressData();
            addressData.assetAddress = GetAddressName(assetPath);
            addressData.assetPath = assetPath;
            addressData.bundlePath = GetBundleName(assetPath);
            addressData.labels = GetLabels();
            addressData.isScene = IsScene(assetPath);

            return addressData;
        }

        public void UpdateAddressData(AssetAddressData addressData)
        {
            string assetPath = addressData.assetPath;
            addressData.assetAddress = GetAddressName(assetPath);
            addressData.bundlePath = GetBundleName(assetPath);
            addressData.labels = GetLabels();
            addressData.isScene = IsScene(assetPath);
        }
    }
}
