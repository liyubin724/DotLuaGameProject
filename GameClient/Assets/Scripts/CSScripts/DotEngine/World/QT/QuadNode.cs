using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEngine.World.QT
{
    public enum QuadDirection
    {
        NW = 0,//左上
        NE = 1,//右上
        SW = 2,//左下
        SE = 3//右下
    }

    public class QuadNode
    {
        public int Depth { get; private set; } = 0;
        public QuadDirection Direction { get; private set; }

        public QuadNode ParentNode { get; internal set; } = null;
        private QuadNode[] m_ChildNodes = new QuadNode[4];
        

        private List<IQuadObject> m_QuadObjects = new List<IQuadObject>();

        public Rect Bounds { get; internal set; } = Rect.zero;

        
        public QuadNode(int depth,Rect bounds)
        {
            Depth = depth;
            Bounds = bounds;
        }

        public QuadNode(int depth,float x,float y,float width,float height) : this(depth,new Rect(x,y,width,height))
        {
        }

        public bool IsLeafNode()
        {
            return m_ChildNodes[0] == null;
        }
    }
}
