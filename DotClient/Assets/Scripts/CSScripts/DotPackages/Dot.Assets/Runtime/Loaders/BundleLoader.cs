using DotEngine.Assets.Operations;
using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEngine.Assets.Loaders
{
    public class BundleLoader : ALoader
    {
        private ItemPool<BundleNode> bundleNodePool = new ItemPool<BundleNode>();
        private ItemPool<AssetNode> assetNodePool = new ItemPool<AssetNode>();

        private Dictionary<string, BundleNode> bundleNodeDic = new Dictionary<string, BundleNode>();
        private Dictionary<string, AssetNode> assetNodeDic = new Dictionary<string, AssetNode>();

        private Dictionary<string, BundleCreateAsyncOperation> createOperationDic = new Dictionary<string, BundleCreateAsyncOperation>();
        private Dictionary<string, BundleLoadAsyncOperation> loadAsyncOperationDic = new Dictionary<string, BundleLoadAsyncOperation>();

        private BundleDetailConfig bundleDetailConfig;
        private string bundleRootPath;

        public BundleNode CreateBundleNode(string bundlePath)
        {
            BundleNode node = bundleNodePool.Get();
            return null;
        }

        //private BundleNode LoadMainBundleAsync(string bundlePath)
        //{
        //    if(bundleNodeDic.TryGetValue(bundlePath,out BundleNode mainBundleNode))
        //    {
        //        return mainBundleNode;
        //    }
        //    BundleNode node = LoadBundleAsync(bundlePath);
        //    string[] depends = bundleDetailConfig.GetDependencies(bundlePath);
        //    foreach (var d in depends)
        //    {
        //        if(!bundleNodeDic.TryGetValue(d,out BundleNode dependBundleNode))
        //        {
        //            dependBundleNode = LoadBundleAsync(d);
        //        }
        //        node.AddDepend(dependBundleNode);
        //    }

        //    return node;
        //}

        //private BundleNode LoadBundleAsync(string bundlePath)
        //{
        //    BundleNode node = bundleNodePool.Get();
        //    node.Path = bundlePath;
        //    node.IsNeverDestroy = bundleDetailConfig.IsNeverDestroy(bundlePath);

        //    bundleNodeDic.Add(bundlePath, node);

        //    BundleCreateAsyncOperation createOperation = new BundleCreateAsyncOperation();
        //    createOperation.DoInitilize(bundlePath);
        //    createOperationDic.Add(bundlePath, createOperation);

        //    return node;
        //}

        //private BundleNode LoadMainBundle(string bundlePath)
        //{
        //    if (bundleNodeDic.TryGetValue(bundlePath, out BundleNode mainBundleNode))
        //    {
        //        if(mainBundleNode.IsDone)
        //        {
        //            return mainBundleNode;
        //        }else
        //        {
        //            Debug.LogError("");
        //            return null;
        //        }
        //    }

        //    BundleNode node = LoadBundle(bundlePath);
        //    string[] depends = bundleDetailConfig.GetDependencies(bundlePath);
        //    foreach (var d in depends)
        //    {
        //        if (!bundleNodeDic.TryGetValue(d, out BundleNode dependBundleNode))
        //        {
        //            dependBundleNode = LoadBundleAsync(d);
        //        }
        //        node.AddDepend(dependBundleNode);
        //    }

        //    return node;
        //}

        //private BundleNode LoadBundle(string bundlePath)
        //{
        //    BundleNode node = bundleNodePool.Get();
        //    node.Path = bundlePath;
        //    node.IsNeverDestroy = bundleDetailConfig.IsNeverDestroy(bundlePath);

        //    bundleNodeDic.Add(bundlePath, node);

        //    AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
        //    node.Bundle = bundle;
        //    return node;
        //}

        //protected string GetBundleFullPath(string bundlePath)
        //{
        //    return $"{bundleRootPath}/{bundlePath}";
        //}




















        public BundleLoader(AssetDetailConfig detailConfig) : base(detailConfig)
        {
        }

        public override void DoInitialize(AssetDetailConfig detailConfig, Action completedCallback, params object[] objects)
        {
            base.DoInitialize(detailConfig, completedCallback, objects);
            bundleDetailConfig = objects[0] as BundleDetailConfig;
            string[] preloadPaths = bundleDetailConfig.GetPreloadPaths();

        }

        protected override void DoInitializeUpdate()
        {

        }

        protected override UnityEngine.Object InstanceAsset(string assetAddress, UnityEngine.Object uObject)
        {
            throw new NotImplementedException();
        }

        protected override UnityEngine.Object LoadAsset(string assetAddress)
        {
            throw new NotImplementedException();
        }
    }
}
