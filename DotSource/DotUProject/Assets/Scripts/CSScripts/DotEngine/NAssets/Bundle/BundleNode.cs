using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEngine.NAssets
{
    internal class BundleNode
    {
        public string Path { get; set; }
        public bool IsNeverDestroy { get; set; }
        public int RefCount { get; set; } = 0;

        private AssetBundle m_Bundle;

        
    }
}
