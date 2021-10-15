using DotEngine.Pool;
using System;
using System.Collections.Generic;

namespace DotEngine.World.QT
{
    public enum QuadNodeDirection
    {
        None = 0,

        LB = 1,//左下
        RB = 2,//右下
        LT = 3,//左上
        RT = 4,//右上
    }

    public class QuadNode : IPoolItem
    {
        /// <summary>
        /// 当前结点深度值
        /// </summary>
        public int Depth { get; private set; } = -1;
        /// <summary>
        /// 当前结点在父结点的位置，对于根结点来说为None
        /// </summary>
        public QuadNodeDirection Direction { get; private set; } = QuadNodeDirection.None;
        /// <summary>
        /// 当前结点的AABB范围
        /// </summary>
        public AABB2D Bounds { get; private set; }
        /// <summary>
        /// 结点的父结点，对于根结点来说其为Null
        /// </summary>
        public QuadNode ParentNode { get; private set; } = null;
        /// <summary>
        /// 结点的四个子结点数组
        /// </summary>
        internal QuadNode[] ChildNodes { get;} = new QuadNode[4];
        /// <summary>
        /// 结点中存储的对象
        /// </summary>
        internal List<IQuadObject> InsideObjects { get; } = new List<IQuadObject>();
        /// <summary>
        /// 结点中存储对象的数量
        /// </summary>
        public int ObjectCount => InsideObjects.Count;
        /// <summary>
        /// 结点是否是叶结点
        /// </summary>
        public bool IsLeaf => ChildNodes[0] == null;
        /// <summary>
        /// 获取或修改子结点
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public QuadNode this[QuadNodeDirection direction]
        {
            get
            {
                if(IsLeaf)
                {
                    return null;
                }
                return ChildNodes[(int)direction - 1];
            }
            internal set
            {
                ChildNodes[(int)direction - 1] = value;

                if(value!=null)
                {
                    value.ParentNode = this;
                }
            }
        }

        public QuadNode() 
        {
        }
        /// <summary>
        /// 设置结点中的数据，为了节省内存已创建的结点将会使用缓存池循环使用
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="direction"></param>
        /// <param name="bounds"></param>
        internal void SetData(int depth,QuadNodeDirection direction, AABB2D bounds)
        {
            Depth = depth;
            Bounds = bounds;
            Direction = direction;
        }
        /// <summary>
        /// 当前结点以及包含的所有的子结点（包括子结点的子结点）中对象的数量
        /// </summary>
        /// <returns></returns>
        public int GetTotalObjectCount()
        {
            return GetNodeObjectCount(this, true);
        }

        private int GetNodeObjectCount(QuadNode node, bool isIncludeChildNode)
        {
            if(node.IsLeaf || !isIncludeChildNode)
            {
                return node.InsideObjects.Count;
            }

            int count = node.InsideObjects.Count;
            foreach (var childNode in node.ChildNodes)
            {
                count += GetNodeObjectCount(childNode, true);
            }

            return count;
        }
        /// <summary>
        /// 获取当前子结点（对于叶结点来说，不存在子结点）
        /// </summary>
        /// <returns></returns>
        public QuadNode[] GetChildNodes()
        {
            if (IsLeaf)
            {
                return null;
            }else
            {
                QuadNode[] result = new QuadNode[ChildNodes.Length];
                Array.Copy(ChildNodes, result, ChildNodes.Length);
                return result;
            }
        }

        public void GetChildNodes(ref List<QuadNode> nodeList)
        {
            if(IsLeaf)
            {
                return;
            }
            else
            {
                nodeList.AddRange(ChildNodes);
            }
        }

        /// <summary>
        /// 获取结点的所有的子结点（包括子结点的子结点，但不包括当前结点）
        /// </summary>
        /// <returns></returns>
        public QuadNode[] GetTotalChildNodes()
        {
            List<QuadNode> nodeList = QuadPool.GetNodeList();
            SearchNodes(this, nodeList, false, true);
            QuadNode[] result = nodeList.ToArray();
            QuadPool.ReleaseNodeList(nodeList);
            return result;
        }

        public void GetTotalChildNodes(ref List<QuadNode> nodeList)
        {
            SearchNodes(this, nodeList, false, true);
        }

        private void SearchNodes(QuadNode node,List<QuadNode> nodeList,bool isIncludeSelf,bool isIncludeChildNode)
        {
            if(isIncludeSelf)
            {
                nodeList.Add(node);
            }
            if(node.IsLeaf)
            {
                return;
            }
            nodeList.AddRange(node.ChildNodes);
            if(isIncludeChildNode)
            {
                foreach (var childNode in node.ChildNodes)
                {
                    SearchNodes(childNode, nodeList, false, isIncludeChildNode);
                }
            }
        }
        /// <summary>
        /// 获取结点中存储的对象
        /// </summary>
        /// <returns></returns>
        public IQuadObject[] GetObjects()
        {
            return InsideObjects.ToArray();
        }

        public void GetObjects(ref List<IQuadObject> objectList)
        {
            objectList.AddRange(InsideObjects);
        }
        /// <summary>
        /// 获取结点及子结点（包括子结点的子结点）中存储的所有的对象
        /// </summary>
        /// <returns></returns>
        public IQuadObject[] GetTotalObjects()
        {
            List<IQuadObject> objectList = QuadPool.GetObjectList();
            SearchObjects(this, objectList, true);
            IQuadObject[] result = objectList.ToArray();
            QuadPool.ReleaseObjectList(objectList);
            return result;
        }

        public void GetTotalObjects(ref List<IQuadObject> objectList)
        {
            SearchObjects(this, objectList, true);
        }

        private void SearchObjects(QuadNode node,List<IQuadObject> objectList,bool isIncludeChildNode)
        {
            objectList.AddRange(node.InsideObjects);

            if(node.IsLeaf || !isIncludeChildNode)
            {
                return;
            }else
            {
                foreach (var childNode in node.ChildNodes)
                {
                    SearchObjects(childNode, objectList, isIncludeChildNode);
                }
            }
        }

        internal void ClearObjects()
        {
            InsideObjects.Clear();
        }

        internal void InsertObject(IQuadObject obj)
        {
            InsideObjects.Add(obj);
        }

        internal bool TryInsertObject(IQuadObject obj)
        {
            if(Bounds.Contains(obj.Bounds))
            {
                InsideObjects.Add(obj);
                return true;
            }
            return false;
        }

        internal void RemoveObject(IQuadObject obj)
        {
            InsideObjects.Remove(obj);
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            for(int i =0;i<ChildNodes.Length;++i)
            {
                ChildNodes[i] = null;
            }

            InsideObjects.Clear();

            ParentNode = null;
        }

        public override string ToString()
        {
            return $"QuadNode[{Depth}][{Direction}] : Bounds = {Bounds},isLeft = {IsLeaf},objectCount={ObjectCount}";
        }
    }
}
