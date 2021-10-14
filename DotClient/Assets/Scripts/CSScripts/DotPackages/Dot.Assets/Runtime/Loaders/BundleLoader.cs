using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Assets.Loaders
{
    public class BundleLoader : ALoader
    {
        private BundleDetailConfig bundleDetailConfig = null;
        public override void DoInitialize(AssetDetailConfig detailConfig, Action completedCallback, params object[] objects)
        {
            base.DoInitialize(detailConfig, completedCallback, objects);
            bundleDetailConfig = objects[0] as BundleDetailConfig;
            string[] preloadPaths = bundleDetailConfig.GetPreloadPaths();

        }

        protected override void DoInitializeUpdate()
        {

        }

        protected override UnityEngine.Object InstanceAsset(string assetAddress, UnityEngine.Object uObject)
        {
            throw new NotImplementedException();
        }

        protected override UnityEngine.Object LoadAsset(string assetAddress)
        {
            throw new NotImplementedException();
        }
    }
}
