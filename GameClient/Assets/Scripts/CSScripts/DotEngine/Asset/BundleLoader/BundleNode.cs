using DotEngine.Log;
using DotEngine.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Asset
{
    /// <summary>
    /// AssetBundle的缓存
    /// </summary>
    public class BundleNode : IObjectPoolItem
    {
        private bool isDone = false;
        private AssetBundle assetBundle = null;
        private List<BundleNode> dependNodes = new List<BundleNode>();

        /// <summary>
        /// 加载完毕后，设置AssetBundle
        /// </summary>
        /// <param name="bundle"></param>
        internal void SetBundle(AssetBundle bundle)
        {
            isDone = true;
            assetBundle = bundle;
        }

        /// <summary>
        /// 添加Bundle依赖的其它Bundle
        /// </summary>
        /// <param name="node"></param>
        internal void AddDepend(BundleNode node)
        {
            if(!dependNodes.Contains(node))
            {
                dependNodes.Add(node);
                node.RetainRef();
            }
        }

        private int refCount = 0;
        internal int RefCount { get => refCount; }
        internal void RetainRef() => ++refCount;
        internal void ReleaseRef()
        {
            --refCount;
            if(refCount == 0)
            {
                foreach(var node in dependNodes)
                {
                    node.ReleaseRef();
                }
            }
        }

        internal bool IsDone
        {
            get
            {
                if(!isDone)
                {
                    return false;
                }

                foreach(var node in dependNodes)
                {
                    if(!node.isDone)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        //用于判断当前Bundle是否被场景使用，如果被场景使用，则卸载时将会使用Unload(false)
        private bool isUsedByScene = false;
        internal void SetUsedByScene(bool isScene)
        {
            isUsedByScene = isScene;
            foreach(var bn in dependNodes)
            {
                bn.isUsedByScene = isScene;
            }
        }

        internal bool IsScene
        {
            get 
            {
                if (assetBundle != null)
                {
                    return assetBundle.isStreamedSceneAssetBundle;
                }
                else if (!isDone)
                {
                    LogUtil.LogError(AssetConst.LOGGER_NAME,"BundleNode::IsScene->AssetBundle has not been loaded,you should call IsDone at first");
                }
                else
                {
                    LogUtil.LogError(AssetConst.LOGGER_NAME, "BundleNode::IsScene->AssetBundle Load failed");
                }
                return false;
            }
        }

        internal UnityEngine.Object GetAsset(string assetPath)
        {
            return IsScene ? assetBundle : assetBundle?.LoadAsset(assetPath);
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            if (assetBundle != null)
            {
                assetBundle.Unload(!isUsedByScene);
                assetBundle = null;
            }
            isUsedByScene = false;
            isDone = false;
            dependNodes.Clear();
            refCount = 0;
        }

        public void OnNew()
        {
        }
    }
}
