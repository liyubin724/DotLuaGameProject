using System;
using System.IO;

namespace DotEngine.NetCore.TCPNetwork
{
    public class DefaultMessageEncoder : IMessageEncoder
    {
        private int serial = 0;
        private MemoryStream stream = new MemoryStream();

        public byte[] EncodeMessage(int msgID, byte[] dataBytes)
        {
            stream.SetLength(0);
            
            ++serial;

            byte[] msgIDBytes = BitConverter.GetBytes(msgID);
            byte[] serialBytes = BitConverter.GetBytes(serial);

            int len = msgIDBytes.Length + serialBytes.Length;
            if (dataBytes != null)
            {
                len += dataBytes.Length;
            }

            byte[] lenBytes = BitConverter.GetBytes(len);
            stream.Write(lenBytes, 0, lenBytes.Length);
            stream.Write(msgIDBytes, 0, msgIDBytes.Length);
            stream.Write(serialBytes, 0, serialBytes.Length);
            if (dataBytes != null)
            {
                stream.Write(dataBytes, 0, dataBytes.Length);
            }

            return stream.ToArray();
        }
    }
}
