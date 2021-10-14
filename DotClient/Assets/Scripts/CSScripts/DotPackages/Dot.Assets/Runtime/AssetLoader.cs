using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public static class AssetLoader
    {
        public static void InitLoader()
        {

        }

        public static UnityObject LoadAsset(string assetPath)
        {
            return null;
        }

        public static UnityObject InstanceAsset(string assetPath)
        {
            return null;
        }

        public static UnityObject[] LoadAllAssets(string[] assetPaths)
        {
            return null;
        }

        public static UnityObject[] InstanceAllAssets(string[] assetPaths)
        {
            return null;
        }

        public static int LoadAssetAsync(string assetPath)
        {
            return -1;
        }

        public static int InstanceAssetAsync(string assetPath)
        {
            return -1;
        }

        public static int LoadAllAssetAsync(string[] assetPaths)
        {
            return -1;
        }

        public static int InstanceAllAssetAsync(string[] assetPaths)
        {
            return -1;
        }

        public static UnityObject Instantiate(string address, UnityObject asset)
        {
            return null;
        }

        public static void CancalAsync(int index)
        {

        }

        public static void UnloadUnusedAssetsAsync()
        {

        }

        public static void UnloadUnusedAssets(Action finishedCallback)
        {

        }
    }
}
