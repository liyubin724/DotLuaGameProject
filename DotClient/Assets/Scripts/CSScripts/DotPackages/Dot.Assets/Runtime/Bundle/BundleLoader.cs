using DotEngine.Assets.Operations;
using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class BundleLoader : ALoader
    {
        protected override UnityObject LoadAsset(string assetPath)
        {
            throw new NotImplementedException();
        }

        protected override void OnInitialize(params object[] values)
        {
            throw new NotImplementedException();
        }

        protected override bool OnInitializeUpdate(float deltaTime)
        {
            throw new NotImplementedException();
        }

        protected override void OnUnloadUnusedAssets()
        {
            throw new NotImplementedException();
        }

        protected override bool OnUnloadUnusedAssetsUpdate()
        {
            throw new NotImplementedException();
        }

        protected override void OnWillReleaseAssetNode(AssetNode node)
        {
            throw new NotImplementedException();
        }
    }
}
