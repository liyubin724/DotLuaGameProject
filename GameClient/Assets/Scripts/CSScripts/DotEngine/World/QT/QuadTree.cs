using DotEngine.Pool;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.World.QT
{
    public class QuadTree
    {
        private static readonly int LIMIT_MAX_DEPTH = 7;

        private ObjectPool<QuadNode> m_NodePool = null;

        private QuadNode m_Root = null;
        public QuadNode RootNode { get => m_Root; }
        private int m_MaxDepth = 0;
        private int m_NodeSplitObjectCount = 0;

        private List<IQuadObject> m_ReusedList = new List<IQuadObject>();

        public QuadTree()
        {
            m_NodePool = new ObjectPool<QuadNode>((int)Mathf.Pow(2, LIMIT_MAX_DEPTH));
        }

        public void SetData(int maxDepth,int nodeSplitObjectCount,Rect rootRect)
        {
            if(maxDepth> LIMIT_MAX_DEPTH)
            {
                throw new Exception("The depth is too large");
            }

            m_MaxDepth = maxDepth;
            m_NodeSplitObjectCount = nodeSplitObjectCount;

            m_Root = m_NodePool.Get();
            m_Root.SetData(0, QuadDirection.None, rootRect);
        }

        #region Query
        public IQuadObject[] Query(Rect bounds)
        {
            QueryObjectFromNode(bounds, m_Root, m_ReusedList);

            IQuadObject[] results = m_ReusedList.ToArray();
            m_ReusedList.Clear();

            return results;
        }

        private void QueryObjectFromNode(Rect bounds, QuadNode node, List<IQuadObject> results)
        {
            if (node == null)
            {
                return;
            }

            if (bounds.IntersectsWith(node.Bounds))
            {
                foreach (var quadObject in node.Objects)
                {
                    if (bounds.IntersectsWith(quadObject.Bounds))
                    {
                        results.Add(quadObject);
                    }
                }

                if (!node.IsLeaf)
                {
                    foreach (var childNode in node.ChildNodes)
                    {
                        QueryObjectFromNode(bounds, childNode, results);
                    }
                }
            }
        }
        #endregion

        #region Insert

        public void InsertObject(IQuadObject quadObject)
        {
            InsertObjectToNode(FindInsertTargetNode(m_Root, quadObject),quadObject);
        }

        private void InsertObjectToNode(QuadNode node, IQuadObject quadObject)
        {
            quadObject.BoundsChanged += OnObjectBoundsChanged;
            node.Objects.Add(quadObject);
        }

        private void OnObjectBoundsChanged(IQuadObject quadObject)
        {
            RemoveObject(quadObject);
            InsertObject(quadObject);
        }

        private QuadNode FindInsertTargetNode(QuadNode node, IQuadObject quadObject)
        {
            if (!node.Bounds.Contains(quadObject.Bounds))
            {
                throw new Exception("QuadObject's bounds is not fit within node bounds");
            }

            if (node.IsLeaf && node.ObjectCount >= m_NodeSplitObjectCount && node.Depth < m_MaxDepth)
            {
                SplitNode(node);

                List<IQuadObject> quadObjects = new List<IQuadObject>(node.Objects);
                node.Objects.Clear();

                foreach(var qObj in quadObjects)
                {
                    InsertObjectToNode(FindInsertTargetNode(node, qObj),qObj);
                }
            }

            if (!node.IsLeaf)
            {
                foreach (var childNode in node.ChildNodes)
                {
                    if (childNode.Bounds.Contains(quadObject.Bounds))
                    {
                        return FindInsertTargetNode(childNode, quadObject);
                    }
                }
            }

            return node;
        }

        private void SplitNode(QuadNode node)
        {
            float width = node.Bounds.width * 0.5f;
            float height = node.Bounds.height * 0.5f;

            QuadNode childNode = m_NodePool.Get();
            childNode.SetData(node.Depth + 1, QuadDirection.LB, node.Bounds.xMin, node.Bounds.yMin, width, height);
            node[QuadDirection.LB] = childNode;

            childNode = m_NodePool.Get();
            childNode.SetData(node.Depth + 1, QuadDirection.RB, node.Bounds.center.x, node.Bounds.yMin, width, height);
            node[QuadDirection.RB] = childNode;

            childNode = m_NodePool.Get();
            childNode.SetData(node.Depth + 1, QuadDirection.LT, node.Bounds.xMin, node.Bounds.center.y, width, height); ;
            node[QuadDirection.LT] = childNode;

            childNode = m_NodePool.Get();
            childNode.SetData(node.Depth + 1, QuadDirection.RT, node.Bounds.center.x, node.Bounds.center.y, width, height);
            node[QuadDirection.RT] = childNode;
        }
        #endregion

        #region Remove

        public void RemoveObject(IQuadObject quadObject)
        {
            RemoveObjectFromNode(FindRemoveTargetNode(m_Root, quadObject), quadObject);
        }

        public void RemoveObjectFromNode(QuadNode node,IQuadObject quadObject)
        {
            node.Objects.Remove(quadObject);
            quadObject.BoundsChanged -= OnObjectBoundsChanged;

            if(node.ParentNode !=null)
            {
                MergeNode(node.ParentNode);
            }
        }

        private QuadNode FindRemoveTargetNode(QuadNode node, IQuadObject quadObject)
        {
            if(node.Objects.Contains(quadObject))
            {
                return node;
            }

            if (!node.IsLeaf)
            {
                foreach(var childNode in node.ChildNodes)
                {
                    return FindRemoveTargetNode(childNode, quadObject);
                }
            }

            return null;
        }

        private void MergeNode(QuadNode node)
        {
            if (node.TotalObjectCount <= m_NodeSplitObjectCount)
            {
                List<IQuadObject> quadObjects = node.TotalObjects;
                List<QuadNode> quadNodes = node.TotalChildNodes;

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
                    InsertObjectToNode(node, qo);
                }
            }
        }
        #endregion

        public void Clear()
        {
            
        }


    }
}
