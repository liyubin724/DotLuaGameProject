using DotEditor.Asset.Group;
using DotEditor.Utilities;
using DotEngine.Assets;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEngine;
using UnityEngine.Build.Pipeline;

namespace DotEditor.Asset.AssetPacker
{
    public static class AssetPackerUtil
    {
        public static PackerData GetPackerData()
        {
            AssetGroupCreater[] groups = AssetDatabaseUtility.FindInstances<AssetGroupCreater>();
            List<AssetResult> results = new List<AssetResult>();
            foreach (var group in groups)
            {
                results.AddRange(group.GetResults());
            }

            PackerData packerData = new PackerData();

            var groupNames = results.GroupBy((result) => result.GroupName).ToList();
            foreach (var gn in groupNames)
            {
                PackerGroupData groupData = new PackerGroupData();
                groupData.GroupName = gn.Key;
                packerData.groupDatas.Add(groupData);

                var bundleNames = gn.GroupBy((result) => result.Bundle).ToList();
                foreach(var bn in bundleNames)
                {
                    PackerBundleData bundleData = new PackerBundleData();
                    bundleData.BundlePath = bn.Key;
                    groupData.bundleDatas.Add(bundleData);

                    foreach(var ad in bn)
                    {
                        PackerAssetData assetData = new PackerAssetData();
                        assetData.Address = ad.Address;
                        assetData.Path = ad.Path;
                        assetData.IsMainAsset = ad.IsMainAsset;
                        assetData.IsScene = ad.IsScene;
                        assetData.Labels = ad.Labels;

                        bundleData.assetDatas.Add(assetData);
                    }
                }
            }

            return packerData;
        }

        public static BundleBuildData ReadBuildData()
        {
            BundleBuildData bundlePackConfig = null;

            string configPath = GetBundleBuildDataFilePath();
            if (File.Exists(configPath))
            {
                string configContent = File.ReadAllText(configPath);
                bundlePackConfig = JsonConvert.DeserializeObject<BundleBuildData>(configContent);
            }

            if (bundlePackConfig == null)
            {
                bundlePackConfig = new BundleBuildData();
            }
            return bundlePackConfig;
        }

        public static void WriteBuildData(BundleBuildData buildData)
        {
            if (buildData == null)
            {
                return;
            }
            string configPath = GetBundleBuildDataFilePath();
            string dir = Path.GetDirectoryName(configPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var json = JsonConvert.SerializeObject(buildData, Formatting.Indented);
            File.WriteAllText(configPath, json);
        }

        private static string GetBundleBuildDataFilePath()
        {
            return $"{Path.GetFullPath(".").Replace("\\", "/")}/AssetConfig/bundle_build_config.json";
        }

        public static void BuildAssetBundles(PackerData packerData, BundleBuildData buildData, string outputDir)
        {
            var manifest = PackAssetBundle(packerData, buildData, outputDir);
            if(manifest==null)
            {
                Debug.LogError("PackAssetBundle Failed");
                return;
            }

            var bundleDetailConfig = CreateBundleDetailConfig(manifest);
            string detailConfigFilePath = $"{outputDir}/{AssetConst.GetBundleDetailConfigFile()}";
            BundleDetailConfig.WriteToFile(bundleDetailConfig, detailConfigFilePath);
        }

        private static CompatibilityAssetBundleManifest PackAssetBundle(PackerData packerData, BundleBuildData buildData, string outputDir)
        {
            List<AssetBundleBuild> bundleBuilds = new List<AssetBundleBuild>();
            foreach (var group in packerData.groupDatas)
            {
                foreach (var bundle in group.bundleDatas)
                {
                    AssetBundleBuild build = new AssetBundleBuild();
                    build.assetBundleName = bundle.BundlePath;
                    build.assetNames = (from asset in bundle.assetDatas select asset.Path).ToArray();
                    bundleBuilds.Add(build);
                }
            }
            var manifest = CompatibilityBuildPipeline.BuildAssetBundles(outputDir, bundleBuilds.ToArray(), buildData.GetBundleOptions(), buildData.GetBuildTarget());
            return manifest;
        }

        private static BundleDetailConfig CreateBundleDetailConfig(CompatibilityAssetBundleManifest manifest)
        { 
            List<BundleDetail> details = new List<BundleDetail>();
            string[] bundles = manifest.GetAllAssetBundles();
            foreach (var bundlePath in bundles)
            {
                BundleDetail detail = new BundleDetail();
                detail.Path = bundlePath;
                detail.Hash = manifest.GetAssetBundleHash(bundlePath).ToString();
                detail.CRC = manifest.GetAssetBundleCrc(bundlePath).ToString();
                detail.Dependencies = manifest.GetAllDependencies(bundlePath);

                details.Add(detail);
            }
            BundleDetailConfig detailConfig = new BundleDetailConfig();
            detailConfig.Details = details.ToArray();

            return detailConfig;
        }
    }

}
