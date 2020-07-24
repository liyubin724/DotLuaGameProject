using System;
using System.Runtime.InteropServices;

namespace DotEngine.Config.Ndb
{
    public static class NDBConst
    {
        public static int GetFieldSize(NDBFieldType fieldType)
        {
            if (fieldType == NDBFieldType.Null)
            {
                return 0;
            }else  if(fieldType == NDBFieldType.Bool)
            {
                return sizeof(bool);
            }else if(fieldType == NDBFieldType.Float)
            {
                return sizeof(float);
            }else if(fieldType == NDBFieldType.Long)
            {
                return sizeof(long);
            }else
            {
                return sizeof(int);
            }
        }

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

        public static T ByteToStruct<T>(byte[] bytes, int startIndex, int size)
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
