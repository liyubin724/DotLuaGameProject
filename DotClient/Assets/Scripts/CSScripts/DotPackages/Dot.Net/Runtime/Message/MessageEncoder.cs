using System;
using System.IO;

namespace DotEngine.Net
{
    public class MessageEncoder : IMessageEncoder
    {
        public IMessageEncryptor Encryptor { get; set; }
        public IMessageCompressor Compressor { get; set; }

        private MemoryStream stream = new MemoryStream();

        public byte[] Encode(int id, byte[] body, bool isEncrypt, bool isCompress)
        {
            stream.SetLength(0);

            byte[] idBytes = BitConverter.GetBytes(id);
            stream.Write(idBytes, 0, idBytes.Length);

            if(body!=null && body.Length>0)
            {
                byte[] dataBytes = body;
                if(isEncrypt && Encryptor!=null)
                {
                    dataBytes = Encryptor.Encrypt(dataBytes);
                    stream.WriteByte(1);
                }else
                {
                    stream.WriteByte(0);
                }

                if (isCompress && Compressor != null)
                {
                    dataBytes = Compressor.Compress(dataBytes);
                    stream.WriteByte(1);
                }
                else
                {
                    stream.WriteByte(0);
                }

                stream.Write(dataBytes, 0, dataBytes.Length);
            }

            return stream.ToArray();
        }
    }
}
