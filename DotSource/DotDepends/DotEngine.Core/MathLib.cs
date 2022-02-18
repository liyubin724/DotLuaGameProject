using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine
{
    public static class MathLib
    {
        public static int RoundToInt(float f) { return (int)Math.Round(f); }
        public static int FloorToInt(float f) { return (int)Math.Floor(f); }
        public static int CeilToInt(float f) { return (int)Math.Ceiling(f); }
    }
}
