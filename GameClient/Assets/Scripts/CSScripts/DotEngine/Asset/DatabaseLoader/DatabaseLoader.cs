#if UNITY_EDITOR

using DotEngine.Asset.Datas;
using DotEngine.Pool;
using DotEngine.Log;
using System.Linq;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Asset
{
    /// <summary>
    /// 使用Database进行资源的加载，只能用于编辑器模式中
    /// </summary>
    public class DatabaseLoader : AAssetLoader
    {
        private ObjectPool<DatabaseAssetNode> assetNodePool = new ObjectPool<DatabaseAssetNode>();

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void DoInitUpdate()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(AssetAddressConfig).Name}");
            if(guids!=null && guids.Length>0)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                addressConfig = AssetDatabase.LoadAssetAtPath<AssetAddressConfig>(assetPath);
                if(addressConfig!=null)
                {
                    State = AssetLoaderState.Running;
                    return;
                }
            }
            LogUtil.Error(AssetConst.LOGGER_NAME, "Address config is null");
            State = AssetLoaderState.Error;
        }

        protected override void OnDataUpdate(AssetLoaderData data)
        {
            if (data.State == AssetLoaderDataState.Canceled || data.State == AssetLoaderDataState.Error)
            {
                foreach (var path in data.Paths)
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        assetNodeDic[path].ReleaseRef();
                    }
                }
            }
            else if (data.State == AssetLoaderDataState.Loading)
            {
                bool isComplete = true;
                bool isProgressChanged = false;
                for (int i = 0; i < data.Paths.Length; ++i)
                {
                    string path = data.Paths[i];
                    if (string.IsNullOrEmpty(path))
                    {
                        continue;
                    }

                    AAssetNode assetNode = assetNodeDic[path];
                    if (assetNode.IsDone())
                    {
                        data.DoComplete(i, assetNode);
                        assetNode.ReleaseRef();
                    }
                    else
                    {
                        float progress = GetAssetProgress(path);
                        data.DoProgress(i, progress);

                        isProgressChanged = true;
                        isComplete = false;
                    }
                }
                if (isProgressChanged)
                {
                    data.DoBatchProgress();
                }
                if (isComplete)
                {
                    data.DoBatchComplete();
                }
            }
        }

        private float GetAssetProgress(string assetPath)
        {
            DatabaseAsyncOperation operation = operations[assetPath] as DatabaseAsyncOperation;
            return operation.GetProgress();
        }

        protected override void OnOperationFinished(AAsyncOperation operation)
        {
            string assetPath = operation.AssetPath;
            if(assetNodeDic.TryGetValue(assetPath,out AAssetNode assetNode))
            {
                DatabaseAssetNode node = assetNode as DatabaseAssetNode;
                node.SetAsset(operation.GetAsset());
            }
        }

        protected internal override void UnloadUnusedAsset()
        {
            string[] assetPaths = assetNodeDic.Keys.ToArray();
            foreach (var assetPath in assetPaths)
            {
                UnloadAsset(assetPath);
            }
        }

        protected override void UnloadAsset(string assetPath)
        {
            if (assetNodeDic.TryGetValue(assetPath, out AAssetNode assetNode) && !assetNode.IsAlive())
            {
                assetNode.Unload();
                assetNodeDic.Remove(assetPath);

                assetNodePool.Release(assetNode as DatabaseAssetNode);
            }
        }

        protected override void StartLoadingData(AssetLoaderData data)
        {
            for (int i = 0; i < data.Paths.Length; ++i)
            {
                string assetPath = data.Paths[i];
                if (!assetNodeDic.TryGetValue(assetPath, out AAssetNode assetNode))
                {
                    assetNode = CreateAssetNode(assetPath);
                }
                assetNode.RetainRef();
            }
        }

        private DatabaseAssetNode CreateAssetNode(string assetPath)
        {
            DatabaseAssetNode assetNode = assetNodePool.Get();
            assetNode.InitNode(assetPath);

            assetNodeDic.Add(assetPath, assetNode);

            DatabaseAsyncOperation operation = new DatabaseAsyncOperation(assetPath);
            operations.Add(assetPath, operation);

            return assetNode;
        }

        /// <summary>
        /// 实例化资源
        /// </summary>
        /// <param name="address"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        protected internal override UnityObject InstantiateAsset(string address, UnityObject asset)
        {
            if(asset!=null)
            {
                return UnityObject.Instantiate(asset);
            }
            return null;
        }
    }
}
#endif