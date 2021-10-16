using DotEngine.Pool;
using System;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Loaders
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

        public bool IsAssetValid()
        {
            return !string.IsNullOrEmpty(path) && GetAsset() != null;
        }

        public bool CanDestroy()
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

        public UnityObject GetAsset()
        {
            if(mainAsset ==  null)
            {
                return null;
            }
            if(mainAsset.TryGetTarget(out UnityObject target))
            {
                if(target == null)
                {
                    return null;
                }
                return target;
            }
            return null;
        }

        public void SetAsset(UnityObject asset)
        {
            if(mainAsset == null)
            {
                mainAsset = new WeakReference<UnityObject>(asset);
            }
            else
            {
                mainAsset.SetTarget(asset);
            }
        }

        public UnityObject GetInstance()
        {
            UnityObject asset = GetAsset();
            if(asset!=null)
            {
                UnityObject instance = UnityObject.Instantiate(asset);
                instances.Add(new WeakReference<UnityObject>(instance));
                return instance;
            }
            return null;
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
