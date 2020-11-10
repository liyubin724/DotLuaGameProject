using UnityEngine;

namespace DotEngine.World.QT
{
    public static class QuadTreeUtil
    {
        public static readonly Color32[] GizmosColors = new Color32[10]
        {
            new Color32(0, 0, 0,255),
            new Color32(74, 20, 140,255),
            new Color32(123, 31, 162,255),
            new Color32(57, 73, 171,255),
            new Color32(0, 96, 100,255),
            new Color32(56, 142, 60,255),
            new Color32(251, 192, 45,255),
            new Color32(245, 127, 23,255),
            new Color32(93, 64, 55,255),
            new Color32(69, 90, 100,255),
        };

        public static Color GetGizmosColor(int depth)
        {
            return GizmosColors[depth % GizmosColors.Length];
        }

        public static void DrawGizmoAABBBorder(AABB2D bounds,Color color)
        {
            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            float minX = bounds.MinX;
            float maxX = bounds.MaxX;
            float minY = bounds.MinY;
            float maxY = bounds.MaxY;

            Gizmos.DrawLine(new Vector3(minX, 0, minY), new Vector3(maxX, 0, minY));
            Gizmos.DrawLine(new Vector3(minX, 0, maxY), new Vector3(maxX, 0, maxY));
            Gizmos.DrawLine(new Vector3(minX, 0, minY), new Vector3(minX, 0, maxY));
            Gizmos.DrawLine(new Vector3(maxX, 0, minY), new Vector3(maxX, 0, maxY));

            Gizmos.color = oldColor;
        }

        public static void DrawGizmoAABBCross(AABB2D bounds,Color color)
        {
            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            float minX = bounds.MinX;
            float maxX = bounds.MaxX;
            float minY = bounds.MinY;
            float maxY = bounds.MaxY;
            float extentX = bounds.Extents.x;
            float extentY = bounds.Extents.y;

            Gizmos.DrawLine(new Vector3(minX, 0, minY + extentY), new Vector3(maxX, 0, minY + extentY));
            Gizmos.DrawLine(new Vector3(minX + extentX, 0, minY), new Vector3(minX + extentX, 0, maxY));

            Gizmos.color = oldColor;
        }
    }
}
