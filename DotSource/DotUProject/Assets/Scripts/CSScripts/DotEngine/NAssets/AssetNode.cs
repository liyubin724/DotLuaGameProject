using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityObject = UnityEngine.Object;

namespace DotEngine.NAssets
{
    public class AssetNode
    {
        public string Path { get; set; }

        private WeakReference<UnityObject> assetWR = null;
        private List<WeakReference<UnityObject>> assetInstanceWRList = null;

    }
}
