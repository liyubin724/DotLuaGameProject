using DotEditor.Utilities;
using DotEngine.AAS;
using DotEngine.Crypto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;
using static DotEngine.AAS.AssetConfig;

namespace DotEditor.AAS.Packer
{
    public static class BundlePackerUtility
    {
        public static bool PackBundle(
            BuildTarget buildTarget, 
            bool isBundleAsMD5 = false,
            bool isForceRebuild = false)
        {
            string outputFolderPath = $"{BundleConst.RootPath}/{buildTarget}";
            string tmpFolderPath = $"{outputFolderPath}-Temp";
            GeneratedAssetData[] datas = (from config in AssetDatabaseUtility.FindInstances<GenerateBundleConfig>()
                                            from data in config.GetDatas()
                                            where data!=null
                                            select data).ToArray();

            return PackBundle(buildTarget, outputFolderPath, tmpFolderPath, datas, isBundleAsMD5, isForceRebuild);
        }

        public static bool PackBundle(
            BuildTarget buildTarget, 
            string outputFolderPath, 
            string tmpFolderPath,
            GeneratedAssetData[] datas, 
            bool isBundleAsMD5 = false,
            bool isForceRebuild = false)
        {
            if (!Directory.Exists(tmpFolderPath))
            {
                Directory.CreateDirectory(tmpFolderPath);
            }
            else if (isForceRebuild)
            {
                Directory.Delete(tmpFolderPath,true);
            }
            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }
            else if (isForceRebuild)
            {
                Directory.Delete(outputFolderPath,true);
            }

            IBundleBuildParameters buildParameters = CreateBundleBuildParameters(tmpFolderPath, outputFolderPath, buildTarget);
            IBundleBuildContent buildContent = CreateBundleBuildContent(datas, isBundleAsMD5);
            IBundleBuildResults buildResults;

            ReturnCode result = ContentPipeline.BuildAssetBundles(buildParameters, buildContent, out buildResults);
            if (result < 0)
            {
                return false;
            }

            AssetConfig assetConfig = CreateAssetConfig(datas);
            SaveAssetConfig(assetConfig, BundleConst.AssetConfigPath);

            BundleConfig config = CreateBundleConfig(buildResults);
            SaveBundleConfig(config, BundleConst.BundleConfigPath);

            return true;
        }


        private static IBundleBuildParameters CreateBundleBuildParameters(string tmpFolderPath,string outputFolderPath,BuildTarget buildTarget)
        {
            BuildTargetGroup buildTargetGroup = BuildTargetGroup.Unknown;
            if(buildTarget == BuildTarget.StandaloneWindows)
            {
                buildTargetGroup = BuildTargetGroup.Standalone;
            }else if(buildTarget == BuildTarget.Android)
            {
                buildTargetGroup = BuildTargetGroup.Android;
            }else if(buildTarget == BuildTarget.iOS)
            {
                buildTargetGroup = BuildTargetGroup.iOS;
            }
            if(buildTargetGroup == BuildTargetGroup.Unknown)
            {
                return null;
            }

            BundleBuildParameters buildParams = new BundleBuildParameters(buildTarget, buildTargetGroup, outputFolderPath);
            //buildParams.BundleCompression = UnityEngine.BuildCompression.Uncompressed;
            buildParams.TempOutputFolder = tmpFolderPath;
            return buildParams;
        }

        private static AssetBundleBuild CreateBundleBuild(string bundleName, GeneratedAssetData[] datas)
        {
            string[] assetPaths = new string[datas.Length];
            string[] assetAddresses = new string[datas.Length];
            for(int i=0;i<datas.Length;++i)
            {
                assetPaths[i] = datas[i].path;
                assetAddresses[i] = string.IsNullOrEmpty(datas[i].address) ? string.Empty : datas[i].address;
            }

            return new AssetBundleBuild()
            {
                assetBundleName = bundleName,
                assetBundleVariant = string.Empty,
                addressableNames = assetAddresses,
                assetNames = assetPaths,
            };
        }

        private static IBundleBuildContent CreateBundleBuildContent(GeneratedAssetData[] datas,bool isBundleAsMd5)
        {
            Dictionary<string, List<GeneratedAssetData>> assetInBundleDic = new Dictionary<string, List<GeneratedAssetData>>();
            foreach(var data in datas)
            {
                if(!assetInBundleDic.TryGetValue(data.bundle,out var list))
                {
                    list = new List<GeneratedAssetData>();
                    assetInBundleDic.Add(data.bundle, list);
                }
                list.Add(data);
            }

            List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
            foreach(var kvp in assetInBundleDic)
            {
                string bundleName = isBundleAsMd5 ? MD5Crypto.Md5(kvp.Key).ToLower() : kvp.Key.ToLower();
                assetBundleBuilds.Add(CreateBundleBuild(bundleName, kvp.Value.ToArray()));
            }

            IBundleBuildContent bundleBuildContent = new BundleBuildContent(assetBundleBuilds);
            return bundleBuildContent;
        }

        private static BundleConfig CreateBundleConfig(IBundleBuildResults buildResults)
        {
            List<BundleConfig.BundleDetail> bundleDetails = new List<BundleConfig.BundleDetail>();
            foreach(var kvp in buildResults.BundleInfos)
            {
                bundleDetails.Add(new BundleConfig.BundleDetail
                {
                    fileName = kvp.Value.FileName,
                    hash = kvp.Value.Hash.ToString(),
                    crc = kvp.Value.Crc,
                    dependencies = kvp.Value.Dependencies,
                    md5 = string.Empty
                });
            }

            return new BundleConfig()
            {
                bundleDetails = bundleDetails.ToArray(),
            };
        }

        public static void SaveBundleConfig(BundleConfig config, string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = BundleConst.AssetConfigPath;
            }
            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string jsonStr = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(filePath, jsonStr);
        }

        public static AssetConfig CreateAssetConfig(GeneratedAssetData[] assetDatas)
        {
            if (assetDatas == null)
            {
                assetDatas = (from config in AssetDatabaseUtility.FindInstances<GenerateBundleConfig>()
                              from data in config.GetDatas()
                              where data != null
                              select data).ToArray();
            }

            List<AssetDetail> assets = new List<AssetDetail>();
            foreach (var assetData in assetDatas)
            {
                if (assetData.isMainAsset)
                {
                    AssetDetail assetDetail = new AssetDetail()
                    {
                        path = assetData.path,
                        bundle = assetData.bundle,
                        address = assetData.address,
                        isPreload = assetData.isNeedPreload,
                        isNeverDestroy = assetData.isNeverDestroy,
                    };
                    assetDetail.labels = new string[assetData.labels.Length];
                    Array.Copy(assetData.labels, assetDetail.labels, assetData.labels.Length);
                    assets.Add(assetDetail);
                }
            }

            return new AssetConfig()
            {
                assetDetails = assets.ToArray(),
            };
        }

        public static void SaveAssetConfig(AssetConfig config,string filePath = null)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                filePath = BundleConst.AssetConfigPath;
            }
            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string jsonStr = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(filePath, jsonStr);
        }
    }


}
