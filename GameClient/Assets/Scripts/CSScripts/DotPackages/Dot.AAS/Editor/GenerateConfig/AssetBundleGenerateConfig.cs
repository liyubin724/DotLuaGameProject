using DotEditor.Utilities;
using DotEngine.GUIExt.NativeDrawer;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.AAS
{
    [CreateAssetMenu(fileName ="asset_bundle_gen_config",menuName ="AseetBundle/Gen Config")]
    [CustomDrawerEditor]
    public class AssetBundleGenerateConfig : ScriptableObject
    {
        [MultilineText]
        public string desc = string.Empty;
        [OpenFolderPath]
        public string rootFolder = string.Empty;
        public bool isIncludeChildFolder = false;
        public bool usedAsAddress = false;

        public AssetFolderFilterRule folderFilterRule = new AssetFolderFilterRule();
        public AssetNameFilterRule nameFilterRule = new AssetNameFilterRule();

        public AssetBundleAssignRule bundleAssignRule = new AssetBundleAssignRule();
        [VisibleIf("usedAsAddress")]
        public AssetAddressAssignRule addressAssignRule = new AssetAddressAssignRule();
        [VisibleIf("usedAsAddress")]
        public AssetLabelAssignRule labelAssignRule = new AssetLabelAssignRule();

        public AssetBundleBuildData[] GetDatas()
        {
            if(string.IsNullOrEmpty(rootFolder))
            {
                return null;
            }

            string[] assetPathes = DirectoryUtility.GetAsset(PathUtility.GetDiskPath(rootFolder), isIncludeChildFolder, true);
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

                AssetBundleBuildData data = new AssetBundleBuildData();
                data.path = assetPath;
                data.bundle = bundleAssignRule.GetBundleName(assetPath);
                data.usedAsAddress = usedAsAddress;
                if(usedAsAddress)
                {
                    data.address = addressAssignRule.GetAddress(assetPath);
                    data.labels = labelAssignRule.GetLabels(assetPath);
                }
                dataList.Add(data);
            }

            return dataList.ToArray();
        }
    }
}
