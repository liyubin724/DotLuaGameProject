﻿using UnityEngine;

namespace DotEngine.Generic
{
    public class Bezier
    {
        //一阶贝塞尔曲线
        public static Vector2 Linear(Vector2 point0, Vector2 point1, float t)
        {
            t = Mathf.Clamp01(t);
            return (1 - t) * point0 + t * point1;
        }

        public static Vector3 Linear(Vector3 point0, Vector3 point1, float t)
        {
            t = Mathf.Clamp01(t);
            return (1 - t) * point0 + t * point1;
        }

        //二阶贝塞尔曲线
        public static Vector2 Quadratic(Vector2 point0, Vector2 point1, Vector2 point2, float t)
        {
            t = Mathf.Clamp01(t);
            return (1 - t) * (1 - t) * point0 + 2 * t * (1 - t) * point1 + t * t * point2;
        }

        public static Vector3 Quadratic(Vector3 point0, Vector3 point1, Vector3 point2, float t)
        {
            t = Mathf.Clamp01(t);
            return (1 - t) * (1 - t) * point0 + 2 * t * (1 - t) * point1 + t * t * point2;
        }

        //三阶贝塞尔曲线
        public static Vector2 Cubic(Vector2 point0, Vector2 point1, Vector2 point2, Vector2 point3, float t)
        {
            t = Mathf.Clamp01(t);
            return (1 - t) * (1 - t) * (1 - t) * point0 + 3 * t * (1 - t) * (1 - t) * point1 + 3 * t * t * (1 - t) * point2 + t * t * t * point3;
        }

        public static Vector3 Cubic(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float t)
        {
            t = Mathf.Clamp01(t);
            return (1 - t) * (1 - t) * (1 - t) * point0 + 3 * t * (1 - t) * (1 - t) * point1 + 3 * t * t * (1 - t) * point2 + t * t * t * point3;
        }
    }
}
