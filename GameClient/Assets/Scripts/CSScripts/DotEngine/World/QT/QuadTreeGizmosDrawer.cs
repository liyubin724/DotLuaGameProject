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
            Color oldColor = Gizmos.color;

            List<IQuadObject> objects = node.InsideObjects;
            foreach (var obj in objects)
            {
                DrawAABB2D(obj.Bounds, Color.blue);
            }

            Gizmos.color = QuadTreeUtil.GetGizmosColor(node.Depth);

            Vector2 lbPoint = node.Bounds.LBPoint;
            Vector2 rtPoint = node.Bounds.RTPoint;

            Vector2 extents = node.Bounds.Extents;

            if (node.ParentNode == null)
            {
                Gizmos.DrawLine(new Vector3(lbPoint.x, 0, lbPoint.y), new Vector3(lbPoint.x, 0, rtPoint.y));
                Gizmos.DrawLine(new Vector3(lbPoint.x, 0, lbPoint.y), new Vector3(rtPoint.x, 0, lbPoint.y));

                Gizmos.DrawLine(new Vector3(rtPoint.x, 0, rtPoint.y), new Vector3(lbPoint.x, 0, rtPoint.y));
                Gizmos.DrawLine(new Vector3(rtPoint.x, 0, rtPoint.y), new Vector3(rtPoint.x, 0, lbPoint.y));
            }

            if(!node.IsLeaf)
            {
                Gizmos.color = QuadTreeUtil.GetGizmosColor(node.Depth+1);
                Gizmos.DrawLine(new Vector3(lbPoint.x, 0, lbPoint.y+extents.y), new Vector3(rtPoint.x, 0, lbPoint.y+extents.y));
                Gizmos.DrawLine(new Vector3(lbPoint.x+ extents.x, 0, lbPoint.y), new Vector3(lbPoint.x+extents.x, 0, rtPoint.y));

                QuadNode[] childNodes = node.ChildNodes;
                foreach (var childNode in childNodes)
                {
                    DrawQuadNode(childNode);
                }
            }

            Gizmos.color = oldColor;
        }

        private void DrawAABB2D(AABB2D bounds,Color color)
        {
            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            Vector2 lbPoint = bounds.LBPoint;
            Vector2 rtPoint = bounds.RTPoint;

            Gizmos.DrawLine(new Vector3(lbPoint.x, 0, lbPoint.y), new Vector3(lbPoint.x, 0, rtPoint.y));
            Gizmos.DrawLine(new Vector3(lbPoint.x, 0, lbPoint.y), new Vector3(rtPoint.x, 0, lbPoint.y));

            Gizmos.DrawLine(new Vector3(rtPoint.x, 0, rtPoint.y), new Vector3(lbPoint.x, 0, rtPoint.y));
            Gizmos.DrawLine(new Vector3(rtPoint.x, 0, rtPoint.y), new Vector3(rtPoint.x, 0, lbPoint.y));

            Gizmos.color = oldColor;
        }
    }
}
