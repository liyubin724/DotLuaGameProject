using DotEngine.Assets;
using DotEngine.Core.Extensions;
using DotEngine.NativeDrawer.Decorator;
using DotEngine.NativeDrawer.Property;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Space = DotEngine.NativeDrawer.Decorator.SpaceAttribute;

namespace DotEditor.Asset.AssetAddress
{

    public enum AssetPackMode
    {
        Together,
        Separate,
    }

    public class AssetBuildGroup : ScriptableObject
    {
        [Help("此配置是否生效")]
        public bool Enable = true;

        [Space]
        [Help("资源所属分组的名称")]
        public string GroupName = "Asset Address Group";
        [OpenFolderPath]
        public string RootFolder = "Assets";
        [Help("根据指定的配置进行资源的筛选")]
        public List<AssetBuildFilter> filters = new List<AssetBuildFilter>();

        [Space]
        [Help("是否为主资源，只有标记为主资源的才能在脚本中通过地址进行加载")]
        public bool isMain = false;
        [Help("地址设置")]
        public AssetAddressOperation AddressOperation = new AssetAddressOperation();

        [Help("AB打包方式.\nTogether:将筛选出来的所有的资源打到一个包内\nSeparate:将筛选出来的资源每个都单独打包")]
        [EnumButton]
        public AssetPackMode packMode = AssetPackMode.Together;

        public string[] GetAssets()
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
            List<AssetDetail> details = new List<AssetDetail>();
            string[] assetPaths = GetAssets();
            foreach (var assetPath in assetPaths)
            {
                AssetDetail detail = new AssetDetail()
                {
                    Address = AddressOperation.GetAddress(assetPath),
                    Path = assetPath,
                    Bundle = GetBundleName(assetPath),
                    IsScene = IsScene(assetPath),
                    Labels = AddressOperation.GetLabels(),
                };
                details.Add(detail);
            }

            return details.ToArray();
        }

        private string GetBundleName(string assetPath)
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
