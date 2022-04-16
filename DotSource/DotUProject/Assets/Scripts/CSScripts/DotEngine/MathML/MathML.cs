using UnityEngine;

namespace DotEngine
{
    public class MathML
    {
        public static Vector3 Lerp(Vector3 point1, Vector3 point2, float t)
        {
            return point1 + (point2 - point1) * t;
        }

        //计算的夹角为[0,-180]，之间的SLerp插值
        public static Vector3 SLerp(Vector3 startDir, Vector3 endDir, float f)
        {
            float dot = Vector3.Dot(startDir, endDir);
            dot = Mathf.Clamp(dot, -1.0f, 1.0f);

            float theta = Mathf.Acos(dot) * f;
            Vector3 relativeVec = (endDir - startDir * dot).normalized;
            return startDir * Mathf.Cos(theta) + relativeVec * Mathf.Sin(theta);
        }

        public static Vector2 GetPointProjectInLine(Vector2 linePoint1,Vector2 linePoint2,Vector2 point)
        {
            float a1 = linePoint2.y - linePoint1.y;
            float b1 = linePoint1.x - linePoint2.x;

            float c1 = a1 * linePoint1.x + b1 * linePoint1.y;
            float c2 = -b1 * point.x + a1 * point.y;
            float det = a1 * a1 + b1 * b1;
            float cx = 0, cy = 0;
            if (det != 0)
            {
                cx = (a1 * c1 - b1 * c2) / det;
                cy = (a1 * c2 + b1 * c1) / det;
            }
            else
            {
                cx = point.x;
                cy = point.y;
            }
            return new Vector2(cx, cy);
        }

    }
}
