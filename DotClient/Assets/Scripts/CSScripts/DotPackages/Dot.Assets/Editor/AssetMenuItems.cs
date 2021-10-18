using DotEditor.Asset.AssetAddress;
using DotEditor.Utilities;
using DotEngine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Assets
{
    public static class AssetMenuItems
    {
        [MenuItem("Game/Asset/Create Address Group", priority = 0)]
        [MenuItem("Assets/Create/Asset/Create Address Group", priority = 0)]
        public static void CreateGroupAsset()
        {
            string dirPath = SelectionUtility.GetSelectionDir();
            if(!string.IsNullOrEmpty(dirPath))
            {
                string filePath = $"{dirPath}/asset_group.asset";
                filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);
                var config = ScriptableObject.CreateInstance<AssetBuildGroup>();
                config.RootFolder = dirPath;
                AssetDatabase.CreateAsset(config, filePath);
                AssetDatabase.ImportAsset(filePath);
            }
            else
            {
                Debug.LogError("AssetMenuItems::CreateGroupAsset->The dir is not found");
            }
        }

        [MenuItem("Game/Asset/Build Asset Detail Config", priority = 1)]
        [MenuItem("Assets/Create/Asset/Build Asset Detail Config", priority = 1)]
        public static void BuildAssetAddressConfig()
        {
            AssetBuildGroup[] groups = AssetDatabaseUtility.FindInstances<AssetBuildGroup>();
            if (groups != null && groups.Length > 0)
            {
                AssetDetailConfig config = new AssetDetailConfig();

                List<AssetDetail> details = new List<AssetDetail>();
                foreach(var group in groups)
                {
                    details.AddRange(group.GetAssetDetails());
                }
                config.Details = details.ToArray();
                string assetFilePath = AssetConst.GetAssetDetailConfigPathInProject();
                
                AssetDetailConfig.WriteToFile(config, PathUtility.GetDiskPath(assetFilePath));
                AssetDatabase.ImportAsset(assetFilePath);
            }
        }
    }
}
