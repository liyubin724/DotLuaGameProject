using DotEngine.Entity.Node;
using DotEngine.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Entity.Node
{
    public static class NodeBehaviour_Extersion
    {
        public static void FindBoneNodes(this NodeBehaviour nodeBehaviour)
        {
            List<NodeData> datas = new List<NodeData>();

            GameObject gObj = nodeBehaviour.gameObject;
            Transform[] transforms = gObj.GetComponentsInChildren<Transform>(true);
            foreach (var transform in transforms)
            {
                NodeData data = new NodeData();
                data.nodeType = NodeType.BoneNode;
                data.name = transform.name;
                data.transform = transform;

                datas.Add(data);
            }

            nodeBehaviour.boneNodes = datas.ToArray();
        }

        public static void FindSMRendererNodes(this NodeBehaviour nodeBehaviour)
        {
            List<NodeData> datas = new List<NodeData>();

            GameObject gObj = nodeBehaviour.gameObject;
            SkinnedMeshRenderer[] renderers = gObj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            foreach (var renderer in renderers)
            {
                NodeData data = new NodeData();
                data.nodeType = NodeType.SMRendererNode;
                data.name = renderer.name;
                data.renderer = renderer;

                datas.Add(data);
            }

            nodeBehaviour.smRendererNodes = datas.ToArray();
        }

        public static void CopyFrom(this NodeBehaviour to, NodeBehaviour from)
        {
            if (from == null || to == null || from.bindNodes == null || from.bindNodes.Length == 0)
            {
                return;
            }

            to.FindBoneNodes();
            to.FindSMRendererNodes();

            NodeData[] fromBindNodes = from.bindNodes;
            List<NodeData> toBindNodes = new List<NodeData>();
            Transform toTran = to.gameObject.transform;
            foreach (var node in fromBindNodes)
            {
                if (node == null || node.transform == null)
                {
                    continue;
                }
                string tranName = node.transform.name;
                Transform targetTran = toTran.GetChildByName(tranName);
                if (targetTran != null)
                {
                    NodeData data = new NodeData();
                    data.name = node.name;
                    data.nodeType = NodeType.BindNode;
                    data.transform = targetTran;
                    toBindNodes.Add(data);
                }
            }

            to.bindNodes = toBindNodes.ToArray();
        }
    }
}
