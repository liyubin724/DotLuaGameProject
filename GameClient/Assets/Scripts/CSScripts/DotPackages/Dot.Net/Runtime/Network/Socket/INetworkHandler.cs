using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public enum ENetworkOperation
    {

    }

    public interface INetworkHandler
    {
        byte[] EncodeMessage(MessageCryptoType cryptoType, byte[] dataBytes);
        byte[] DecodeMessage(MessageCryptoType cryptoType, byte[] dataBytes);
        byte[] CompressMessage(MessageCompressType compressType, byte[] dataBytes);
        byte[] UncompressMessage(MessageCompressType compressType, byte[] dataBytes);

        void OnMessageHandler(ANetworkSocket socket, int messageID, byte[] dataBytes);
        void OnOperationLog(ENetworkOperation operation, string log);
    }
}
