using DotEngine.Assets.Operations;
using DotEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEngine.Assets.Loaders
{
    public class BundleNode
    {
        private string path;
        private bool isBundleLoaded = false;
        private AssetBundle bundle;
        private List<BundleNode> depends = new List<BundleNode>();

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
                if(string.IsNullOrEmpty(path))
                {
                    return false;
                }
                if(!isBundleLoaded)
                {
                    return false;
                }
                foreach (var d in depends)
                {
                    if(!d.IsDone)
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

        public void AddDepend(BundleNode node)
        {
            node.RetainRef();
            depends.Add(node);
        }
    }
}
