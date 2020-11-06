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
            if(IsLeaf || !isIncludeChildNode)
            {
                return m_Objects.Count;
            }

            int count = m_Objects.Count;
            foreach (var childNode in m_ChildNodes)
            {
                count += childNode.GetObjectCount(true);
            }

            return count;
        }

        public QuadNode[] GetChildNodes(bool isIncludeChildNode)
        {
            if(IsLeaf)
            {
                return new QuadNode[0];
            }

            if(!isIncludeChildNode)
            {
                QuadNode[] result = new QuadNode[4];
                Array.Copy(m_ChildNodes, result, result.Length);
                return result;
            }
            else
            {

                List<QuadNode> nodes = ListPool<QuadNode>.Get();
                foreach(var node in m_ChildNodes)
                {
                    nodes.Add(node);
                    nodes.AddRange(node.GetChildNodes(true));
                }

                QuadNode[] result = nodes.ToArray();
                ListPool<QuadNode>.Release(nodes);
                return result;
            }
        }

        public IQuadObject[] GetObjects(bool isIncludeChildNode)
        {
            if(IsLeaf || !isIncludeChildNode)
            {
                return m_Objects.ToArray();
            }

            List<IQuadObject> objects = ListPool<IQuadObject>.Get();
            objects.AddRange(m_Objects);
            foreach (var node in m_ChildNodes)
            {
                objects.AddRange(node.GetObjects(true));
            }

            IQuadObject[] result = objects.ToArray();
            ListPool<IQuadObject>.Release(objects);

            return result;
        }

        internal IQuadObject[] ClearObjects()
        {
            IQuadObject[] objs = m_Objects.ToArray();

            m_Objects.Clear();

            return objs;
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
