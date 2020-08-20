using UnityEngine;

namespace DotEngine.Utilities
{
    public static class RectUtility
    {
        public static bool Contains(this Rect rect,Rect targetRect)
        {
            return rect.xMin < targetRect.xMin
                && rect.yMin <= targetRect.yMin
                && rect.xMax >= targetRect.xMax
                && rect.yMax >= targetRect.yMax;
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
