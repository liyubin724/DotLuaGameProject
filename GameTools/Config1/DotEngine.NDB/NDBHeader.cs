using System.Runtime.InteropServices;

namespace DotEngine.NDB
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NDBHeader
    {
        public int fieldCount;
        public int lineCount;

        public int lineSize;

        public int fieldOffset;
        public int lineOffset;
        public int stringOffset;
    }
}
