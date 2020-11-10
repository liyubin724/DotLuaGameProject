using DotEngine.Log;
using DotEngine.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.World.QT
{
    /// <summary>
    /// 四叉树实现
    /// </summary>
    public class QuadTree
    {
        public const string LOGGER_NAME = "QuadTree";

        private int m_MaxDepth = 7;//最大深度
        private int m_NodeSplitThreshold = 10;//进行分裂的数量

        private ObjectPool<QuadNode> m_NodePool = new ObjectPool<QuadNode>();
        
        private Dictionary<IQuadObject, QuadNode> m_ObjectToNodeDic = new Dictionary<IQuadObject, QuadNode>();
        /// <summary>
        /// 根结点
        /// </summary>
        public QuadNode Root { get; private set; }

        public QuadTree(int maxDepth,int splitThreshold,AABB2D bounds)
        {
            m_MaxDepth = maxDepth;
            m_NodeSplitThreshold = splitThreshold;

            Root = new QuadNode();
            Root.SetData(0,QuadNodeDirection.None,bounds);
        }

        #region insert QuadObject
        /// <summary>
        /// 向树中插入对象
        /// </summary>
        /// <param name="quadObject"></param>
        public void InsertObject(IQuadObject quadObject)
        {
            if(quadObject.IsBoundsChangeable)
            {
                quadObject.OnBoundsChanged += OnHandleObjectBoundsChanged;
            }
            InsertObjectToNode(Root, quadObject);
        }

        private QuadNode InsertObjectToNode(QuadNode node,IQuadObject quadObject)
        {
            if(!node.Bounds.Contains(quadObject.Bounds))
            {
                LogUtil.LogError(LOGGER_NAME, "QuadTree::InsertObjectToNode->Object's bounds is not fit within node bounds");
                return null;
            }

            if(node.IsLeaf && node.Depth<m_MaxDepth && node.ObjectCount >= m_NodeSplitThreshold)
            {
                SplitNode(node);
                ResetNodeObjects(node);
            }

            if(!node.IsLeaf)
            {
                QuadNode[] childNodes = node.ChildNodes;
                foreach (var childNode in childNodes)
                {
                    if(childNode.Bounds.Contains(quadObject.Bounds))
                    {
                        return InsertObjectToNode(childNode, quadObject);
                    }
                }
            }

            node.InsertObject(quadObject);
            m_ObjectToNodeDic[quadObject] = node;

            return node;
        }
        //结点达到分裂的限定，进行结点的分裂
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
        //结点分裂后重新整理原来的对象
        private void ResetNodeObjects(QuadNode node)
        {
            IQuadObject[] objects = node.GetObjects();
            node.ClearObjects();

            List<IQuadObject> nodeObjects = QuadPool.GetObjectList();
            foreach (var obj in objects)
            {
                bool isInsertToChildNode = false;

                QuadNode[] childNodes = node.ChildNodes;
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
                    node.InsertObject(obj);
                }
            }
            QuadPool.ReleaseObjectList(nodeObjects);
        }

        private void OnHandleObjectBoundsChanged(IQuadObject quadObject,AABB2D oldBounds,AABB2D newBounds)
        {
            UpdateObject(quadObject);
        }

        #endregion

        #region Update QuadObject
        /// <summary>
        /// 更新指定的对象（一般情况下，由于对象发生了位置变化造成AABB变化后需要更新）
        /// </summary>
        /// <param name="quadObject"></param>
        public void UpdateObject(IQuadObject quadObject)
        {
            if(m_ObjectToNodeDic.TryGetValue(quadObject,out var node))
            {
                RemoveObjectFromNode(node, quadObject);

                QuadNode targetNode = node;
                while(targetNode != null)
                {
                    if(targetNode.Bounds.Contains(quadObject.Bounds))
                    {
                        break;
                    }
                    targetNode = targetNode.ParentNode;
                }
                if(targetNode != null)
                {
                    targetNode = InsertObjectToNode(targetNode, quadObject);

                    if(targetNode!=node)
                    {
                        MergeNode(node);
                    }
                }else
                {
                    LogUtil.LogError(QuadTree.LOGGER_NAME, "the object hasn't been added to tree");
                }
            }else
            {
                LogUtil.LogError(QuadTree.LOGGER_NAME, "the object hasn't been added to tree");
            }
        }
        #endregion

        #region Remove QuadObject
        /// <summary>
        /// 删除指定的对象
        /// </summary>
        /// <param name="quadObject"></param>
        public void RemoveObject(IQuadObject quadObject)
        {
            if(m_ObjectToNodeDic.TryGetValue(quadObject,out var node))
            {
                if(quadObject.IsBoundsChangeable)
                {
                    quadObject.OnBoundsChanged -= OnHandleObjectBoundsChanged;
                }

                RemoveObjectFromNode(node, quadObject);
                MergeNode(node);
            }
        }

        private void RemoveObjectFromNode(QuadNode node,IQuadObject quadObject)
        {
            m_ObjectToNodeDic.Remove(quadObject);
            node.RemoveObject(quadObject);
        }
        //对象删除或更新后，需要对旧的结点进行合并操作
        private void MergeNode(QuadNode mergedNode)
        {
            if(mergedNode == null || mergedNode.ObjectCount>=m_NodeSplitThreshold)
            {
                return;
            }

            QuadNode targetNode = mergedNode;
            while(targetNode!=null)
            {
                if(targetNode.ParentNode!=null && targetNode.ParentNode.GetTotalObjectCount() >= m_NodeSplitThreshold)
                {
                    break;
                }

                targetNode = targetNode.ParentNode;
            }

            if(targetNode!=null&&!targetNode.IsLeaf)
            {
                QuadNode[] childNodes = targetNode.GetTotalChildNodes();
                IQuadObject[] objects = targetNode.GetTotalObjects();

                targetNode[QuadNodeDirection.LB] = null;
                targetNode[QuadNodeDirection.RB] = null;
                targetNode[QuadNodeDirection.LT] = null;
                targetNode[QuadNodeDirection.RT] = null;

                foreach (var childNode in childNodes)
                {
                    m_NodePool.Release(childNode);
                }

                targetNode.ClearObjects();
                foreach (var obj in objects)
                {
                    m_ObjectToNodeDic.Remove(obj);
                    InsertObjectToNode(targetNode, obj);
                }
            }
        }

        #endregion

        #region Query QuadObject
        public IQuadObject[] QueryIntersectsObjects(AABB2D bounds)
        {
            List<IQuadObject> objects = QuadPool.GetObjectList();
            QueryIntersectsObjectsFromNode(Root, bounds, objects);
            IQuadObject[] result = objects.ToArray();
            QuadPool.ReleaseObjectList(objects);
            return result;
        }

        private void QueryIntersectsObjectsFromNode(QuadNode node, AABB2D bounds, List<IQuadObject> objectList)
        {
            if (!node.Bounds.Intersects(bounds))
            {
                return;
            }

            List<IQuadObject> objectsInNode = node.InsideObjects;
            foreach (var obj in objectsInNode)
            {
                if (bounds.Intersects(obj.Bounds))
                {
                    objectList.Add(obj);
                }
            }

            if(!node.IsLeaf)
            {
                QuadNode[] childNodes = node.ChildNodes;
                foreach (var childNode in childNodes)
                {
                    if (childNode.Bounds.Intersects(bounds))
                    {
                        QueryIntersectsObjectsFromNode(childNode, bounds, objectList);
                    }
                }
            }
        }

        public IQuadObject[] QueryContainsObjects(AABB2D bounds)
        {
            List<IQuadObject> objects = QuadPool.GetObjectList();
            QueryContainsObjectsFromNode(Root, bounds, objects);
            IQuadObject[] result = objects.ToArray();
            QuadPool.ReleaseObjectList(objects);
            return result;
        }

        private void QueryContainsObjectsFromNode(QuadNode node,AABB2D bounds, List<IQuadObject> objectList)
        {
            if(!node.Bounds.Intersects(bounds))
            {
                return;
            }

            List<IQuadObject> objectsInNode = node.InsideObjects;
            foreach(var obj in objectsInNode)
            {
                if(bounds.Contains(obj.Bounds))
                {
                    objectList.Add(obj);
                }
            }

            if(!node.IsLeaf)
            {
                QuadNode[] childNodes = node.ChildNodes;
                foreach(var childNode in childNodes)
                {
                    if(childNode.Bounds.Intersects(bounds))
                    {
                        QueryContainsObjectsFromNode(childNode, bounds,objectList);
                    }
                }
            }
        }
        #endregion

        public void Clear()
        {
            
        }
    }
}
