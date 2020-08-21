using DotEngine.Log;
using DotEngine.Pool;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace DotEngine.World.QT
{
    public class QuadTree
    {
        private ObjectPool<QuadNode> m_NodePool = null;

        private Dictionary<IQuadObject, QuadNode> m_ObjectInNodeDic = new Dictionary<IQuadObject, QuadNode>();

        private QuadNode m_Root = null;
        private int m_MaxDepth = 0;
        private int m_NodeSplitObjectCount = 0;

        private List<IQuadObject> m_ReusedObjectList = new List<IQuadObject>();

        public QuadTree()
        {
        }

        public void SetData(int maxDepth, int nodeSplitObjectCount, Rect rootRect)
        {
            if(m_NodePool == null)
            {
                m_NodePool = new ObjectPool<QuadNode>((int)(Mathf.Pow(2, Mathf.Pow(2, maxDepth) - 1))) ;
            }

            m_MaxDepth = maxDepth;
            m_NodeSplitObjectCount = nodeSplitObjectCount;

            m_Root = m_NodePool.Get();
            m_Root.SetData(0, QuadDirection.None, rootRect);
        }

        public QuadNode[] GetNodes()
        {
            return m_Root.GetNodes(true);
        }

        public IQuadObject[] QueryInsideObjects(Rect bounds)
        {
            return m_Root?.GetInsideObjects(bounds);
        }

        #region Insert

        public void InsertObject(IQuadObject quadObject)
        {
            Log($"InsertObject::Bounds = {quadObject.ProjectRect}");

            InsertObjectToNode(m_Root, quadObject);
        }

        private void InsertObjectToNode(QuadNode node,IQuadObject quadObject)
        {
            if (!node.ProjectRect.Contains(quadObject.ProjectRect))
            {
                throw new Exception("QuadObject's bounds is not fit within node bounds");
            }

            Log($"InsertObjectToNode::try to add to node({node.ToString()}),object = {quadObject.ProjectRect}");

            if (node.IsLeaf && node.ObjectCount >= m_NodeSplitObjectCount && node.Depth < m_MaxDepth)
            {
                Log($"InsertObjectToNode::try to Split node({node.ToString()})");
                SplitNode(node);

                List<IQuadObject> relocateObjects = new List<IQuadObject>();
                foreach (var obj in node.Objects)
                {
                    foreach (var childNode in node.ChildNodes)
                    {
                        if (childNode.ProjectRect.Contains(obj.ProjectRect))
                        {
                            relocateObjects.Add(obj);
                            break;
                        }
                    }
                }

                foreach(var obj in relocateObjects)
                {
                    Log($"InsertObjectToNode::Relocate Object int to node({node.ToString()}),object = {obj.ProjectRect}");
                    RemoveObjectFromNode(node, obj,false);
                    InsertObjectToNode(node, obj);
                }
            }

            if(!node.IsLeaf)
            {
                foreach (var childNode in node.ChildNodes)
                {
                    if (childNode.ProjectRect.Contains(quadObject.ProjectRect))
                    {
                        Log($"InsertObjectToNode::Add Obj into childNode.ParentNode = ({node.ToString()}),childNode = {childNode},object = {quadObject.ProjectRect}");
                        InsertObjectToNode(childNode, quadObject);
                        return;
                    }
                }
            }

            Log($"InsertObjectToNode::Add Obj into Node.Node = ({node.ToString()}),object = {quadObject.ProjectRect}");
            AddObjectToNode(node, quadObject);
        }

        private void AddObjectToNode(QuadNode node, IQuadObject quadObject)
        {
            Log($"AddObjectToNode::Add Obj into Node.Node = ({node.ToString()}),object = {quadObject.ProjectRect}");

            quadObject.BoundsChangedHandler += OnObjectBoundsChanged;
            node.Objects.Add(quadObject);

            m_ObjectInNodeDic.Add(quadObject, node);
        }

        private void OnObjectBoundsChanged(IQuadObject quadObject)
        {
            Log($"OnObjectBoundsChanged::the bounds of object is changed. object = {quadObject.ProjectRect}");

            RemoveObject(quadObject);
            InsertObject(quadObject);
        }

        private void SplitNode(QuadNode node)
        {
            float width = node.ProjectRect.width * 0.5f;
            float height = node.ProjectRect.height * 0.5f;

            QuadNode childNode = m_NodePool.Get();
            childNode.SetData(node.Depth + 1, QuadDirection.LB, node.ProjectRect.xMin, node.ProjectRect.yMin, width, height);
            node[QuadDirection.LB] = childNode;

            childNode = m_NodePool.Get();
            childNode.SetData(node.Depth + 1, QuadDirection.RB, node.ProjectRect.center.x, node.ProjectRect.yMin, width, height);
            node[QuadDirection.RB] = childNode;

            childNode = m_NodePool.Get();
            childNode.SetData(node.Depth + 1, QuadDirection.LT, node.ProjectRect.xMin, node.ProjectRect.center.y, width, height); ;
            node[QuadDirection.LT] = childNode;

            childNode = m_NodePool.Get();
            childNode.SetData(node.Depth + 1, QuadDirection.RT, node.ProjectRect.center.x, node.ProjectRect.center.y, width, height);
            node[QuadDirection.RT] = childNode;
        }
        #endregion

        #region Remove

        public void RemoveObject(IQuadObject quadObject)
        {
            if(m_ObjectInNodeDic.TryGetValue(quadObject,out var node))
            {
                Log($"RemoveObject::try to remove object. object = {quadObject.ProjectRect}");

                RemoveObjectFromNode(node, quadObject, true);
            }
        }

        private void RemoveObjectFromNode(QuadNode node, IQuadObject quadObject,bool isMergeNode)
        {
            Log($"RemoveObjectFromNode::remove object. node = {node},object = {quadObject}");
            quadObject.BoundsChangedHandler -= OnObjectBoundsChanged;
            node.Objects.Remove(quadObject);
            m_ObjectInNodeDic.Remove(quadObject);

            if(isMergeNode && node.ParentNode !=null)
            {
                Log($"RemoveObjectFromNode::Merge node. node = {node}");
                MergeNode(node.ParentNode);
            }
        }

        private void MergeNode(QuadNode node)
        {
            if (node.TotalObjectCount <= m_NodeSplitObjectCount)
            {
                IQuadObject[] quadObjects = node.GetObjects();
                QuadNode[] quadNodes = node.GetNodes(false);

                node.Objects.Clear();
                node[QuadDirection.LB] = null;
                node[QuadDirection.RB] = null;
                node[QuadDirection.LT] = null;
                node[QuadDirection.RT] = null;

                foreach (var qn in quadNodes)
                {
                    m_NodePool.Release(qn);
                }

                foreach (var qo in quadObjects)
                {
                    node.Objects.Add(qo);
                    m_ObjectInNodeDic[qo] = node;
                }

                if(node.ParentNode!=null)
                {
                    MergeNode(node.ParentNode);
                }
            }
        }
        #endregion

        public void Clear()
        {

        }

        [Conditional("QUAD_TREE_DEBUG")]
        private void Log(string message)
        {
            if(!LogUtil.IsInited)
            {
                DotEngine.Log.ILogger logger = new UnityLogger();
                LogUtil.SetLogger(logger);
            }
            LogUtil.LogInfo("QuadTree", message);
        }
    }
}
