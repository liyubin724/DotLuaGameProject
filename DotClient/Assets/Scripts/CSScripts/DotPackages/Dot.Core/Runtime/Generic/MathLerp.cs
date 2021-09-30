using UnityEngine;

namespace DotEngine.Generic
{
    public static class MathLerp
    {
        public static Vector3 Lerp(Vector3 point1,Vector3 point2,float t)
        {
            return point1 + (point2 - point1) * t;
        }

        //计算的夹角为[0,-180]，之间的SLerp插值
        public static Vector3 SLerp(Vector3 startDir,Vector3 endDir,float f)
        {
            float dot = Vector3.Dot(startDir, endDir);
            dot = Mathf.Clamp(dot, -1.0f, 1.0f);

            float theta = Mathf.Acos(dot) * f;
            Vector3 relativeVec = (endDir - startDir * dot).normalized;
            return startDir * Mathf.Cos(theta) + relativeVec * Mathf.Sin(theta);
        }
    }
}
