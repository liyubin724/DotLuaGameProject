﻿using UnityEngine;

namespace DotEngine.Utilities
{
    public static class RectUtility
    {
        public static bool Contains(this Rect rect,Rect targetRect)
        {
            float xMin1 = rect.xMin;
            float yMin1 = rect.yMin;
            float xMax1 = rect.xMax;
            float yMax1 = rect.yMax;

            float xMin2= targetRect.xMin;
            float yMin2= targetRect.yMin;
            float xMax2 = targetRect.xMax;
            float yMax2 = targetRect.yMax;

            return xMin1 < xMin2
                && yMin1 <= yMin2
                && xMax1 >= xMax2
                && yMax1 >= yMax2;
        }

        public static bool IntersectsWith(this Rect rect,Rect targetRect)
        {
            Vector2 lbPoint = new Vector2(targetRect.xMin, targetRect.yMin);
            Vector2 rbPoint = new Vector2(targetRect.xMin, targetRect.yMax);
            Vector2 ltPoint = new Vector2(targetRect.xMax, targetRect.yMin);
            Vector2 rtPoint = new Vector2(targetRect.xMax, targetRect.yMax);

            return rect.Contains(lbPoint) || rect.Contains(rbPoint) || rect.Contains(ltPoint) || rect.Contains(rtPoint);
        }
    }
}
