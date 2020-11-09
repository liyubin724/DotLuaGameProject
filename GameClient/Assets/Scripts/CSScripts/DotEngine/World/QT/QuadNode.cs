using DotEngine.Pool;
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

        private List<IQuadObject> m_Objects = new List<IQuadObject>();

        internal QuadNode ParentNode { get; private set; } = null;
        private QuadNode[] m_ChildNodes = new QuadNode[4];

        public bool IsLeaf => m_ChildNodes[0] == null;

        public QuadNode this[QuadNodeDirection direction]
        {
            get
            {
                if(IsLeaf)
                {
                    return null;
                }
                return m_ChildNodes[(int)direction - 1];
            }
            set
            {
                m_ChildNodes[(int)direction - 1] = value;

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

        public int GetObjectCount(bool isIncludeChildNode)
        {
            return GetNodeObjectCount(this,isIncludeChildNode);
        }

        private int GetNodeObjectCount(QuadNode node, bool isIncludeChildNode)
        {
            if(node.IsLeaf || !isIncludeChildNode)
            {
                return node.m_Objects.Count;
            }

            int count = node.m_Objects.Count;
            foreach (var childNode in node.m_ChildNodes)
            {
                count += GetNodeObjectCount(childNode, true);
            }

            return count;
        }

        public QuadNode[] GetChildNodes(bool isIncludeChildNode)
        {
            List<QuadNode> nodeList = QuadPool.GetNodeList();
            SearchNodes(this, nodeList, false, isIncludeChildNode);
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
            nodeList.AddRange(node.m_ChildNodes);
            if(isIncludeChildNode)
            {
                foreach (var childNode in node.m_ChildNodes)
                {
                    SearchNodes(childNode, nodeList, false, isIncludeChildNode);
                }
            }
        }

        public IQuadObject[] GetObjects(bool isIncludeChildNode)
        {
            List<IQuadObject> objectList = QuadPool.GetObjectList();
            SearchObjects(this, objectList, isIncludeChildNode);
            IQuadObject[] result = objectList.ToArray();
            QuadPool.ReleaseObjectList(objectList);
            return result;
        }

        private void SearchObjects(QuadNode node,List<IQuadObject> objectList,bool isIncludeChildNode)
        {
            objectList.AddRange(node.m_Objects);

            if(node.IsLeaf || !isIncludeChildNode)
            {
                return;
            }else
            {
                foreach (var childNode in node.m_ChildNodes)
                {
                    SearchObjects(childNode, objectList, isIncludeChildNode);
                }
            }
        }

        internal void ClearObjects()
        {
            m_Objects.Clear();
        }

        internal bool TryInsertObject(IQuadObject obj)
        {
            if(Bounds.Contains(obj.Bounds))
            {
                m_Objects.Add(obj);
                return true;
            }
            return false;
        }

        internal void RemoveObject(IQuadObject obj)
        {
            m_Objects.Remove(obj);
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            for(int i =0;i<m_ChildNodes.Length;++i)
            {
                m_ChildNodes[i] = null;
            }

            m_Objects.Clear();

            ParentNode = null;
        }

        public override string ToString()
        {
            return $"QuadNode[{Depth}][{Direction}] : Bounds = {Bounds},isLeft = {IsLeaf},objectCount={GetObjectCount(false)}";
        }
    }
}
