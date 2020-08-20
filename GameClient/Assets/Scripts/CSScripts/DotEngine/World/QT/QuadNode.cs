using DotEngine.Pool;
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
        public Rect Bounds { get; private set; } = Rect.zero;

        public QuadNode ParentNode { get; internal set; } = null;
        
        public QuadNode[] ChildNodes { get; private set; } = new QuadNode[4];
        public bool IsLeaf { get => ChildNodes[0] == null; }
        public List<QuadNode> TotalChildNodes
        {
            get
            {
                List<QuadNode> nodes = new List<QuadNode>();
                if(!IsLeaf)
                {
                    foreach(var node in ChildNodes)
                    {
                        nodes.AddRange(node.TotalChildNodes);
                    }
                }
                return nodes;
            }
        }
        
        public List<IQuadObject> Objects { get; } = new List<IQuadObject>();
        public List<IQuadObject> TotalObjects
        {
            get
            {
                List<IQuadObject> results = new List<IQuadObject>();
                results.AddRange(Objects);
                if(!IsLeaf)
                {
                    foreach(var childNode in ChildNodes)
                    {
                        results.AddRange(childNode.TotalObjects);
                    }
                }
                return results;
            }
        }
        public int ObjectCount { get => Objects.Count; }
        public int TotalObjectCount
        {
            get
            {
                int count = ObjectCount;
                if(!IsLeaf)
                {
                    foreach(var childNode in ChildNodes)
                    {
                        count += childNode.TotalObjectCount;
                    }
                }
                return count;
            }
        }

        public QuadNode this[QuadDirection direction]
        {
            get
            {
                switch(direction)
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

                if(value!=null)
                {
                    value.ParentNode = this;
                }
            }
        }

        public QuadNode()
        { }

        public void SetData(int depth,QuadDirection direction,Rect bounds)
        {
            Depth = depth;
            Direction = direction;
            Bounds = bounds;
        }

        public void SetData(int depth, QuadDirection direction, float x, float y, float width, float height)
        {
            SetData(depth, direction, new Rect(x, y, width, height));
        }

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
    }
}
