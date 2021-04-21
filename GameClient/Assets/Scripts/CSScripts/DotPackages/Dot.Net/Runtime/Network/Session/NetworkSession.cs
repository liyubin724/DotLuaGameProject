using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public class NetworkSession : INetworkSession
    {
        private NetMessageBuffer receivedMessageBuffer = new NetMessageBuffer();
        private Dictionary<MessageCompressType, IMessageCompressor> compressorDic = new Dictionary<MessageCompressType, IMessageCompressor>();
        private Dictionary<MessageCryptoType, IMessageCryptor> cryptorDic = new Dictionary<MessageCryptoType, IMessageCryptor>();

        public Socket NetSocket => throw new NotImplementedException();

        public void AddCompressor(MessageCompressType compressType, IMessageCompressor compressor)
        {
            if(!compressorDic.ContainsKey(compressType))
            {
                compressorDic.Add(compressType, compressor);
            }
        }

        public void RemoveCompressor(MessageCompressType compressType)
        {
            if (compressorDic.ContainsKey(compressType))
            {
                compressorDic.Remove(compressType);
            }
        }

        public void AddCryptor(MessageCryptoType cryptoType, IMessageCryptor cryptor)
        {
            if (!cryptorDic.ContainsKey(cryptoType))
            {
                cryptorDic.Add(cryptoType, cryptor);
            }
        }

        public void RemoveCryptor(MessageCryptoType cryptoType)
        {
            if (cryptorDic.ContainsKey(cryptoType))
            {
                cryptorDic.Remove(cryptoType);
            }
        }

        public void OnDataReceived(byte[] bytes,int size)
        {
            receivedMessageBuffer.WriteBytes(bytes, size);
        }

        public byte[] Serialize(int messageID)
        {
            return Serialize(messageID, null);
        }

        public byte[] Serialize(int messageID, byte[] dataBytes)
        {
            return null;
        }

        public void SendMessage(int messageID,MessageCompressType compressType,MessageCryptoType cryptoType,byte[] dataBytes)
        {

        }

        public void SendMessage(int messageID)
        {

        }
    }
}
