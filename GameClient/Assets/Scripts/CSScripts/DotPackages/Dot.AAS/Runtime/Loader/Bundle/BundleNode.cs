using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEngine.AAS.Loader
{
    public enum BundleState
    {
        None = 0,
        Waiting,
        Loading,
        Finished,
    }

    public class BundleNode
    {
        public BundleState State { get; set; } = BundleState.None;
        public string Path { get; set; }

        private BundleNode[] dependBundles = new BundleNode[0];
    }
}
