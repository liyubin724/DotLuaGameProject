using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Assets
{
    public class DatabaseLoader : ILoader
    {
        public void DoDestroy()
        {
            throw new NotImplementedException();
        }

        public void DoInitialize(AssetDetailConfig detailConfig, Action initCallback, params object[] objects)
        {
            throw new NotImplementedException();
        }

        public void DoUdpate(float deltaTime, float unscaleDeltaTime)
        {
            throw new NotImplementedException();
        }

        public UnityEngine.Object InstanceAsset(string assetPath)
        {
            throw new NotImplementedException();
        }

        public UnityEngine.Object[] InstanceAssets(string[] assetPaths)
        {
            throw new NotImplementedException();
        }

        public UnityEngine.Object LoadAsset(string assetPath)
        {
            throw new NotImplementedException();
        }

        public UnityEngine.Object[] LoadAssets(string[] assetPaths)
        {
            throw new NotImplementedException();
        }

        public void UnloadUnusedAssets(Action unloadCallback)
        {
            throw new NotImplementedException();
        }
    }
}
