using DotEngine.Assets.Operations;
using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{

    public class BundleLoader : ALoader
    {
        private ItemPool<BundleLoadAsyncOperation> bundleLoadAsyncOpeartionPool = new ItemPool<BundleLoadAsyncOperation>();
        private ItemPool<BundleAssetLoadAsyncOperation> assetLoadAsyncOperationPool = new ItemPool<BundleAssetLoadAsyncOperation>();
        private ItemPool<BundleNode> bundleNodePool = new ItemPool<BundleNode>();

        private string bundleRootDir;
        private BundleDetailConfig bundleDetailConfig = null;

        private Dictionary<string, BundleLoadAsyncOperation> createAsyncOperationDic = new Dictionary<string, BundleLoadAsyncOperation>();
        private Dictionary<string, BundleAssetLoadAsyncOperation> loadAsyncOperationDic = new Dictionary<string, BundleAssetLoadAsyncOperation>();
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
            return createAsyncOperationDic.Count + loadAsyncOperationDic.Count < AsyncOperationMaxCount;
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
            string[] assetPaths = request.paths;
            var result = request.result;
            for (int i = 0; i < assetPaths.Length; i++)
            {
                if(result.IsDoneAt(i))
                {
                    continue;
                }
                string assetPath = assetPaths[i];
                AssetNode assetNode = assetNodeDic[assetPath];
                if(assetNode.IsLoaded())
                {
                    if(request.isInstance)
                    {
                        request.SetUObject(i, assetNode.CreateInstance());
                    }else
                    {
                        request.SetUObject(i, assetNode.GetAsset());
                    }
                    assetNode.ReleaseRef();
                    continue;
                }
                string bundlePath = assetDetailConfig.GetBundleByPath(assetPath);
                BundleNode bundleNode = bundleNodeDic[bundlePath];
                if(bundleNode.IsDone)
                {
                    
                }else
                {

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
            if(loadAsyncOperationDic.TryGetValue(assetPath,out var loadAsyncOperation))
            {
                progress += loadAsyncOperation.Progress;
            }
            progressCount += 1;
            return progress / progressCount;
        }

        private float GetAsycBundleProgress(string bundlePath)
        {
            float progress = 0.0f;
            BundleNode mainBundleNode = bundleNodeDic[bundlePath];
            if(mainBundleNode.IsDone)
            {
                progress = 1.0f;
            }else if(createAsyncOperationDic.TryGetValue(bundlePath,out var operation))
            {
                progress = operation.Progress;
            }else
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
            BundleLoadAsyncOperation createAsyncOperation = LoadBundleAsync(bundlePath);
            createAsyncOperationDic.Add(bundlePath, createAsyncOperation);

            string[] dependBundlePaths = bundleDetailConfig.GetDependencies(bundlePath);
            for (int i = 0; i < dependBundlePaths.Length; i++)
            {
                BundleNode dependBundleNode = GetAsyncBundleNode(dependBundlePaths[i]);
                mainBundleNode.BindDepend(dependBundleNode);
            }

            bundleNodeDic.Add(bundlePath, mainBundleNode);

            return mainBundleNode;
        }


        private BundleLoadAsyncOperation LoadBundleAsync(string bundlePath)
        {
            BundleLoadAsyncOperation createAsyncOperation = bundleLoadAsyncOpeartionPool.Get();
            createAsyncOperation.DoInitilize(bundlePath, bundleRootDir);
            createAsyncOperation.OnOperationComplete = OnBundleCreated;

            return createAsyncOperation;
        }
        private void OnBundleCreated(AAsyncOperation operation)
        {
            BundleLoadAsyncOperation createAsyncOperation = (BundleLoadAsyncOperation)operation;
            string bundlePath = createAsyncOperation.Path;
            if (bundleNodeDic.TryGetValue(bundlePath, out var node))
            {
                node.Bundle = (AssetBundle)createAsyncOperation.GetAsset();
            }
            createAsyncOperationDic.Remove(bundlePath);
            bundleLoadAsyncOpeartionPool.Release(createAsyncOperation);
        }

        private BundleAssetLoadAsyncOperation LoadAssetFromBundleAsync(BundleNode bundleNode, string assetPath)
        {
            BundleAssetLoadAsyncOperation loadAsyncOperation = assetLoadAsyncOperationPool.Get();
            loadAsyncOperation.DoInitilize(assetPath);
            loadAsyncOperation.OnOperationComplete = OnAssetFromBundleCreated;

            return loadAsyncOperation;
        }

        private void OnAssetFromBundleCreated(AAsyncOperation operation)
        {
            BundleAssetLoadAsyncOperation loadAsyncOperation = (BundleAssetLoadAsyncOperation)operation;
            string assetPath = operation.Path;
            if(assetNodeDic.TryGetValue(assetPath,out var assetNode))
            {
                assetNode.SetAsset(loadAsyncOperation.GetAsset());
            }
            loadAsyncOperationDic.Remove(assetPath);
            assetLoadAsyncOperationPool.Release(loadAsyncOperation);
        }
        #endregion
    }
}
