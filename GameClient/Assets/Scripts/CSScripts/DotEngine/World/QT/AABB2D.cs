using System;
using UnityEngine;

namespace DotEngine.World.QT
{
    /// <summary>
    /// 四叉树范围AABB
    /// </summary>
    [Serializable]
    public struct AABB2D
    {
        /// <summary>
        /// AABB中心坐标
        /// </summary>
        public Vector2 Center { get; set; }
        /// <summary>
        /// AABB的扩展
        /// </summary>
        public Vector2 Extents { get; set; }

        public Vector2 HalfExtents => Extents * 0.5f;
        /// <summary>
        /// AABB的长度及宽度
        /// </summary>
        public Vector2 Size => Extents * 2;

        public float MinX => Center.x - Extents.x;
        public float MaxX => Center.x + Extents.y;
        public float MinY => Center.y - Extents.y;
        public float MaxY => Center.y + Extents.y;

        public AABB2D(Vector2 center,Vector2 extents)
        {
            Center = center;
            Extents = extents;
        }

        public AABB2D(float minX,float minY,float width,float height)
        {
            Extents = new Vector2(width, height) * 0.5f;
            Center = new Vector2(minX, minY) + Extents;
        }
        /// <summary>
        /// 判断AABB是否包含指定的点
        /// </summary>
        /// <param name="point">二维点</param>
        /// <returns></returns>
        public bool Contains(Vector2 point)
        {
            if (point.x < Center.x - Extents.x)
            {
                return false;
            }
            if (point.x > Center.x + Extents.x)
            {
                return false;
            }

            if (point.y < Center.y - Extents.y)
            {
                return false;
            }
            if (point.y > Center.y + Extents.y)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 判断是否完全包含指定的AABB
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Contains(AABB2D b)
        {
            return Contains(b.Center + new Vector2(-b.Extents.x, -b.Extents.y)) &&
                    Contains(b.Center + new Vector2(-b.Extents.x, b.Extents.y)) &&
                    Contains(b.Center + new Vector2(b.Extents.x, -b.Extents.y)) &&
                    Contains(b.Center + new Vector2(b.Extents.x, b.Extents.y));
        }
        /// <summary>
        /// 判断是否与指定的AABB有交集
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Intersects(AABB2D b)
        {
            return (Mathf.Abs(Center.x - b.Center.x) < (Extents.x + b.Extents.x)) &&
                   (Mathf.Abs(Center.y - b.Center.y) < (Extents.y + b.Extents.y));
        }

        public override string ToString()
        {
            return $"center={Center},size={Size}";
        }
    }
}
