﻿using DotEngine.Asset;
using DotEngine.Asset.Datas;
using DotEngine.Utilities;
using DotEngine.Crypto;
using DotEditor.Asset.AssetAddress;
using DotEditor.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Asset.AssetPacker
{
    public static class AssetPackerUtil
    {
        public static AssetPackerConfig GetAssetPackerConfig()
        {
            AssetPackerConfig packerConfig = new AssetPackerConfig();

            AssetAddressGroup[] groups = AssetDatabaseUtility.FindInstances<AssetAddressGroup>();
            if(groups!=null && groups.Length>0)
            {
                foreach(var group in groups)
                {
                    AssetPackerGroupData groupData = new AssetPackerGroupData();
                    groupData.groupName = group.groupName;
                    groupData.isMain = group.isMain;
                    groupData.isPreload = group.isPreload;
                    groupData.isNeverDestroy = group.isNeverDestroy;

                    groupData.assetFiles = GetAssetsInGroup(group);

                    packerConfig.groupDatas.Add(groupData);
                }
            }
            packerConfig.Sort();

            AddAddressGroup(packerConfig);

            return packerConfig;
        }

        private static string AddressGroupName = "Default Address Group";
        private static void AddAddressGroup(AssetPackerConfig assetPackerConfig)
        {
            if (assetPackerConfig != null)
            {
                RemoveAddressGroup(assetPackerConfig);

                string[] assetPaths = AssetDatabaseUtility.FindAssets<AssetAddressConfig>();
                if (assetPaths == null || assetPaths.Length == 0)
                {
                    Debug.LogError("AssetPackUtil::AddAddressGroup->AssetAddressConfig is not found!");
                    return;
                }

                AssetPackerGroupData groupData = new AssetPackerGroupData()
                {
                    groupName = AddressGroupName,
                };
                AssetPackerAddressData addressData = new AssetPackerAddressData()
                {
                    assetAddress = AssetConst.ASSET_ADDRESS_CONFIG_NAME,
                    assetPath = assetPaths[0],
                    bundlePath = AssetConst.ASSET_ADDRESS_BUNDLE_NAME,
                    bundlePathMd5 = MD5Crypto.Md5(AssetConst.ASSET_ADDRESS_BUNDLE_NAME).ToLower(),
                };
                groupData.assetFiles.Add(addressData);

                assetPackerConfig.groupDatas.Add(groupData);
            }
        }

        private static void RemoveAddressGroup(AssetPackerConfig assetPackerConfig)
        {
            if (assetPackerConfig != null)
            {
                foreach (var group in assetPackerConfig.groupDatas)
                {
                    if (group.groupName == AddressGroupName)
                    {
                        assetPackerConfig.groupDatas.Remove(group);
                        break;
                    }
                }
            }
        }

        private static List<AssetPackerAddressData> GetAssetsInGroup(AssetAddressGroup groupData)
        {
            List<string> assets = new List<string>();
            foreach(var filter in groupData.filters)
            {
                string[] finderAssets = filter.Filter();
                if(finderAssets!=null && finderAssets.Length>0)
                {
                    assets.AddRange(finderAssets);
                }
            }
            assets = assets.Distinct().ToList();

            List<AssetPackerAddressData> addressDatas = new List<AssetPackerAddressData>();
            foreach(var asset in assets)
            {
                AssetPackerAddressData data = new AssetPackerAddressData();
                data.assetPath = asset;
                data.assetAddress = groupData.operation.GetAddressName(asset);
                data.bundlePath = groupData.operation.GetBundleName(asset);
                data.bundlePathMd5 = MD5Crypto.Md5(data.bundlePath).ToLower();
                data.labels = groupData.operation.GetLabels();

                addressDatas.Add(data);
            }
            return addressDatas;
        }

        private static string GetBundleBuildConfigPath()
        {
            return $"{Path.GetFullPath(".").Replace("\\", "/")}/AssetConfig/bundle_build_config.json";
        }

        public static BundleBuildConfig ReadBundleBuildConfig()
        {
            BundleBuildConfig bundlePackConfig = null;

            string configPath = GetBundleBuildConfigPath();
            if(File.Exists(configPath))
            {
                string configContent = File.ReadAllText(configPath);
                bundlePackConfig = JsonConvert.DeserializeObject<BundleBuildConfig>(configContent);
            }

            if(bundlePackConfig == null)
            {
                bundlePackConfig = new BundleBuildConfig();
            }
            return bundlePackConfig;
        }

        public static void WriteBundleBuildConfig(BundleBuildConfig config)
        {
            if (config == null)
            {
                return;
            }
            string configPath = GetBundleBuildConfigPath();
            string dir = Path.GetDirectoryName(configPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(configPath, json);
        }

        public static void ClearBundleNames(bool isShowProgressBar = false)
        {
            string[] bundleNames = AssetDatabase.GetAllAssetBundleNames();
            if (isShowProgressBar)
            {
                EditorUtility.DisplayProgressBar("Clear Bundle Names", "", 0.0f);
            }
            for (int i = 0; i < bundleNames.Length; i++)
            {
                if (isShowProgressBar)
                {
                    EditorUtility.DisplayProgressBar("Clear Bundle Names", bundleNames[i], i / (float)bundleNames.Length);
                }
                AssetDatabase.RemoveAssetBundleName(bundleNames[i], true);
            }
            if (isShowProgressBar)
            {
                EditorUtility.ClearProgressBar();
            }

            AssetDatabase.SaveAssets();
        }

        public static void SetAssetBundleNames(AssetPackerConfig assetPackerConfig, BundlePathFormatType bundlePathFormat,bool isShowProgressBar = false)
        {
            if (isShowProgressBar)
            {
                EditorUtility.DisplayProgressBar("Set Bundle Names", "", 0f);
            }

            List<AssetPackerAddressData> addressDatas = new List<AssetPackerAddressData>();
            assetPackerConfig.groupDatas.ForEach((groupData) =>
            {
                addressDatas.AddRange(groupData.assetFiles);
            });

            for (int i = 0; i < addressDatas.Count; ++i)
            {
                if (isShowProgressBar)
                {
                    EditorUtility.DisplayProgressBar("Set Bundle Names", addressDatas[i].assetPath, i / (float)addressDatas.Count);
                }

                string assetPath = addressDatas[i].assetPath;
                string bundlePath = addressDatas[i].bundlePath;
                if(bundlePathFormat == BundlePathFormatType.MD5)
                {
                    bundlePath = addressDatas[i].bundlePathMd5;
                }

                AssetImporter ai = AssetImporter.GetAtPath(assetPath);
                ai.assetBundleName = bundlePath;

                if (Path.GetExtension(assetPath).ToLower() == ".spriteatlas")
                {
                    SetSpriteBundleNameByAtlas(assetPath, bundlePath);
                }
            }

            if (isShowProgressBar)
            {
                EditorUtility.ClearProgressBar();
            }

            AssetDatabase.SaveAssets();
        }

        private static void SetSpriteBundleNameByAtlas(string atlasAssetPath, string bundlePath)
        {
            SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasAssetPath);
            if (atlas != null)
            {
                List<string> spriteAssetPathList = new List<string>();
                UnityObject[] objs = atlas.GetPackables();
                foreach (var obj in objs)
                {
                    if (obj.GetType() == typeof(Sprite))
                    {
                        spriteAssetPathList.Add(AssetDatabase.GetAssetPath(obj));
                    }
                    else if (obj.GetType() == typeof(DefaultAsset))
                    {
                        string folderPath = AssetDatabase.GetAssetPath(obj);
                        string[] assets = AssetDatabaseUtility.FindAssetInFolder<Sprite>(folderPath);
                        spriteAssetPathList.AddRange(assets);
                    }
                }
                spriteAssetPathList.Distinct();
                foreach (var path in spriteAssetPathList)
                {
                    AssetImporter ai = AssetImporter.GetAtPath(path);
                    ai.assetBundleName = bundlePath;
                }
            }
        }

        public static void PackAssetBundle(AssetPackerConfig packerConfig, BundleBuildConfig buildConfig)
        {
            IAssetBundlePacker bundlePacker = null;
            Type[] bundlePackerTypes = AssemblyUtility.GetDerivedTypes(typeof(IAssetBundlePacker));
            if(bundlePackerTypes!=null && bundlePackerTypes.Length>0)
            {
                bundlePacker = (IAssetBundlePacker)Activator.CreateInstance(bundlePackerTypes[0]);
            }

            if(bundlePacker == null)
            {
                Debug.LogError("AssetPackerUtil::PackAssetBundle->DoPackAssetBundle is null.");
                return;
            }

            if(string.IsNullOrEmpty(buildConfig.bundleOutputDir))
            {
                Debug.Log("AssetPackerUtil::PackAssetBundle->bundleOutputDir is null.");
                return;
            }

            string outputDir = $"{buildConfig.bundleOutputDir}/{buildConfig.buildTarget.ToString()}/assetbundles";
            if (buildConfig.cleanupBeforeBuild && Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }
            if (!Directory.CreateDirectory(outputDir).Exists)
            {
                Debug.LogError("AssetPackUitl::PackAssetBundle->Folder is not found. dir = " + outputDir);
                return;
            }

            AssetBundleConfig bundleConfig = bundlePacker.PackAssetBundle(packerConfig, buildConfig,outputDir);
            var json = JsonConvert.SerializeObject(bundleConfig, Formatting.Indented);
            string jsonFilePath = $"{outputDir}/{AssetConst.ASSET_BUNDLE_CONFIG_NAME}";
            File.WriteAllText(jsonFilePath, json);
        }

    }

}
