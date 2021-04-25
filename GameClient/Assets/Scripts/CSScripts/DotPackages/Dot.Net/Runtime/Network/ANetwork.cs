using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public abstract class ANetwork : INetworkHandler
    {
        private Dictionary<MessageCompressType, IMessageCompressor> compressorDic = new Dictionary<MessageCompressType, IMessageCompressor>();
        private Dictionary<MessageCryptoType, IMessageCryptor> cryptorDic = new Dictionary<MessageCryptoType, IMessageCryptor>();

        protected Socket socket;

        public void AddCompressor(MessageCompressType compressType, IMessageCompressor compressor)
        {
            if (!compressorDic.ContainsKey(compressType))
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

        public byte[] CompressMessage(MessageCompressType compressType, byte[] dataBytes)
        {
            if(compressorDic.TryGetValue(compressType,out var compressor))
            {
                return compressor.Compress(dataBytes);
            }
            return dataBytes;
        }

        public byte[] UncompressMessage(MessageCompressType compressType, byte[] dataBytes)
        {
            if(compressorDic.TryGetValue(compressType,out var compressor))
            {
                return compressor.Uncompress(dataBytes);
            }
            return dataBytes;
        }

        public byte[] DecodeMessage(MessageCryptoType cryptoType, byte[] dataBytes)
        {
            if(cryptorDic.TryGetValue(cryptoType,out var cryptor))
            {
                return cryptor.Decode(dataBytes);
            }
            return dataBytes;
        }

        public byte[] EncodeMessage(MessageCryptoType cryptoType, byte[] dataBytes)
        {
            if (cryptorDic.TryGetValue(cryptoType, out var cryptor))
            {
                return cryptor.Encode(dataBytes);
            }
            return dataBytes;
        }



        public abstract void OnMessageHandler(ANetworkSocket socket, int messageID, byte[] dataBytes);

        public void OnOperationLog(ENetworkOperation operation, string log)
        {
            throw new NotImplementedException();
        }


    }
}
