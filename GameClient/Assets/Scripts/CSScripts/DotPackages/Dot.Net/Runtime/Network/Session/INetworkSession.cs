using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public enum MessageCompressType
    {
        None = 0,
        Snappy,
    }

    public enum MessageCryptoType
    {
        None = 0,
    }

    public interface INetworkSession : IMessageSerializer, IMessageDeserializer
    {
        Socket NetSocket { get; set; }

        void AddCompressor(MessageCompressType compressType, IMessageCompressor compressor);
        void RemoveCompressor(MessageCompressType compressType);

        void AddCryptor(MessageCryptoType cryptoType, IMessageCryptor cryptor);
        void RemoveCryptor(MessageCryptoType cryptoType);


    }
}
