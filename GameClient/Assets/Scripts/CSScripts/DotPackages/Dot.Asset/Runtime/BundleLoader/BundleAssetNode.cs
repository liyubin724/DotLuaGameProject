using System;
using System.Collections.Generic;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Asset
{
    /// <summary>
    /// 对Unity资源的缓存
    /// </summary>
    public class BundleAssetNode : AAssetNode
    {
        //资源所在的AssetBundle
        private BundleNode bundleNode = null;
        //采用弱引用的形式缓存的对资源的引用
        private WeakReference assetWeakRef = null;
        //生成的资源的实例
        private List<WeakReference> instanceWeakRefs = new List<WeakReference>();

        internal bool IsScene { get; private set; } = false;

        internal void InitNode(string assetPath,bool isScene,BundleNode node)
        {
            InitNode(assetPath);
            IsScene = isScene;
            bundleNode = node;
            bundleNode.RetainRef();
            if(isScene)
            {
                bundleNode.SetUsedByScene(true);
            }
        }
        /// <summary>
        /// 从AssetBundle中读取资源
        /// </summary>
        /// <returns></returns>
        protected internal override UnityObject GetAsset()
        {
            UnityObject asset = bundleNode.GetAsset(AssetPath);
            if(assetWeakRef == null)
            {
                assetWeakRef = new WeakReference(asset);
            }else
            {
                assetWeakRef.Target = asset;
            }
            return asset;
        }

        /// <summary>
        /// 从AssetBundle中获取资源后，并进行实例化。场景资源无法实例化
        /// </summary>
        /// <returns></returns>
        protected internal override UnityObject GetInstance()
        {
            UnityObject asset = bundleNode.GetAsset(AssetPath);
            if (asset == null)
            {
                return null;
            }
            if (bundleNode.IsScene)
            {
                DebugLog.Error(AssetConst.LOGGER_NAME,"AssetNode::GetInstance->bundle is scene.can't Instance it");
                return null;
            }

            UnityObject instance = UnityObject.Instantiate(asset);
            AddInstance(instance);

            return instance;
        }

        protected internal override UnityObject GetInstance(UnityObject uObj)
        {
            if(uObj!=null)
            {
                UnityObject instance = UnityObject.Instantiate(uObj);
                AddInstance(instance);

                return instance;
            }
            return null;
        }

        protected internal override bool IsAlive()
        {
            if(IsNeverDestroy)
            {
                return true;
            }
            if(refCount>0)
            {
                return true;
            }
            if(IsScene)
            {
                return true;
            }
            if(assetWeakRef!=null && !IsNull(assetWeakRef.Target))
            {
                return true;
            }

            foreach (var instance in instanceWeakRefs)
            {
                if (!IsNull(instance.Target))
                {
                    return true;
                }
            }

            return false;
        }

        protected internal override bool IsDone() => bundleNode.IsDone;

        

        protected internal override void Unload()
        {
            if (bundleNode != null)
            {
                bundleNode.ReleaseRef();
                bundleNode = null;
            }
            assetWeakRef = null;
            foreach (var asset in instanceWeakRefs)
            {
                asset.Target = null;
            }
        }

        private void AddInstance(UnityObject uObj)
        {
            bool isSet = false;
            for (int i = 0; i < instanceWeakRefs.Count; ++i)
            {
                if (IsNull(instanceWeakRefs[i].Target))
                {
                    instanceWeakRefs[i].Target = uObj;
                    isSet = true;
                    break;
                }
            }

            if (!isSet)
            {
                instanceWeakRefs.Add(new WeakReference(uObj, false));
            }
        }

        private bool IsNull(SystemObject sysObj)
        {
            if (sysObj == null || sysObj.Equals(null))
            {
                return true;
            }

            return false;
        }

        public override void OnRelease()
        {
            Unload();
            IsScene = false;
            base.OnRelease();
        }
    }
}
