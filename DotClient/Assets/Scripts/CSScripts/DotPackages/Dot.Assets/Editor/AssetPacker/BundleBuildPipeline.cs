﻿using DotEditor.Asset.AssetPacker;
using DotEngine.Assets;
using DotEngine.Crypto;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEngine.Build.Pipeline;

public class BundleBuildPipeline : IAssetBundlePacker
{
    private static BundleDetailConfig ConvertToBundleConfig(AssetPackerConfig assetPackerConfig, CompatibilityAssetBundleManifest manifest, string outputDir)
    {
        //AssetBundleConfig assetBundleConfig = new AssetBundleConfig();

        //List<AssetBundleDetail> bundleDetails = new List<AssetBundleDetail>();

        //string[] bundles = manifest.GetAllAssetBundles();
        //foreach (var bundlePath in bundles)
        //{
        //    AssetBundleDetail detail = new AssetBundleDetail();
        //    detail.path = bundlePath;
        //    detail.hash = manifest.GetAssetBundleHash(bundlePath).ToString();
        //    detail.crc = manifest.GetAssetBundleCrc(bundlePath).ToString();
        //    detail.dependencies = manifest.GetAllDependencies(bundlePath);

        //    string bundleDiskPath = $"{outputDir}/{bundlePath}";
        //    detail.md5 = MD5Crypto.Md5File(bundleDiskPath);

        //    bundleDetails.Add(detail);
        //}
        //assetBundleConfig.details = bundleDetails.ToArray();

        //return assetBundleConfig;
        return null;
    }

    public BundleDetailConfig PackAssetBundle(AssetPackerConfig packerConfig, BundleBuildConfig buildConfig, string outputDir)
    {
        var manifest = CompatibilityBuildPipeline.BuildAssetBundles(outputDir, buildConfig.GetBundleOptions(), buildConfig.GetBuildTarget());
        return ConvertToBundleConfig(packerConfig, manifest, outputDir);
    }
}