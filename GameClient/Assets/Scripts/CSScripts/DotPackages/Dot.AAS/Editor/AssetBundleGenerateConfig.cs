using DotEditor.Utilities;
using DotEngine.GUIExt.NativeDrawer;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.AAS
{
    [CreateAssetMenu(fileName ="asset_bundle_gen_config",menuName ="AseetBundle/Gen Config")]
    [NativeDrawerEditor]
    public class AssetBundleGenerateConfig : ScriptableObject
    {
        [MultilineText]
        public string desc = string.Empty;

        public string rootFolder = string.Empty;
        public bool isIncludeChildFolder = false;

        public AssetFolderFilterRule folderFilterRule = new AssetFolderFilterRule();
        public AssetNameFilterRule nameFilterRule = new AssetNameFilterRule();

        public AssetBundleAssignRule bundleAssignRule = new AssetBundleAssignRule();
        public AssetAddressAssignRule addressAssignRule = new AssetAddressAssignRule();
        public AssetLabelAssignRule labelAssignRule = new AssetLabelAssignRule();

        public AssetBundleBuildData[] GetDatas()
        {
            if(string.IsNullOrEmpty(rootFolder))
            {
                return null;
            }

            string[] assetPathes = DirectoryUtility.GetAsset(rootFolder, isIncludeChildFolder, true);
            if(assetPathes == null || assetPathes.Length == 0)
            {
                return null;
            }

            List<AssetBundleBuildData> dataList = new List<AssetBundleBuildData>();
            foreach(var assetPath in assetPathes)
            {
                if(!folderFilterRule.IsValid(assetPath) || !nameFilterRule.IsValid(assetPath))
                {
                    continue;
                }

                AssetBundleBuildData data = new AssetBundleBuildData()
                {
                    path = assetPath,
                    bundle = bundleAssignRule.GetBundleName(assetPath),
                    address = addressAssignRule.GetAddress(assetPath),
                    labels = labelAssignRule.GetLabels(assetPath)
                };
                dataList.Add(data);
            }

            return dataList.ToArray();
        }
    }
}


