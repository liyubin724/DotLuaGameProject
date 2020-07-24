using System;
using UnityEngine;

namespace DotEngine.Entity.Node
{
    [Serializable]
    public class NodeData
    {
        public NodeType nodeType = NodeType.BindNode;
        public string name = string.Empty;
        public Transform transform = null;
        public SkinnedMeshRenderer renderer = null;
    }
}
