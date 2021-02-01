using System;
using UnityEngine;

namespace DotEngine.Entity.Node
{
    [Serializable]
    public class NodeData
    {
        public string name = string.Empty;
        public NodeType nodeType = NodeType.BindNode;
        public Transform transform = null;

        public SkinnedMeshRenderer renderer = null;
    }
}
