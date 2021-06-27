using UnityEngine;

namespace DotEngine.Generic
{
    public static class MathCatmullRom
    {
        //计算p1至p2之间的点
        //如果p0是起点，计算p0至p1之间的可以使用（p0,p0,p1,p2,t)
        //如果p3是终点,计算p2至p3之间的可以使用(p1,p2,p3,p3,t)
        public static Vector3 CatmullRomCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            return p1 + (0.5f * (p2 - p0) * t) + 0.5f * (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t + 0.5f * (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t;
        }
    }
}
