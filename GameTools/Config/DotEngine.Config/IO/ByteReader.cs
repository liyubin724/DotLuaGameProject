using System.Text;

namespace DotEngine.Config.IO
{
    internal static unsafe class ByteReader
    {
        public static int ReadInt(byte[] bytes, int startIndex, out int offset)
        {
            offset = sizeof(int);
            fixed (byte* bytePtr = &bytes[startIndex])
            {
                return *((int*)bytePtr);
            }
        }

        public static bool ReadBool(byte[] bytes, int startIndex, out int offset)
        {
            offset = sizeof(bool);
            fixed (byte* bytePtr = &bytes[startIndex])
            {
                return *((bool*)bytePtr);
            }
        }

        public static long ReadLong(byte[] bytes, int startIndex, out int offset)
        {
            offset = sizeof(long);
            fixed (byte* bytePtr = &bytes[startIndex])
            {
                return *((long*)bytePtr);
            }
        }

        public static float ReadFloat(byte[] bytes, int startIndex, out int offset)
        {
            offset = sizeof(float);
            fixed (byte* bytePtr = &bytes[startIndex])
            {
                return *((float*)bytePtr);
            }
        }

        public static string ReadString(byte[] bytes, int startIndex, out int offset)
        {
            offset = sizeof(int);
            fixed (byte* bytePtr = &bytes[startIndex])
            {
                int len = *((int*)bytePtr);
                offset += len;

                return Encoding.UTF8.GetString(bytes, startIndex + sizeof(int), len);
            }
        }

        public static int[] ReadIntArray(byte[] bytes, int startIndex, out int offset)
        {
            fixed (byte* bytePtr = &bytes[startIndex])
            {
                int len = *((int*)bytePtr);

                int[] result = new int[len];
                offset = sizeof(int);
                for (int i = 0; i < len; ++i)
                {
                    offset = sizeof(int);
                    result[i] = *((int*)(bytePtr + offset));
                }

                return result;
            }
        }

        public static bool[] ReadBoolArray(byte[] bytes,int startIndex,out int offset)
        {
            fixed (byte* bytePtr = &bytes[startIndex])
            {
                int len = *((int*)bytePtr);

                bool[] result = new bool[len];
                offset = sizeof(int);
                for (int i = 0; i < len; ++i)
                {
                    offset = sizeof(bool);
                    result[i] = *((bool*)(bytePtr + offset));
                }

                return result;
            }
        }

        public static long[] ReadLongArray(byte[] bytes, int startIndex, out int offset)
        {
            fixed (byte* bytePtr = &bytes[startIndex])
            {
                int len = *((int*)bytePtr);

                long[] result = new long[len];
                offset = sizeof(int);
                for (int i = 0; i < len; ++i)
                {
                    offset = sizeof(long);
                    result[i] = *((long*)(bytePtr + offset));
                }

                return result;
            }
        }

        public static float[] ReadFloatArray(byte[] bytes, int startIndex, out int offset)
        {
            fixed (byte* bytePtr = &bytes[startIndex])
            {
                int len = *((int*)bytePtr);

                float[] result = new float[len];
                offset = sizeof(int);
                for (int i = 0; i < len; ++i)
                {
                    offset = sizeof(float);
                    result[i] = *((float*)(bytePtr + offset));
                }

                return result;
            }
        }

        public static string[] ReadStringArray(byte[] bytes, int startIndex, out int offset)
        {
            fixed (byte* bytePtr = &bytes[startIndex])
            {
                int len = *((int*)bytePtr);

                string[] result = new string[len];
                offset = sizeof(int);
                for (int i = 0; i < len; ++i)
                {
                    int strLen = *((int*)(bytePtr+ offset));
                    offset += sizeof(int);

                    result[i] = Encoding.UTF8.GetString(bytes, startIndex + offset, strLen);

                    offset += strLen;
                }

                return result;
            }
        }
    }
}
