//using DotEditor.Utilities;
//using DotEngine.Asset;
//using DotEngine.Asset.Datas;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;
//using static DotEngine.Asset.Datas.AssetAddressConfig;

//namespace DotEditor.Asset.AssetAddress
//{
//    public static class AssetAddressUtil
//    {
        

//        [MenuItem("Game/Asset/Build Address", priority = 1)]
//        public static void BuildAssetAddressConfig()
//        {
//            AssetBuildGroup[] groups = AssetDatabaseUtility.FindInstances<AssetBuildGroup>();
//            if(groups!=null && groups.Length>0)
//            {
//                AssetAddressConfig config = GetOrCreateConfig();
//                config.Clear();

//                foreach(var group in groups)
//                {
//                    UpdateConfigByGroup(group);
//                }
//                config.Reload();

//                EditorUtility.SetDirty(config);

//                AssetDatabase.SaveAssets();
//            }
//        }

//        public static void UpdateConfigByGroup(AssetBuildGroup group)
//        {
//            if(!group.isMain ||!group.Enable)
//            {
//                return;
//            }
//            AssetAddressConfig config = GetOrCreateConfig();
//            Dictionary<string, AssetAddressData> dataDic = new Dictionary<string, AssetAddressData>();
//            foreach(var d in config.addressDatas)
//            {
//                dataDic.Add(d.assetPath, d);
//            }

//            foreach (var filter in group.filters)
//            {
//                string[] assetPaths = filter.Filter();
//                if (assetPaths != null && assetPaths.Length > 0)
//                {
//                    foreach (var assetPath in assetPaths)
//                    {
//                        if(!dataDic.TryGetValue(assetPath,out AssetAddressData addressData))
//                        {
//                            addressData = group.AddressOperation.GetAddressData(assetPath);
//                            dataDic.Add(assetPath, addressData);
//                        }else
//                        {
//                            group.AddressOperation.UpdateAddressData(addressData);
//                        }
//                        addressData.isPreload = group.isPreload;
//                        addressData.isNeverDestroy = group.isNeverDestroy;
//                    }
//                }
//            }

//            config.addressDatas = dataDic.Values.ToArray();
//            config.Reload();
//            EditorUtility.SetDirty(config);
//        }

//        public static AssetAddressConfig GetOrCreateConfig()
//        {
//            AssetAddressConfig[] configs = AssetDatabaseUtility.FindInstances<AssetAddressConfig>();

//            AssetAddressConfig config;
//            if (configs == null || configs.Length == 0)
//            {
//                string defaultAssetPath = $"Assets/{AssetConst.ASSET_ADDRESS_CONFIG_NAME}";
                
//                config = ScriptableObject.CreateInstance<AssetAddressConfig>();

//                AssetDatabase.CreateAsset(config, defaultAssetPath);
//                AssetDatabase.ImportAsset(defaultAssetPath);
//            }else
//            {
//                config = configs[0];
//            }
//            return config;
//        }
//    }
//}
