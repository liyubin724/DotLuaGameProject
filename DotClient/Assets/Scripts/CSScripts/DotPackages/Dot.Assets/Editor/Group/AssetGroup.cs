using DotEngine.Assets;
using DotEngine.Core.Extensions;
using DotEngine.Crypto;
using DotEngine.NativeDrawer.Decorator;
using DotEngine.NativeDrawer.Property;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Space = DotEngine.NativeDrawer.Decorator.SpaceAttribute;

namespace DotEditor.Asset.Group
{
    public enum AssetAddressMode
    {
        FullPath = 0,
        FileName,
        FileNameWithoutExtension,
    }

    [Serializable]
    public class AssetAddressOperation
    {
        [Help("资源地址生成方式\nFullPath:使用资源的完整路径作为地址\nFileName:使用带后缀的文件名做为地址\nFileNameWithoutExtension:使用不带后缀的文件名作为地址")]
        [EnumButton]
        public AssetAddressMode AddressMode = AssetAddressMode.FullPath;
        [Help("资源标签，加载时可以使用标签对所有的资源进行加载")]
        public string[] Labels = new string[0];
    }

    public enum AssetPackMode
    {
        Together,
        Separate,
    }

    [Serializable]
    public class AssetBundleOperation
    {
        public bool IsUseMD5 = false;

        [Help("AB打包方式.\nTogether:将筛选出来的所有的资源打到一个包内\nSeparate:将筛选出来的资源每个都单独打包")]
        [EnumButton]
        public AssetPackMode PackMode = AssetPackMode.Separate;
    }

    public class AssetGroup : ScriptableObject
    {
        [Help("此配置是否生效")]
        public bool Enable = true;

        [Space]
        [Help("资源所属分组的名称")]
        public string GroupName = "Asset Address Group";
        [OpenFolderPath]
        public string RootFolder = "Assets";
        [Help("根据指定的配置进行资源的筛选")]
        public List<AssetFilter> filters = new List<AssetFilter>();

        [Space]
        [Help("是否为主资源，只有标记为主资源的才能在脚本中通过地址进行加载")]
        public bool IsMainAssets = false;
        [Help("地址设置")]
        public AssetAddressOperation AddressOperation = new AssetAddressOperation();
        [Help("Bundle设置")]
        public AssetBundleOperation BundleOperation = new AssetBundleOperation();

        public string[] GetAssetPaths()
        {
            if(string.IsNullOrEmpty(RootFolder) || filters.Count == 0)
            {
                return new string[0];
            }

            List<string> assetPaths = new List<string>();
            foreach (var filter in filters)
            {
                assetPaths.AddRange(filter.GetAssets(RootFolder));
            }
            return assetPaths.Distinct().ToArray();
        }
        
        public AssetDetail[] GetAssetDetails()
        {
            if(!IsMainAssets)
            {
                return new AssetDetail[0];
            }

            List<AssetDetail> details = new List<AssetDetail>();
            string[] assetPaths = GetAssetPaths();
            foreach (var assetPath in assetPaths)
            {
                AssetDetail detail = new AssetDetail()
                {
                    Address = GetAddressName(assetPath),
                    Path = assetPath,
                    Bundle = GetBundleName(assetPath),
                    IsScene = IsScene(assetPath),
                    Labels = AddressOperation.Labels,
                };
                details.Add(detail);
            }

            return details.ToArray();
        }

        private string GetAddressName(string assetPath)
        {
            if (AddressOperation.AddressMode == AssetAddressMode.FullPath)
                return assetPath;
            else if (AddressOperation.AddressMode == AssetAddressMode.FileName)
                return Path.GetFileName(assetPath);
            else if (AddressOperation.AddressMode == AssetAddressMode.FileNameWithoutExtension)
                return Path.GetFileNameWithoutExtension(assetPath);
            else
                return assetPath;
        }

        private string GetBundleName(string assetPath)
        {
            string bundleName = assetPath;
            if (BundleOperation.PackMode == AssetPackMode.Separate)
            {
                bundleName = assetPath.ReplaceSpecialCharacter("_");
            }
            else if (BundleOperation.PackMode == AssetPackMode.Together)
            {
                string rootFolder = Path.GetDirectoryName(assetPath).Replace("\\", "/");
                bundleName = rootFolder.ReplaceSpecialCharacter("_");
            }

            bundleName = bundleName.ToLower();
            if(BundleOperation.IsUseMD5)
            {
                bundleName = MD5Crypto.Md5(bundleName).ToLower();
            }

            return bundleName;
        }

        private bool IsScene(string assetPath)
        {
            if (Path.GetExtension(assetPath).ToLower() == ".unity")
            {
                return true;
            }
            return false;
        }

    }
}
