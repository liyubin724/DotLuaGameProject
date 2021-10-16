using DotEngine.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Assets.Loaders
{
    public class BundleNode : IPoolItem
    {
        private string path;
        private bool isBundleLoaded = false;
        private AssetBundle bundle;
        private List<BundleNode> depends = new List<BundleNode>();
        private bool isNeverDestroy = false;

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

        public AssetBundle Bundle
        {
            get
            {
                return bundle;
            }
            set
            {
                bundle = value;
                isBundleLoaded = true;
            }
        }

        public bool IsDone
        {
            get
            {
                if (string.IsNullOrEmpty(path))
                {
                    return false;
                }
                if (!isBundleLoaded)
                {
                    return false;
                }
                foreach (var d in depends)
                {
                    if (!d.IsDone)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private int refCount = 0;
        public void RetainRef() => ++refCount;
        public void ReleaseRef() => --refCount;

        public bool IsNeverDestroy
        {
            get
            {
                return IsNeverDestroy;
            }
            set
            {
                isNeverDestroy = value;
            }
        }

        public bool CanDestroy()
        {
            if(refCount>0 || isNeverDestroy)
            {
                return false;
            }
            return true;
        }

        public void AddDepend(BundleNode node)
        {
            node.RetainRef();
            depends.Add(node);
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            foreach (var d in depends)
            {
                d.ReleaseRef();
            }

            path = null;
            isBundleLoaded = false;
            if (bundle != null)
            {
                bundle.Unload(true);
            }
            bundle = null;
            depends.Clear();
        }
    }
}
