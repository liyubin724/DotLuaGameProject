using DotEngine.Config.IO;
using System.IO;
using System.Text;

namespace DotEngine.Config.NDB
{
    public static unsafe class NDBByteUtility
    {
        public static NDBField ReadField(byte[] bytes,int startIndex,out int offset)
        {
            offset = 0;

            NDBField field = new NDBField();
            fixed (byte* bytePtr = &bytes[startIndex])
            {
                field.Type = (NDBFieldType)(*(bytePtr + offset));
                offset += sizeof(byte);

                int len = *((int*)(bytePtr+offset));
                offset += sizeof(int);

                field.Name = Encoding.UTF8.GetString(bytes, startIndex + offset, len);
                offset += len;
            }

            return field;
        }

        public static void WriteField(Stream stream,NDBField field)
        {
            stream.WriteByte((byte)field.Type);
            ByteWriter.WriteString(stream, field.Name);
        }
    }
}
