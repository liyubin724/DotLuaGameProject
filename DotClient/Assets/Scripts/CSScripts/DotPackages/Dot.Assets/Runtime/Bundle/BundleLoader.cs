using DotEngine.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class BundleLoader : ALoader
    {
        private ItemPool<BundleAsyncOperation> bundleOperationPool = new ItemPool<BundleAsyncOperation>();
        private ItemPool<BundleAssetAsyncOperation> assetOperationPool = new ItemPool<BundleAssetAsyncOperation>();
        private ItemPool<BundleNode> bundleNodePool = new ItemPool<BundleNode>();

        private string bundleRootDir;
        private BundleDetailConfig bundleDetailConfig = null;

        private Dictionary<string, BundleAsyncOperation> bundleOperationDic = new Dictionary<string, BundleAsyncOperation>();
        private Dictionary<string, BundleAssetAsyncOperation> assetOperationDic = new Dictionary<string, BundleAssetAsyncOperation>();

        private Dictionary<string, BundleNode> bundleNodeDic = new Dictionary<string, BundleNode>();

        #region initialize
        public override void DoInitialize(AssetDetailConfig detailConfig, OnInitFinished initCallback, params object[] values)
        {
            base.DoInitialize(detailConfig, initCallback, values);
            bundleRootDir = (string)(values[0]);
            bundleDetailConfig = (BundleDetailConfig)(values[1]);
        }

        protected override bool OnInitializeUpdate(float deltaTime)
        {
            if (bundleDetailConfig == null || string.IsNullOrEmpty(bundleRootDir))
            {
                State = LoaderState.Error;
            }
            else
            {
                State = LoaderState.Initialized;
            }
            return true;
        }
        #endregion

        #region unload
        protected override bool OnUnloadAssetsUpdate()
        {
            bool isFinished = true;
            var keys = bundleNodeDic.Keys;
            foreach(var key in keys)
            {
                BundleNode node = bundleNodeDic[key];
                if(!node.IsInUsing())
                {
                    isFinished = false;

                    bundleNodeDic.Remove(key);
                    bundleNodePool.Release(node);
                }
            }

            return isFinished;
        }
        #endregion

        protected override UnityObject RequestAssetSync(string assetPath)
        {
            var bundlePath = assetDetailConfig.GetBundleByPath(assetPath);
            BundleNode bundleNode = GetBundleNode(bundlePath);
            bundleNode.RetainRef();

            return LoadAssetFromBundle(bundleNode, assetPath);
        }

        protected override bool CanStartRequest()
        {
            return operationCount < OperationMaxCount;
        }

        protected override void OnAsyncRequestStart(AsyncRequest request)
        {
        }

        protected override void OnAsyncRequestUpdate(AsyncRequest request)
        {
            var result = request.result;
            string[] assetPaths = request.paths;
            for (int i = 0; i < assetPaths.Length; i++)
            {
                if(result.IsDoneAt(i))
                {
                    continue;
                }

                string assetPath = assetPaths[i];
                AssetNode assetNode = assetNodeDic[assetPath];
                if (assetNode.IsLoaded())
                {
                    if (request.isInstance)
                    {
                        request.SetUObject(i, assetNode.CreateInstance());
                    }
                    else
                    {
                        request.SetUObject(i, assetNode.GetAsset());
                    }
                    assetNode.ReleaseRef();
                    continue;
                }else
                {
                    string bundlePath = assetDetailConfig.GetBundleByPath(assetPath);
                    BundleNode bundleNode = bundleNodeDic[bundlePath];
                    if (bundleNode.IsDone && !assetOperationDic.ContainsKey(assetPath))
                    {
                        CreateAssetOperation(bundleNode, assetPath);
                    }
                    request.SetProgress(i, GetAsyncAssetProgress(assetPath));
                }
            }
            if(result.IsDone())
            {
                if(request.isInstance)
                {
                    request.state = AsyncState.InstanceFinished;
                }else
                {
                    request.state = AsyncState.LoadFinished;
                }
            }
        }

        protected override void OnAsyncRequestEnd(AsyncRequest request)
        {

        }

        private float GetAsyncAssetProgress(string assetPath)
        {
            AssetNode assetNode = assetNodeDic[assetPath];
            if(assetNode.IsLoaded())
            {
                return 1.0f;
            }
            if(!assetNode.IsLoading())
            {
                return 0.0f;
            }
            string bundlePath = assetDetailConfig.GetBundleByPath(assetPath);
            float progress = GetAsycBundleProgress(bundlePath);
            int progressCount = 1;
            string[] dependBundlePaths = bundleDetailConfig.GetDependencies(bundlePath);
            if(dependBundlePaths!=null && dependBundlePaths.Length>0)
            {
                progressCount += dependBundlePaths.Length;
                foreach(var dbp in dependBundlePaths)
                {
                    progress += GetAsycBundleProgress(dbp);
                }
            }
            if(assetOperationDic.TryGetValue(assetPath,out var loadAsyncOperation))
            {
                progress += loadAsyncOperation.Progress;
            }
            progressCount += 1;
            return progress / progressCount;
        }

        private float GetAsycBundleProgress(string bundlePath)
        {
            BundleNode mainBundleNode = bundleNodeDic[bundlePath];
            float progress;
            if (mainBundleNode.IsDone)
            {
                progress = 1.0f;
            }
            else if (bundleOperationDic.TryGetValue(bundlePath, out var operation))
            {
                progress = operation.Progress;
            }
            else
            {
                progress = 0.0f;
            }
            return progress;
        }

        protected override void OnCreateAssetNodeSync(AssetNode assetNode)
        {
        }

        protected override void OnCreateAssetNodeAsync(AssetNode assetNode)
        {
            string bundlePath = assetDetailConfig.GetBundleByPath(assetNode.Path);
            BundleNode bundleNode = GetAsyncBundleNode(bundlePath);
            bundleNode.RetainRef();
        }

        protected override void OnReleaseAssetNode(AssetNode assetNode)
        {
            string bundlePath = assetDetailConfig.GetBundleByPath(assetNode.Path);
            BundleNode bundleNode = GetAsyncBundleNode(bundlePath);
            bundleNode.ReleaseRef();
        }

        #region Sync
        private BundleNode GetBundleNode(string bundlePath)
        {
            if (bundleNodeDic.TryGetValue(bundlePath, out var bundleNode))
            {
                if (bundleNode.IsDone)
                {
                    return bundleNode;
                }
                throw new Exception("");
            }

            BundleNode mainBundleNode = bundleNodePool.Get();

            string[] dependBundlePaths = bundleDetailConfig.GetDependencies(bundlePath);
            for (int i = 0; i < dependBundlePaths.Length; i++)
            {
                BundleNode dependBundleNode = GetBundleNode(dependBundlePaths[i]);
                mainBundleNode.BindDepend(dependBundleNode);
            }

            AssetBundle bundle = LoadBundle(bundlePath);
            mainBundleNode.Bundle = bundle;

            bundleNodeDic.Add(bundlePath, mainBundleNode);

            return mainBundleNode;
        }


        private AssetBundle LoadBundle(string bundlePath)
        {
            string fullBundlePath = $"{bundleRootDir}/{bundlePath}";
            AssetBundle bundle = AssetBundle.LoadFromFile(fullBundlePath);
            return bundle;
        }

        private UnityObject LoadAssetFromBundle(BundleNode bundleNode, string assetPath)
        {
            if (bundleNode.Bundle != null)
            {
                return bundleNode.Bundle.LoadAsset(assetPath);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Async
        private AssetNode GetAsyncAssetNode(string assetPath)
        {
            if (assetNodeDic.TryGetValue(assetPath, out var node))
            {
                return node;
            }

            node = assetNodePool.Get();
            node.DoInitialize(assetPath);
            node.State = NodeState.Loading;

            string bundlePath = assetDetailConfig.GetBundleByPath(assetPath);
            BundleNode bundleNode = GetAsyncBundleNode(bundlePath);
            bundleNode.RetainRef();

            assetNodeDic.Add(assetPath, node);
            return node;
        }

        private BundleNode GetAsyncBundleNode(string bundlePath)
        {
            if (bundleNodeDic.TryGetValue(bundlePath, out var bundleNode))
            {
                return bundleNode;
            }

            BundleNode mainBundleNode = bundleNodePool.Get();
            mainBundleNode.State = NodeState.Loading;

            CreateBundleOperation(bundlePath);

            string[] dependBundlePaths = bundleDetailConfig.GetDependencies(bundlePath);
            for (int i = 0; i < dependBundlePaths.Length; i++)
            {
                BundleNode dependBundleNode = GetAsyncBundleNode(dependBundlePaths[i]);
                mainBundleNode.BindDepend(dependBundleNode);
            }

            bundleNodeDic.Add(bundlePath, mainBundleNode);

            return mainBundleNode;
        }

        private void CreateBundleOperation(string bundlePath)
        {
            BundleAsyncOperation bundleOperation = bundleOperationPool.Get();
            bundleOperation.DoInitilize(bundlePath, bundleRootDir);
            bundleOperation.OnOperationComplete = OnBundleCreated;

            bundleOperationDic.Add(bundlePath, bundleOperation);
            operationLDic.Add(bundlePath, bundleOperation);
        }
        private void OnBundleCreated(AAsyncOperation operation)
        {
            BundleAsyncOperation bundleOperation = (BundleAsyncOperation)operation;
            string bundlePath = bundleOperation.Path;
            if (bundleNodeDic.TryGetValue(bundlePath, out var node))
            {
                node.Bundle = (AssetBundle)bundleOperation.GetAsset();
            }
            bundleOperationDic.Remove(bundlePath);
            bundleOperationPool.Release(bundleOperation);

            operationLDic.Remove(bundlePath);
        }

        private void CreateAssetOperation(BundleNode bundleNode, string assetPath)
        {
            BundleAssetAsyncOperation assetOperation = assetOperationPool.Get();
            assetOperation.DoInitilize(assetPath);
            assetOperation.OnOperationComplete = OnAssetFromBundleCreated;

            assetOperationDic.Add(assetPath, assetOperation);
            operationLDic.Add(assetPath, assetOperation);
        }

        private void OnAssetFromBundleCreated(AAsyncOperation operation)
        {
            BundleAssetAsyncOperation assetOperation = (BundleAssetAsyncOperation)operation;
            string assetPath = operation.Path;
            if(assetNodeDic.TryGetValue(assetPath,out var assetNode))
            {
                assetNode.SetAsset(assetOperation.GetAsset());
            }
            assetOperationDic.Remove(assetPath);
            operationLDic.Remove(assetPath);
            assetOperationPool.Release(assetOperation);
        }

        protected override void OnAsyncRequestCancel(AsyncRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
