using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Entity.Node
{
    public class NodeBehaviour : MonoBehaviour,ISerializationCallbackReceiver
    {
        public NodeData[] bindNodes = new NodeData[0];
        public NodeData[] boneNodes = new NodeData[0];
        public NodeData[] smRendererNodes = new NodeData[0];

        private Dictionary<NodeType, Dictionary<string, NodeData>> nodeDic = new Dictionary<NodeType, Dictionary<string, NodeData>>();

        private NodeData GetNode(NodeType nodeType, string name)
        {
            if (nodeDic.TryGetValue(nodeType, out Dictionary<string, NodeData> dataDic) 
                && dataDic.TryGetValue(name, out NodeData data))
            {
                return data;
            }
            return null;
        }

        public Transform GetBindTransform(string name)
        {
            return GetNode(NodeType.BindNode, name)?.transform;
        }

        public Transform GetBoneTransform(string name)
        {
            return GetNode(NodeType.BoneNode, name)?.transform;
        }

        public SkinnedMeshRenderer GetSMRenderer(string name)
        {
            return GetNode(NodeType.SMRendererNode, name)?.renderer;
        }

        public Transform[] GetBoneTransformByNames(string[] names)
        {
            if (names == null || names.Length == 0)
                return new Transform[0];

            Transform[] transforms = new Transform[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                transforms[i] = GetBoneTransform(names[i]);
            }
            return transforms;
        }

        public void OnBeforeSerialize()
        {
           
        }

        public void OnAfterDeserialize()
        {
            Dictionary<string, NodeData> dataDic = new Dictionary<string, NodeData>();
            nodeDic.Add(NodeType.BindNode, dataDic);
            foreach (var data in bindNodes)
            {
                dataDic.Add(data.name, data);
            }

            dataDic = new Dictionary<string, NodeData>();
            nodeDic.Add(NodeType.BoneNode, dataDic);
            foreach (var data in boneNodes)
            {
                dataDic.Add(data.name, data);
            }

            dataDic = new Dictionary<string, NodeData>();
            nodeDic.Add(NodeType.SMRendererNode, dataDic);
            foreach (var data in smRendererNodes)
            {
                dataDic.Add(data.name, data);
            }
        }
    }
}
