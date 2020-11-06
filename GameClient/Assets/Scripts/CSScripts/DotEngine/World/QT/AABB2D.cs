using System;
using UnityEngine;

namespace DotEngine.World.QT
{
    [Serializable]
    public struct AABB2D
    {
        public Vector2 Center { get; set; }
        public Vector2 Extents { get; set; }

        public Vector2 HalfExtents => Extents * 0.5f;

        public Vector2 Size => Extents * 2;

        public Vector2 LBPoint => Center - Extents;
        public Vector2 RBPoint => Center + new Vector2(Extents.x, -Extents.y);
        public Vector2 LTPoint => Center + new Vector2(-Extents.x, Extents.y);
        public Vector2 RTPoint => Center + Extents;

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

        public bool Contains(AABB2D b)
        {
            return Contains(b.Center + new Vector2(-b.Extents.x, -b.Extents.y)) &&
                    Contains(b.Center + new Vector2(-b.Extents.x, b.Extents.y)) &&
                    Contains(b.Center + new Vector2(b.Extents.x, -b.Extents.y)) &&
                    Contains(b.Center + new Vector2(b.Extents.x, b.Extents.y));
        }

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
