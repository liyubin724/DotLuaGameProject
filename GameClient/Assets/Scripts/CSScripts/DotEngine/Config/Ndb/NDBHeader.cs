using System.Runtime.InteropServices;

namespace DotEngine.Config.Ndb
{
    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    public struct NDBHeader
    {
        public static int Size = Marshal.SizeOf(typeof(NDBHeader));

        public int version;

        public int dataCount;
        public int fieldCount;

        public int dataSize;

        public int dataOffset;
        public int stringOffset;
        public int arrayOffset;
    }
}
