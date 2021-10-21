using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class Handler : IPoolItem
    {
        private string[] addresses = null;
        private string[] paths = null;
        private bool isInstance = false;
        private UnityObject[] uObjects = null;
        private float[] progresses = null;
        private SystemObject userdata = null;
        
        private OnLoadAssetProgress onLoadAssetProgress;
        private OnLoadAssetComplete onLoadAssetComplete;
        private OnLoadAssetsProgress onLoadAssetsProgress;
        private OnLoadAssetsComplete onLoadAssetsComplete;

        public void SetData(string[] addresses,
            bool isInstance,
            OnLoadAssetProgress assetLoadProgress,
            OnLoadAssetComplete assetLoadComplete,
            OnLoadAssetsProgress assetsLoadProgress,
            OnLoadAssetsComplete assetsLoadComplete,
            AsyncPriority priority,
            SystemObject userdata)
        {

        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
        }
    }
}
