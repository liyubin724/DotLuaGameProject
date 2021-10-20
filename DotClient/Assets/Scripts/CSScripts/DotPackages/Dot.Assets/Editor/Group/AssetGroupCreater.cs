using DotEngine.Core.Extensions;
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

    public enum BundlePackMode
    {
        Together,
        Separate,
    }

    public enum BundleNamedMode
    {
        FullPath = 0,
        FileName,
        PathAsName,
    }

    [Serializable]
    public class AssetBundleOperation
    {
        [Help("AB打包方式.\nTogether:将筛选出来的所有的资源打到一个包内\nSeparate:将筛选出来的资源每个都单独打包")]
        [EnumButton]
        public BundlePackMode PackMode = BundlePackMode.Separate;

        [Help("设定Bundle的命名规则")]
        [EnumButton]
        public BundleNamedMode NamedMode = BundleNamedMode.FullPath;
    }

    public class AssetGroupCreater : ScriptableObject
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
            if (string.IsNullOrEmpty(RootFolder) || filters.Count == 0)
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
            if (BundleOperation.NamedMode == BundleNamedMode.FullPath)
            {
                bundleName = assetPath;
                if (BundleOperation.PackMode == BundlePackMode.Together)
                {
                    bundleName = Path.GetDirectoryName(assetPath).Replace("\\", "/");
                }
            }
            else if (BundleOperation.NamedMode == BundleNamedMode.FileName)
            {
                if (BundleOperation.PackMode == BundlePackMode.Separate)
                {
                    bundleName = Path.GetFileName(assetPath);
                }
                else if (BundleOperation.PackMode == BundlePackMode.Together)
                {
                    string rootFolder = Path.GetDirectoryName(assetPath).Replace("\\", "/");
                    bundleName = rootFolder.Substring(rootFolder.LastIndexOf("/")+1);
                }
            }
            else if (BundleOperation.NamedMode == BundleNamedMode.PathAsName)
            {
                if (BundleOperation.PackMode == BundlePackMode.Separate)
                {
                    bundleName = assetPath.Replace("/", "_");
                }
                else if (BundleOperation.PackMode == BundlePackMode.Together)
                {
                    string rootFolder = Path.GetDirectoryName(assetPath).Replace("\\", "/");
                    bundleName = rootFolder.Replace("/", "_");
                }
            }

            return bundleName.ReplaceSpecialCharacter("_").ToLower();
        }

        private bool IsScene(string assetPath)
        {
            if (Path.GetExtension(assetPath).ToLower() == ".unity")
            {
                return true;
            }
            return false;
        }

        private string[] GetLabels(string assetPath)
        {
            return AddressOperation.Labels;
        }

        public AssetResult[] GetResults()
        {
            if (!Enable)
            {
                return new AssetResult[0];
            }

            List<AssetResult> results = new List<AssetResult>();
            string[] assetPaths = GetAssetPaths();
            foreach (var assetPath in assetPaths)
            {
                AssetResult result = new AssetResult();
                result.GroupName = GroupName;
                result.IsMainAsset = IsMainAssets;
                if (IsMainAssets)
                {
                    result.Address = GetAddressName(assetPath);
                }
                result.Bundle = GetBundleName(assetPath);
                result.Path = assetPath;
                result.IsScene = IsScene(assetPath);
                result.Labels = GetLabels(assetPath);

                results.Add(result);
            }

            return results.ToArray();
        }
    }
}
