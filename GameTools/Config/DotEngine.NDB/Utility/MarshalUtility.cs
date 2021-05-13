using System;
using System.Runtime.InteropServices;

namespace DotEngine.NDB
{
    public static class MarshalUtility
    {
        public static byte[] StructToByte(object structObject, int size)
        {
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structObject, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static T ByteToStruct<T>(byte[] bytes, int startIndex, int size) where T : struct
        {
            if (bytes == null || bytes.Length <= 0)
            {
                return default(T);
            }

            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, startIndex, buffer, size);
                T result = (T)Marshal.PtrToStructure(buffer, typeof(T));
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

    }
}
