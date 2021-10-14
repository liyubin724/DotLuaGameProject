using DotEngine.Assets.Operations;
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
        private AssetBundle bundle;
        private bool isAsyncFinished = false;
        private BundleCreateAsyncOperation requestOperation;
        private List<BundleNode> depends = new List<BundleNode>();

        public bool IsDone
        {
            get
            {
                return false;
            }
        }

        public AssetBundle GetAssetBundle()
        {
            return bundle;
        }

        private int refCount = 0;
        public void RetainRef() => ++refCount;
        public void ReleaseRef() => --refCount;

        public void InitNode(string bundlePath,BundleCreateAsyncOperation operation)
        {
            path = bundlePath;
            requestOperation = operation;
            requestOperation.OnCompleteCallback += OnOperationComplete;
        }

        public void AddDepend(BundleNode node)
        {
            node.RetainRef();
            depends.Add(node);
        }

        public void DestroyNode()
        {
            if(refCount!= 0)
            {
                Debug.LogError("");
            }

        }

        private void OnOperationComplete()
        {
            isAsyncFinished = true;

            requestOperation.OnCompleteCallback -= OnOperationComplete;
            bundle = (AssetBundle)requestOperation.GetAsset();
            requestOperation = null;
        }
    }
}
