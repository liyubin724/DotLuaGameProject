using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.World.QT
{
    public class QuadTreeGizmosDrawer : MonoBehaviour
    {
        private QuadTree m_QuadTree = null;
        public static QuadTreeGizmosDrawer DrawGizmos(QuadTree tree)
        {
            GameObject gObj = new GameObject("QuadTree Drawer");
            QuadTreeGizmosDrawer drawer = gObj.AddComponent<QuadTreeGizmosDrawer>();
            drawer.m_QuadTree = tree;

            return drawer;
        }

        public static void DestroyGizmos(QuadTreeGizmosDrawer drawer)
        {
            GameObject.Destroy(drawer.gameObject);
        }

        private void OnDrawGizmos()
        {
            if(m_QuadTree!=null && m_QuadTree.Root!=null)
            {
                DrawQuadNode(m_QuadTree.Root);
            }
        }

        private void DrawQuadNode(QuadNode node)
        {
            List<IQuadObject> objects = node.InsideObjects;
            foreach (var obj in objects)
            {
                QuadTreeUtil.DrawGizmoAABBBorder(obj.Bounds, Color.blue);
            }

            if (node.ParentNode == null)
            {
                QuadTreeUtil.DrawGizmoAABBBorder(node.Bounds, QuadTreeUtil.GetGizmosColor(node.Depth));
            }

            if(!node.IsLeaf)
            {
                QuadTreeUtil.DrawGizmoAABBCross(node.Bounds, QuadTreeUtil.GetGizmosColor(node.Depth + 1));

                QuadNode[] childNodes = node.ChildNodes;
                foreach (var childNode in childNodes)
                {
                    DrawQuadNode(childNode);
                }
            }
        }
    }
}
