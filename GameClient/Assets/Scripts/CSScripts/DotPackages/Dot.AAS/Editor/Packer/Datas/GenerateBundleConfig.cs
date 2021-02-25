using DotEditor.AAS.Filters;
using DotEngine.GUIExt.NativeDrawer;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.AAS.Packer
{
    [CreateAssetMenu(fileName ="gen_bundle_config",menuName ="AAS/Gen Bundle Config")]
    [CustomDrawerEditor]
    public class GenerateBundleConfig : ScriptableObject
    {
        [MultilineText]
        public string desc = string.Empty;
        
        public AssetFilter filter = new AssetFilter();

        public bool isMainAsset = false;
        public bool isNeedPreload = false;
        public bool isNeverDestroy = false;

        public AssetBundleAssignRule bundleAssignRule = new AssetBundleAssignRule();
        [VisibleIf("isMainAsset")]
        public AssetAddressAssignRule addressAssignRule = new AssetAddressAssignRule();
        [VisibleIf("isMainAsset")]
        public AssetLabelAssignRule labelAssignRule = new AssetLabelAssignRule();

        public GeneratedBundleData[] GetDatas()
        {
            string[] assetPathes = filter.Filter();
            if(assetPathes == null || assetPathes.Length == 0)
            {
                return null;
            }

            List<GeneratedBundleData> dataList = new List<GeneratedBundleData>();
            foreach(var assetPath in assetPathes)
            {
                GeneratedBundleData data = new GeneratedBundleData();
                data.path = assetPath;
                data.bundle = bundleAssignRule.GetBundleName(assetPath);

                data.isMainAsset = isMainAsset;
                data.isNeedPreload = isNeedPreload;
                data.isNeverDestroy = isNeverDestroy;

                if(isMainAsset)
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
