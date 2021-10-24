using DotEngine.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{

    public class BundleLoader : ALoader
    {
        private ItemPool<BundleLoadAsyncOperation> bundleOperationPool = new ItemPool<BundleLoadAsyncOperation>();
        private ItemPool<BundleAssetLoadAsyncOperation> assetOperationPool = new ItemPool<BundleAssetLoadAsyncOperation>();
        private ItemPool<BundleNode> bundleNodePool = new ItemPool<BundleNode>();

        private string bundleRootDir;
        private BundleDetailConfig bundleDetailConfig = null;

        private int operationCount = 0;
        private Dictionary<string, BundleLoadAsyncOperation> bundleOperationDic = new Dictionary<string, BundleLoadAsyncOperation>();
        private Dictionary<string, BundleAssetLoadAsyncOperation> assetOperationDic = new Dictionary<string, BundleAssetLoadAsyncOperation>();
        private List<AAsyncOperation> operations = new List<AAsyncOperation>();

        private Dictionary<string, BundleNode> bundleNodeDic = new Dictionary<string, BundleNode>();

        #region initialize
        protected override void OnInitialize(params object[] values)
        {
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

        #region unload unused
        protected override void OnUnloadUnusedAssets()
        {

        }
        protected override bool OnUnloadUnusedAssetsUpdate()
        {
            return true;
        }
        #endregion

        protected override UnityObject LoadAsset(string assetPath)
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

        protected override void StartRequest(AsyncRequest request)
        {
            string[] assetPaths = request.paths;
            for (int i = 0; i < assetPaths.Length; i++)
            {
                string assetPath = assetPaths[i];
                AssetNode assetNode = GetAsyncAssetNode(assetPath);
                assetNode.RetainRef();
            }
        }

        protected override void UpdateRequest(AsyncRequest request)
        {
            if (operations.Count > 0 && operationCount < OperationMaxCount)
            {
                int diffCount = OperationMaxCount - operationCount;
                for (int i = 0; i < operations.Count; i++)
                {
                    if(!operations[i].IsRunning)
                    {
                        operations[i].DoStart();
                        diffCount--;
                    }
                    if(diffCount<=0)
                    {
                        break;
                    }
                }
            }

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
                }else if(assetNode.IsLoading())
                {
                    string bundlePath = assetDetailConfig.GetBundleByPath(assetPath);
                    BundleNode bundleNode = bundleNodeDic[bundlePath];
                    if (bundleNode.IsDone && !assetOperationDic.ContainsKey(assetPath))
                    {
                        CreateAssetOperation(bundleNode, assetPath);
                    }
                    request.SetProgress(i, GetAsyncAssetProgress(assetPath));
                }else
                {
                    throw new Exception();
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

        protected override void EndRequest(AsyncRequest request)
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
            BundleLoadAsyncOperation bundleOperation = bundleOperationPool.Get();
            bundleOperation.DoInitilize(bundlePath, bundleRootDir);
            bundleOperation.OnOperationComplete = OnBundleCreated;

            bundleOperationDic.Add(bundlePath, bundleOperation);
            operations.Add(bundleOperation);
        }
        private void OnBundleCreated(AAsyncOperation operation)
        {
            BundleLoadAsyncOperation bundleOperation = (BundleLoadAsyncOperation)operation;
            string bundlePath = bundleOperation.Path;
            if (bundleNodeDic.TryGetValue(bundlePath, out var node))
            {
                node.Bundle = (AssetBundle)bundleOperation.GetAsset();
            }
            bundleOperationDic.Remove(bundlePath);
            bundleOperationPool.Release(bundleOperation);

            operations.Remove(bundleOperation);
        }

        private void CreateAssetOperation(BundleNode bundleNode, string assetPath)
        {
            BundleAssetLoadAsyncOperation assetOperation = assetOperationPool.Get();
            assetOperation.DoInitilize(assetPath);
            assetOperation.OnOperationComplete = OnAssetFromBundleCreated;

            assetOperationDic.Add(assetPath, assetOperation);
            operations.Add(assetOperation);
        }

        private void OnAssetFromBundleCreated(AAsyncOperation operation)
        {
            BundleAssetLoadAsyncOperation assetOperation = (BundleAssetLoadAsyncOperation)operation;
            string assetPath = operation.Path;
            if(assetNodeDic.TryGetValue(assetPath,out var assetNode))
            {
                assetNode.SetAsset(assetOperation.GetAsset());
            }
            assetOperationDic.Remove(assetPath);
            assetOperationPool.Release(assetOperation);

            operations.Remove(assetOperation);
        }
        #endregion
    }
}
