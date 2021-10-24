using DotEngine.Pool;
using System;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class AssetNode : IPoolItem
    {
        internal string Path { get; private set; }
        internal NodeState State { get; set; } = NodeState.None;

        private WeakReference<UnityObject> assetRef = null;
        private List<WeakReference<UnityObject>> instanceRefs = new List<WeakReference<UnityObject>>();

        private int refCount = 0;
        private UnityObject cachedAsset = null;
        internal void RetainRef()
        {
            ++refCount;
            if (cachedAsset == null && assetRef != null && assetRef.TryGetTarget(out var target))
            {
                cachedAsset = target;
            }
        }
        internal void ReleaseRef()
        {
            --refCount;
            if (refCount == 0 && cachedAsset != null)
            {
                cachedAsset = null;
            }
            if (refCount < 0)
            {
                throw new Exception();
            }
        }

        internal void DoInitialize(string path)
        {
            Path = path;
            State = NodeState.None;
        }

        internal bool IsLoaded()
        {
            if(State == NodeState.LoadError)
            {
                return true;
            }else if(State == NodeState.Loaded)
            {
                if(IsAssetValid())
                {
                    return true;
                }else
                {
                    return false;
                }
            }

            return false;
        }

        internal bool IsLoading()
        {
            return State == NodeState.Loading;
        }

        internal bool IsAssetValid()
        {
            return assetRef != null && assetRef.TryGetTarget(out var _);
        }

        internal UnityObject GetAsset()
        {
            if (assetRef == null)
            {
                return null;
            }
            if (assetRef.TryGetTarget(out UnityObject target))
            {
                return target;
            }
            return null;
        }
        internal void SetAsset(UnityObject asset)
        {
            if (asset == null)
            {
                State = NodeState.LoadError;
                assetRef = null;
            }
            else
            {
                State = NodeState.Loaded;
                if (assetRef == null)
                {
                    assetRef = new WeakReference<UnityObject>(asset);
                }
                else
                {
                    assetRef.SetTarget(asset);
                }
                if (refCount > 0 && cachedAsset == null)
                {
                    cachedAsset = asset;
                }
            }
        }

        internal UnityObject CreateInstance()
        {
            UnityObject asset = GetAsset();
            if (asset != null)
            {
                UnityObject instance = UnityObject.Instantiate(asset);
                bool isAdded = false;
                for (int i = 0; i < instanceRefs.Count; i++)
                {
                    if (!instanceRefs[i].TryGetTarget(out var _))
                    {
                        isAdded = true;
                        instanceRefs[i].SetTarget(instance);
                        break;
                    }
                }
                if (!isAdded)
                {
                    instanceRefs.Add(new WeakReference<UnityObject>(instance));
                }
                return instance;
            }
            return null;
        }
        internal void DestroyInstance(UnityObject uObject)
        {
            for (int i = 0; i < instanceRefs.Count; i++)
            {
                WeakReference<UnityObject> weakRef = instanceRefs[i];
                if (weakRef.TryGetTarget(out var target) && target == uObject)
                {
                    weakRef.SetTarget(null);
                    break;
                }
            }
        }

        public bool IsInUsing()
        {
            if (IsAssetValid())
            {
                return true;
            }
            for (int i = 0; i < instanceRefs.Count; i++)
            {
                if (instanceRefs[i].TryGetTarget(out var _))
                {
                    return true;
                }
            }
            return false;
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            Path = null;
            State = NodeState.None;
            refCount = 0;
            assetRef = null;
            instanceRefs.Clear();
        }
    }
}
