using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEngine.World.QT
{
    public class QuadTree
    {
        public QuadNode RootNode { get; private set; } = null;

        private int m_MaxDepth = 0;
        private int m_MaxObjectsPerLeaf = 0;
        public QuadTree(int maxDepth, int maxObjectsPerLeaf, Rect maxRect)
        {
            m_MaxDepth = maxDepth;
            m_MaxObjectsPerLeaf = maxObjectsPerLeaf;

            RootNode = new QuadNode(0, maxRect);
        }

        public void Insert(IQuadObject quadObject)
        {

        }

        public List<IQuadObject> Query(Rect bounds)
        {
            return null;
        }


    }
}
