using DotEngine.NativeDrawer.Property;
using UnityEngine;

namespace DotEngine.BL.Node
{
    public class NodeData : ScriptableObject
    {
        [Readonly]
        public int UniqueID  = -1;
        [Readonly]
        public NodeCategory Category = NodeCategory.None;
        [Readonly]
        public NodePlatform Platform = NodePlatform.All;
    }
}