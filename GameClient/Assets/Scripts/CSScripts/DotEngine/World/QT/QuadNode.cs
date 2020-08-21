using DotEngine.Pool;
using DotEngine.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.World.QT
{
    public enum QuadDirection
    {
        None = 0,
        LB = 1,//左下
        RB = 2,//右下
        LT = 3,//左上
        RT = 4,//右上
    }

    public class QuadNode : IObjectPoolItem
    {
        public int Depth { get; private set; } = 0;
        public QuadDirection Direction { get; private set; } = QuadDirection.None;
        public QuadNode this[QuadDirection direction]
        {
            get
            {
                switch (direction)
                {
                    case QuadDirection.LB:
                        return ChildNodes[0];
                    case QuadDirection.RB:
                        return ChildNodes[1];
                    case QuadDirection.LT:
                        return ChildNodes[2];
                    case QuadDirection.RT:
                        return ChildNodes[3];
                    default:
                        return null;
                }
            }
            set
            {
                switch (direction)
                {
                    case QuadDirection.LB:
                        ChildNodes[0] = value;
                        break;
                    case QuadDirection.RB:
                        ChildNodes[1] = value;
                        break;
                    case QuadDirection.LT:
                        ChildNodes[2] = value;
                        break;
                    case QuadDirection.RT:
                        ChildNodes[3] = value;
                        break;
                }

                if (value != null)
                {
                    value.ParentNode = this;
                }
            }
        }
        public Rect ProjectRect { get; private set; } = Rect.zero;

        public Bounds Bounds { get; private set; }

        public QuadNode ParentNode { get; private set; } = null;

        public QuadNode[] ChildNodes { get; private set; } = new QuadNode[4];
        public bool IsLeaf { get => ChildNodes[0] == null; }

        public List<IQuadObject> Objects { get; } = new List<IQuadObject>();

        public int ObjectCount { get => Objects.Count; }
        public int TotalObjectCount
        {
            get
            {
                int count = ObjectCount;
                if (!IsLeaf)
                {
                    foreach (var childNode in ChildNodes)
                    {
                        count += childNode.TotalObjectCount;
                    }
                }
                return count;
            }
        }

        private List<QuadNode> m_ReusedNodeList = new List<QuadNode>();
        private List<IQuadObject> m_ReusedObjectList = new List<IQuadObject>();

        public QuadNode()
        { }

        #region SetData
        public void SetData(int depth, QuadDirection direction, Rect rect)
        {
            Depth = depth;
            Direction = direction;
            ProjectRect = rect;

            Bounds = new Bounds(new Vector3(rect.center.x, 0.0f, rect.center.y), new Vector3(rect.size.x, 0.0f, rect.size.y));
        }

        public void SetData(int depth, QuadDirection direction, float x, float y, float width, float height)
        {
            SetData(depth, direction, new Rect(x, y, width, height));
        }
        #endregion

        #region Get Nodes

        public QuadNode GetContainsNode(Rect pRect)
        {
            return QueryContainsNode(this, pRect);
        }

        private QuadNode QueryContainsNode(QuadNode node, Rect projectRect)
        {
            if (node.ProjectRect.Contains(projectRect))
            {
                if (!node.IsLeaf)
                {
                    foreach (var childNode in node.ChildNodes)
                    {
                        if (childNode.ProjectRect.Contains(projectRect))
                        {
                            return QueryContainsNode(childNode, projectRect);
                        }
                    }
                }

                return node;
            }
            return null;
        }

        public QuadNode[] GetNodes(bool isIncludeSelf)
        {
            QueryNodes(this, m_ReusedNodeList, isIncludeSelf);
            QuadNode[] nodes = m_ReusedNodeList.ToArray();

            m_ReusedNodeList.Clear();

            return nodes;
        }

        private void QueryNodes(QuadNode node, List<QuadNode> nodeList, bool isIncludeSelf)
        {
            if (isIncludeSelf)
            {
                nodeList.Add(node);
            }
            if (!node.IsLeaf)
            {
                foreach (var childNode in node.ChildNodes)
                {
                    QueryNodes(childNode, nodeList, true);
                }
            }
        }

        #endregion

        #region Get Objects
        public IQuadObject[] GetObjects()
        {
            QueryObjects(this, m_ReusedObjectList);

            IQuadObject[] objects = m_ReusedObjectList.ToArray();
            m_ReusedObjectList.Clear();

            return objects;
        }

        private void QueryObjects(QuadNode node, List<IQuadObject> objList)
        {
            objList.AddRange(node.Objects);
            if (!node.IsLeaf)
            {
                foreach (var childNode in node.ChildNodes)
                {
                    QueryObjects(childNode, objList);
                }
            }
        }

        public IQuadObject[] GetInsideObjects(Rect projectRect)
        {
            QueryInsideObjects(this, projectRect, m_ReusedObjectList);

            IQuadObject[] objects = m_ReusedObjectList.ToArray();
            m_ReusedObjectList.Clear();

            return objects;
        }

        private void QueryInsideObjects(QuadNode node, Rect projectRect, List<IQuadObject> objList)
        {
            if (node.ProjectRect.IntersectsWith(projectRect))
            {
                foreach (var obj in node.Objects)
                {
                    if (projectRect.Contains(obj.ProjectRect))
                    {
                        objList.Add(obj);
                    }
                }

                if (!node.IsLeaf)
                {
                    foreach (var childNode in node.ChildNodes)
                    {
                        QueryInsideObjects(childNode, projectRect, objList);
                    }
                }

            }
        }

        public IQuadObject[] GetIntersectedObjects(Rect projectRect)
        {
            QueryIntersectedObjects(this, projectRect, m_ReusedObjectList);

            IQuadObject[] objects = m_ReusedObjectList.ToArray();
            m_ReusedObjectList.Clear();

            return objects;
        }

        private void QueryIntersectedObjects(QuadNode node,Rect projectRect,List<IQuadObject> objList)
        {
            if (node.ProjectRect.IntersectsWith(projectRect))
            {
                foreach (var obj in node.Objects)
                {
                    if (projectRect.IntersectsWith(obj.ProjectRect))
                    {
                        objList.Add(obj);
                    }
                }

                if (!node.IsLeaf)
                {
                    foreach (var childNode in node.ChildNodes)
                    {
                        QueryIntersectedObjects(childNode, projectRect, objList);
                    }
                }
            }
        }

        public IQuadObject[] GetIntersectedObjects(Plane[] planes)
        {
            QueryIntersectedObjects(this, planes, m_ReusedObjectList);

            IQuadObject[] objects = m_ReusedObjectList.ToArray();
            m_ReusedObjectList.Clear();

            return objects;
        }

        private void QueryIntersectedObjects(QuadNode node, Plane[] planes, List<IQuadObject> objList)
        {
            if(GeometryUtility.TestPlanesAABB(planes,Bounds))
            {
                foreach (var obj in node.Objects)
                {
                    if(GeometryUtility.TestPlanesAABB(planes, obj.Bounds))
                    {
                        objList.Add(obj);
                    }
                }

                if (!node.IsLeaf)
                {
                    foreach (var childNode in node.ChildNodes)
                    {
                        QueryIntersectedObjects(childNode, planes, objList);
                    }
                }
            }
        }

        #endregion

        #region Pool Item
        public void OnNew()
        {
        }

        public void OnGet()
        {
        }

        public void OnRelease()
        {
            ParentNode = null;
            Objects.Clear();

            ChildNodes[0] = null;
            ChildNodes[1] = null;
            ChildNodes[2] = null;
            ChildNodes[3] = null;
        }
        #endregion

        public override string ToString()
        {
            return $"{Depth}_{Direction}_({ProjectRect})";
        }

    }
}
