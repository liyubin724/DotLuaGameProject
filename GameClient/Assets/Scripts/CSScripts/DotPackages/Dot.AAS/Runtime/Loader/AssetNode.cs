using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityObject = UnityEngine.Object;

namespace DotEngine.AAS.Loader
{
    public enum AssetState
    {
        None = 0,
        Waiting,
        Loading,
        Instancing,
        Finished,
    }

    public class AssetNode
    {
        public AssetState State { get; set; } = AssetState.None;
        public string Address { get; set; }
        public bool IsNeverDestroy { get; set; } = false;

        
    }
}
