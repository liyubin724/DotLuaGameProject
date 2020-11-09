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

    public class QuadNode : IObjectPoolItem
    {
        public int Depth { get; private set; } = -1;
        public QuadNodeDirection Direction { get; private set; } = QuadNodeDirection.None;
        public AABB2D Bounds { get; private set; }

        public QuadNode ParentNode { get; private set; } = null;
        internal QuadNode[] ChildNodes { get;} = new QuadNode[4];
        internal List<IQuadObject> InsideObjects { get; } = new List<IQuadObject>();

        public int ObjectCount => InsideObjects.Count;
        
        public bool IsLeaf => ChildNodes[0] == null;

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
            set
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

        internal void SetData(int depth,QuadNodeDirection direction, AABB2D bounds)
        {
            Depth = depth;
            Bounds = bounds;
            Direction = direction;
        }

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

        public QuadNode[] GetTotalChildNodes()
        {
            List<QuadNode> nodeList = QuadPool.GetNodeList();
            SearchNodes(this, nodeList, false, true);
            QuadNode[] result = nodeList.ToArray();
            QuadPool.ReleaseNodeList(nodeList);
            return result;
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

        public IQuadObject[] GetObjects()
        {
            return InsideObjects.ToArray();
        }

        public IQuadObject[] GetTotalObjects()
        {
            List<IQuadObject> objectList = QuadPool.GetObjectList();
            SearchObjects(this, objectList, true);
            IQuadObject[] result = objectList.ToArray();
            QuadPool.ReleaseObjectList(objectList);
            return result;
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
