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
    }
}
