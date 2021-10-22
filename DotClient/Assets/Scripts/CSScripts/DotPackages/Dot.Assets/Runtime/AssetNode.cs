using DotEngine.Pool;
using System;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class AssetNode : IPoolItem
    {
        private string path;
        private WeakReference<UnityObject> mainAsset = null;
        private List<WeakReference<UnityObject>> instances = new List<WeakReference<UnityObject>>();

        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }

        public UnityObject GetAsset()
        {
            if (mainAsset == null)
            {
                return null;
            }
            if (mainAsset.TryGetTarget(out UnityObject target))
            {
                return target;
            }
            return null;
        }

        public void SetAsset(UnityObject asset)
        {
            if (mainAsset == null)
            {
                mainAsset = new WeakReference<UnityObject>(asset);
            }
            else
            {
                mainAsset.SetTarget(asset);
            }
        }

        public UnityObject CreateInstance()
        {
            UnityObject asset = GetAsset();
            if (asset != null)
            {
                UnityObject instance = UnityObject.Instantiate(asset);
                instances.Add(new WeakReference<UnityObject>(instance));
                return instance;
            }
            return null;
        }

        public void DestroyInstance(UnityObject uObject)
        {
            for (int i = instances.Count - 1; i >= 0; --i)
            {
                WeakReference<UnityObject> weakRef = instances[i];
                if (weakRef == null || !weakRef.TryGetTarget(out UnityObject target))
                {
                    instances.RemoveAt(i);
                }else if(target == uObject)
                {
                    UnityObject.Destroy(target);
                    instances.RemoveAt(i);
                    break;
                }
            }
        }

        public bool IsAssetValid()
        {
            return !string.IsNullOrEmpty(path) && GetAsset() != null;
        }

        public bool IsInUnused()
        {
            if(IsAssetValid())
            {
                return false;
            }
            for (int i = instances.Count-1; i >=0; --i)
            {
                WeakReference<UnityObject> weakRef = instances[i];
                if(weakRef ==null || !weakRef.TryGetTarget(out UnityObject target))
                {
                    instances.RemoveAt(i);
                    return false;
                }
                if(target == null)
                {
                    instances.RemoveAt(i);
                    return false;
                }
            }
            return instances.Count == 0;
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            path = null;
            mainAsset = null;
            instances.Clear();
        }
    }
}
