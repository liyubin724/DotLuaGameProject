using UnityEditor.Build.Pipeline;
using UnityEngine;

namespace DotEngine.Generic
{
    public partial class MathLib
    {
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
