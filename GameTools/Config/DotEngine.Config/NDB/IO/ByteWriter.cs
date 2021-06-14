using System;
using System.IO;
using System.Text;

namespace DotEngine.Config.NDB
{
    public static class ByteWriter
    {
        public static void WriteInt(Stream stream, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteBool(Stream stream, bool value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteLong(Stream stream, long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteFloat(Stream stream, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteString(Stream stream, string value)
        {
            int len = string.IsNullOrEmpty(value) ? 0 : value.Length;
            byte[] bytes = BitConverter.GetBytes(len);
            stream.Write(bytes, 0, bytes.Length);
            if (len > 0)
            {
                bytes = Encoding.UTF8.GetBytes(value);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public static void WriteIntArray(Stream stream, int[] array)
        {
            int len = array == null ? 0 : array.Length;
            byte[] bytes = BitConverter.GetBytes(len);
            stream.Write(bytes, 0, bytes.Length);
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    WriteInt(stream, array[i]);
                }
            }
        }

        public static void WriteBoolArray(Stream stream, bool[] array)
        {
            int len = array == null ? 0 : array.Length;
            byte[] bytes = BitConverter.GetBytes(len);
            stream.Write(bytes, 0, bytes.Length);
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    WriteBool(stream, array[i]);
                }
            }
        }

        public static void WriteLongArray(Stream stream, long[] array)
        {
            int len = array == null ? 0 : array.Length;
            byte[] bytes = BitConverter.GetBytes(len);
            stream.Write(bytes, 0, bytes.Length);
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    WriteLong(stream, array[i]);
                }
            }
        }

        public static void WriteFloatArray(Stream stream, float[] array)
        {
            int len = array == null ? 0 : array.Length;
            byte[] bytes = BitConverter.GetBytes(len);
            stream.Write(bytes, 0, bytes.Length);
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    WriteFloat(stream, array[i]);
                }
            }
        }

        public static void WriteStringArray(Stream stream, string[] array)
        {
            int len = array == null ? 0 : array.Length;
            byte[] bytes = BitConverter.GetBytes(len);
            stream.Write(bytes, 0, bytes.Length);
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    WriteString(stream, array[i]);
                }
            }
        }
    }
}
