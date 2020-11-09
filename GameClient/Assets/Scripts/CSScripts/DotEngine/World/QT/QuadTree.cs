﻿using DotEngine.Log;
using DotEngine.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.World.QT
{
    public class QuadTree
    {
        public const string LOGGER_NAME = "QuadTree";

        private int m_MaxDepth = 7;//最大深度
        private int m_NodeSplitThreshold = 10;//进行分裂的数量

        private ObjectPool<QuadNode> m_NodePool = new ObjectPool<QuadNode>();
        
        private Dictionary<IQuadObject, QuadNode> m_ObjectToNodeDic = new Dictionary<IQuadObject, QuadNode>();

        public QuadNode Root { get; private set; }

        public QuadTree(int maxDepth,int splitThreshold,AABB2D bounds)
        {
            m_MaxDepth = maxDepth;
            m_NodeSplitThreshold = splitThreshold;

            Root = new QuadNode();
            Root.SetData(0,QuadNodeDirection.None,bounds);
        }

        #region insert QuadObject
        public void InsertObject(IQuadObject quadObject)
        {
            InsertObjectToNode(Root, quadObject);
        }

        private void InsertObjectToNode(QuadNode node,IQuadObject quadObject)
        {
            if(!node.Bounds.Contains(quadObject.Bounds))
            {
                LogUtil.LogError(LOGGER_NAME, "QuadTree::InsertObjectToNode->Object's bounds is not fit within node bounds");
                return;
            }

            if(node.IsLeaf && node.Depth<m_MaxDepth && node.GetObjectCount(false) >= m_NodeSplitThreshold)
            {
                SplitNode(node);
                ResetNodeObjects(node);
            }

            if(!node.IsLeaf)
            {
                QuadNode[] childNodes = node.GetChildNodes(false);
                foreach (var childNode in childNodes)
                {
                    if(childNode.Bounds.Contains(quadObject.Bounds))
                    {
                        InsertObjectToNode(childNode, quadObject);
                        return;
                    }
                }
            }

            node.TryInsertObject(quadObject);

            quadObject.OnBoundsChanged += OnHandleObjectBoundsChanged;

            m_ObjectToNodeDic[quadObject] = node;
        }

        private void SplitNode(QuadNode node)
        {
            AABB2D bounds = node.Bounds;

            Vector2 childNodeExtents = bounds.HalfExtents;

            QuadNode childNode = m_NodePool.Get();
            childNode.SetData(node.Depth + 1, QuadNodeDirection.LB, new AABB2D(bounds.Center - childNodeExtents, childNodeExtents));
            node[QuadNodeDirection.LB] = childNode;

            childNode = m_NodePool.Get();
            childNode.SetData(node.Depth + 1, QuadNodeDirection.RB, new AABB2D(bounds.Center + new Vector2(childNodeExtents.x,-childNodeExtents.y), childNodeExtents));
            node[QuadNodeDirection.RB] = childNode;

            childNode = m_NodePool.Get();
            childNode.SetData(node.Depth + 1, QuadNodeDirection.LT, new AABB2D(bounds.Center + new Vector2(-childNodeExtents.x, childNodeExtents.y), childNodeExtents));
            node[QuadNodeDirection.LT] = childNode;

            childNode = m_NodePool.Get();
            childNode.SetData(node.Depth + 1, QuadNodeDirection.RT, new AABB2D(bounds.Center + childNodeExtents, childNodeExtents));
            node[QuadNodeDirection.RT] = childNode;
        }

        private void ResetNodeObjects(QuadNode node)
        {
            IQuadObject[] objects = node.GetObjects(false);
            node.ClearObjects();
            List<IQuadObject> nodeObjects = ListPool<IQuadObject>.Get();
            foreach (var obj in objects)
            {
                bool isInsertToChildNode = false;
                QuadNode[] childNodes = node.GetChildNodes(false);
                foreach(var childNode in childNodes)
                {
                    if(childNode.TryInsertObject(obj))
                    {
                        m_ObjectToNodeDic[obj] = childNode;
                        isInsertToChildNode = true;
                        break;
                    }
                }

                if(!isInsertToChildNode)
                {
                    nodeObjects.Add(obj);
                }
            }

            if (nodeObjects.Count > 0)
            {
                foreach (var obj in nodeObjects)
                {
                    node.TryInsertObject(obj);
                }
            }

            ListPool<IQuadObject>.Release(nodeObjects);
        }

        private void OnHandleObjectBoundsChanged(IQuadObject quadObject,AABB2D oldBounds,AABB2D newBounds)
        {
            UpdateObject(quadObject);
        }

        #endregion

        #region Update QuadObject
        public void UpdateObject(IQuadObject quadObject)
        {
            if(m_ObjectToNodeDic.ContainsKey(quadObject))
            {
                RemoveObject(quadObject);
                InsertObject(quadObject);
            }else
            {
                LogUtil.LogError(QuadTree.LOGGER_NAME, "the object hasn't been added to tree");
            }
        }
        #endregion

        #region Remove QuadObject
        public void RemoveObject(IQuadObject quadObject)
        {
            if(m_ObjectToNodeDic.TryGetValue(quadObject,out var node))
            {
                RemoveObjectFromNode(node, quadObject);
            }
        }

        private void RemoveObjectFromNode(QuadNode node,IQuadObject quadObject)
        {
            quadObject.OnBoundsChanged -= OnHandleObjectBoundsChanged;
            m_ObjectToNodeDic.Remove(quadObject);
            node.RemoveObject(quadObject);

            QuadNode parentNode = node.ParentNode;
            if(parentNode!=null && parentNode.GetObjectCount(true) < m_NodeSplitThreshold)
            {
                QuadNode[] childNodes = parentNode.GetChildNodes(true);
                IQuadObject[] objects = parentNode.GetObjects(true);

                parentNode[QuadNodeDirection.LB] = null;
                parentNode[QuadNodeDirection.RB] = null;
                parentNode[QuadNodeDirection.LT] = null;
                parentNode[QuadNodeDirection.RT] = null;

                foreach (var childNode in childNodes)
                {
                    m_NodePool.Release(childNode);
                }

                parentNode.ClearObjects();
                foreach(var obj in objects)
                {
                    m_ObjectToNodeDic.Remove(obj);
                    InsertObjectToNode(parentNode, obj);
                }
            }
        }

        #endregion

        #region Query QuadObject
        public IQuadObject[] QueryIntersectsObjects(AABB2D bounds)
        {
            return QueryIntersectsObjectsFromNode(Root, bounds);
        }

        private IQuadObject[] QueryIntersectsObjectsFromNode(QuadNode node,AABB2D bounds)
        {
            if (!node.Bounds.Intersects(bounds))
            {
                return new IQuadObject[0];
            }

            List<IQuadObject> objects = ListPool<IQuadObject>.Get();
            IQuadObject[] objectsInNode = node.GetObjects(false);
            foreach (var obj in objectsInNode)
            {
                if (bounds.Intersects(obj.Bounds))
                {
                    objects.Add(obj);
                }
            }

            if (!node.IsLeaf)
            {
                QuadNode[] childNodes = node.GetChildNodes(false);
                foreach (var childNode in childNodes)
                {
                    if (childNode.Bounds.Intersects(bounds))
                    {
                        objects.AddRange(QueryContainsObjectsFromNode(childNode, bounds));
                    }
                }
            }

            IQuadObject[] result = objects.ToArray();
            ListPool<IQuadObject>.Release(objects);
            return result;
        }

        public IQuadObject[] QueryContainsObjects(AABB2D bounds)
        {
            return QueryContainsObjectsFromNode(Root, bounds);
        }

        private IQuadObject[] QueryContainsObjectsFromNode(QuadNode node,AABB2D bounds)
        {
            if(!node.Bounds.Intersects(bounds))
            {
                return new IQuadObject[0];
            }

            List<IQuadObject> objects = ListPool<IQuadObject>.Get();
            IQuadObject[] objectsInNode = node.GetObjects(false);
            foreach(var obj in objectsInNode)
            {
                if(bounds.Contains(obj.Bounds))
                {
                    objects.Add(obj);
                }
            }

            if(!node.IsLeaf)
            {
                QuadNode[] childNodes = node.GetChildNodes(false);
                foreach(var childNode in childNodes)
                {
                    if(childNode.Bounds.Intersects(bounds))
                    {
                        objects.AddRange(QueryContainsObjectsFromNode(childNode, bounds));
                    }
                }
            }

            IQuadObject[] result = objects.ToArray();
            ListPool<IQuadObject>.Release(objects);
            return result;
        }
        #endregion

    }
}
