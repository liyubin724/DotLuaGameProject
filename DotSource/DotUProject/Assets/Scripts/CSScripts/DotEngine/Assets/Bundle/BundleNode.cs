using DotEngine.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Assets
{
    internal class BundleNode : IElement
    {
        internal string Path { get; private set; }
        internal bool IsNeverDestroy { get; private set; } = false;
        internal NodeState State { get; set; } = NodeState.None;
        private AssetBundle bundle;
        internal AssetBundle Bundle
        {
            get
            {
                return bundle;
            }
            set
            {
                bundle = value;
                if(bundle!=null)
                {
                    State = NodeState.Loaded;
                }else
                {
                    State = NodeState.LoadError;
                }
            }
        }

        private int refCount = 0;
        internal void RetainRef() => ++refCount;
        internal void ReleaseRef() => --refCount;

        internal bool IsDone
        {
            get
            {
                if(State == NodeState.None || State == NodeState.Loading)
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

        private List<BundleNode> depends = new List<BundleNode>();
        internal void BindDepend(BundleNode node)
        {
            node.RetainRef();
            depends.Add(node);
        }

        internal bool IsInUsing()
        {
            if(refCount > 0 || IsNeverDestroy)
            {
                return true;
            }
            return false;
        }

        internal void DoInitialize(string path,bool isNeverDestroy = false)
        {
            Path = path;
            IsNeverDestroy = isNeverDestroy;
            State = NodeState.None;
        }

        public void OnGetFromPool()
        {
        }

        public void OnReleaseToPool()
        {
            foreach (var d in depends)
            {
                d.ReleaseRef();
            }
            depends.Clear();
            if (bundle != null)
            {
                bundle.Unload(true);
            }
            bundle = null;
            refCount = 0;
            State = NodeState.None;
            Path = null;
        }
    }
}
