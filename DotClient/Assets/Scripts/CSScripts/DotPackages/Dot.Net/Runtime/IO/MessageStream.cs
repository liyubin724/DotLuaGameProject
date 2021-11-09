using System;
using System.IO;

namespace DotEngine.Net
{
    public class MessageStream : MemoryStream
    {
        private byte[] intBuffer = new byte[sizeof(int)];

        public bool ReadInt(int startIndex, out int value)
        {
            value = 0;
            if (startIndex + sizeof(int) <= Length)
            {
                Seek(startIndex, SeekOrigin.Begin);
                Read(intBuffer, 0, intBuffer.Length);
                value = BitConverter.ToInt32(intBuffer, 0);

                return true;
            }
            return false;
        }

        public bool ReadByte(int startIndex, out byte value)
        {
            value = 0;
            if (startIndex + sizeof(byte) <= Length)
            {
                Seek(startIndex, SeekOrigin.Begin);
                value = (byte)ReadByte();

                return true;
            }
            return false;
        }

        public void Clear()
        {
            SetLength(0);
            Seek(0, SeekOrigin.Begin);
        }
    }
}
