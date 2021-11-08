using System;
using System.IO;

namespace DotEngine.Net
{
    public abstract class AMessageHandler : IMessageHandler
    {
        public IMessageCompressor Compressor { get; set; }
        public IMessageUncompressor Uncompressor { get; set; }
        public IMessageEncryptor Encryptor { get; set; }
        public IMessageDecryptor Decryptor { get; set; }

        private MemoryStream stream = new MemoryStream();

        public bool Deserialize(byte[] bytes, out int messageId, out object message)
        {
            int id = BitConverter.ToInt32(bytes, 0);
            int offset = sizeof(int);
            int index = BitConverter.ToInt32(bytes, offset);
            offset += sizeof(int);

            messageId = id;
            if (bytes.Length > offset)
            {
                byte[] bodyBytes = new byte[bytes.Length - offset];
                Array.Copy(bytes, offset, bodyBytes, 0, bodyBytes.Length);
                message = Decode(bodyBytes);
            }else
            {
                message = default;
            }
            ++deserializeIndex;
            if(index != deserializeIndex)
            {
                return false;
            }
            return true;
        }

        public byte[] Serialize(int messageId, object message)
        {
            ++serializeIndex;
            stream.SetLength(0);

            byte[] idBytes = BitConverter.GetBytes(messageId);
            byte[] indexBytes = BitConverter.GetBytes(serializeIndex);
            byte[] bodyBytes = Encode(message);
            if(bodyBytes!=null)
            {
                if(Encryptor!=null)
                {
                    bodyBytes = Encryptor.Encrypt(messageId, bodyBytes);
                }
                if(Compressor!=null)
                {
                    bodyBytes = Compressor.Compress(messageId, bodyBytes);
                }
            }
            stream.Write(idBytes, 0, idBytes.Length);
            stream.Write(indexBytes, 0, indexBytes.Length);
            if(bodyBytes!=null)
            {
                stream.Write(bodyBytes, 0, bodyBytes.Length);
            }
            return stream.ToArray();
        }

        protected abstract byte[] Encode(object message);
        protected abstract object Decode(byte[] bytes);
    }
}
