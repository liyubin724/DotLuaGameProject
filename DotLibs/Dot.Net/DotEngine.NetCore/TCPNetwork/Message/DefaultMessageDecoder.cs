using DotEngine.NetCore.IO;
using System;

namespace DotEngine.NetCore.TCPNetwork
{
    public class DefaultMessageDecoder : IMessageDecoder
    {
        private int serial = 0;

        public bool DecodeMessage(byte[] dataBytes, out int msgID, out byte[] msgBytes)
        {
            int startIndex = 0;
            msgBytes = null;

            ++serial;

            int offset;
            msgID = ByteReader.ReadInt(dataBytes, startIndex, out offset);
            startIndex += offset;

            int tmpSerial = ByteReader.ReadInt(dataBytes, startIndex, out offset);
            if (tmpSerial != serial)
            {
                return false;
            }
            startIndex += offset;

            if (dataBytes.Length > startIndex)
            {
                msgBytes = new byte[dataBytes.Length - startIndex];
                Array.Copy(dataBytes, startIndex, msgBytes, 0, msgBytes.Length);
            }

            return true;
        }
    }
}
