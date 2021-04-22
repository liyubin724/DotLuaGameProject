using System.Net.Sockets;

namespace DotEngine.Net
{
    public enum NetworkSessionState
    {
        Unavailable = 0,
        Connecting,
        Normal,
        ConnectedFailed,
        Disconnected,
    }

    public interface INetworkSession
    {
        Socket NetSocket { get; }
        INetworkSessionHandler SessionHandler { get; }

        void BindSocket(Socket socket, INetworkSessionHandler handler);

        void AddCompressor(MessageCompressType compressType, IMessageCompressor compressor);
        void RemoveCompressor(MessageCompressType compressType);

        void AddCryptor(MessageCryptoType cryptoType, IMessageCryptor cryptor);
        void RemoveCryptor(MessageCryptoType cryptoType);

        byte[] Serialize(int messageID, MessageCompressType compressType, MessageCryptoType cryptoType, byte[] dataBytes);
        byte Desrialize(byte[] bytes, out int messageID, out byte[] dataBytes);

        void OnDataReceived(byte[] bytes, int size);

        void OnMessageReceived(int messageID, byte[] dataBytes);
        void SendMessage(int messageID, MessageCompressType compressType, MessageCryptoType cryptoType, byte[] dataBytes);

        void DoConnect(string ip, int port);
        void DoConnect(int port, int maxCount);
        void DoReceive();
        void DoDisconnect();

        void Dispose();
    }
}
