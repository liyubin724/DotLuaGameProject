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

namespace DotEditor.AAS.Packer
{
    public static class BundlePackerUtility
    {
        public static bool PackBundle(
            BuildTarget buildTarget, 
            bool isBundleAsMD5 = false,
            bool isForceRebuild = false)
        {
            string outputFolderPath = $"AssetBundle/{buildTarget}";
            string tmpFolderPath = $"{outputFolderPath}-Temp";
            GeneratedBundleData[] datas = (from config in AssetDatabaseUtility.FindInstances<GenerateBundleConfig>()
                                            from data in config.GetDatas()
                                            where data!=null
                                            select data).ToArray();

            return PackBundle(buildTarget, outputFolderPath, tmpFolderPath, datas, isBundleAsMD5, isForceRebuild);
        }

        public static bool PackBundle(
            BuildTarget buildTarget, 
            string outputFolderPath, 
            string tmpFolderPath,
            GeneratedBundleData[] datas, 
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

            BundleDescriptionConfig config = CreateBundleConfig(datas, buildResults);
            string jsonStr = JsonConvert.SerializeObject(config);
            File.WriteAllText(outputFolderPath+"/" + BundleConst.TXT_BUNDLE_DETAIL_FILE, jsonStr);
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

        private static AssetBundleBuild CreateBundleBuild(string bundleName, GeneratedBundleData[] datas)
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

        private static IBundleBuildContent CreateBundleBuildContent(GeneratedBundleData[] datas,bool isBundleAsMd5)
        {
            Dictionary<string, List<GeneratedBundleData>> assetInBundleDic = new Dictionary<string, List<GeneratedBundleData>>();
            foreach(var data in datas)
            {
                if(!assetInBundleDic.TryGetValue(data.bundle,out var list))
                {
                    list = new List<GeneratedBundleData>();
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

        private static BundleDescriptionConfig CreateBundleConfig(GeneratedBundleData[] buildDatas, IBundleBuildResults buildResults)
        {
            List<BundleDescriptionConfig.AssetDetail> assetDetails = new List<BundleDescriptionConfig.AssetDetail>();
            foreach(var data in buildDatas)
            {
                if(!data.isMainAsset)
                {
                    continue;
                }
                BundleDescriptionConfig.AssetDetail assetDetail = new BundleDescriptionConfig.AssetDetail();
                assetDetail.path = data.path;
                assetDetail.bundle = data.bundle;
                assetDetail.address = data.address;
                if(data.labels!=null && data.labels.Length>0)
                {
                    assetDetail.labels = new string[data.labels.Length];
                    Array.Copy(data.labels, assetDetail.labels, data.labels.Length);
                }
                assetDetails.Add(assetDetail);
            }

            List<BundleDescriptionConfig.BundleDetail> bundleDetails = new List<BundleDescriptionConfig.BundleDetail>();
            foreach(var kvp in buildResults.BundleInfos)
            {
                bundleDetails.Add(new BundleDescriptionConfig.BundleDetail
                {
                    fileName = kvp.Value.FileName,
                    hash = kvp.Value.Hash.ToString(),
                    crc = kvp.Value.Crc,
                    dependencies = kvp.Value.Dependencies,
                    md5 = string.Empty
                });
            }

            return new BundleDescriptionConfig()
            {
                assetDetails = assetDetails.ToArray(),
                bundleDetails = bundleDetails.ToArray(),
            };

        }
    }


}
