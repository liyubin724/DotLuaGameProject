namespace DotEngine.Net
{
    public interface IMessageEncryptor
    {
        byte[] Encrypt(int id, byte[] body);
    }

    public interface IMessageDecryptor
    {
        byte[] Decrypt(int id, byte[] body);
    }

    public interface IMessageCompressor
    {
        byte[] Compress(int id, byte[] body);
    }

    public interface IMessageUncompressor
    {
        byte[] Uncompress(int id, byte[] body);
    }

    public interface  IMessageHandler
    {
        IMessageCompressor Compressor { get; set; }
        IMessageUncompressor Uncompressor { get; set; }

        IMessageEncryptor Encryptor { get; set; }
        IMessageDecryptor Decryptor { get; set; }

        byte[] Serialize(int messageId, object message);
        bool Deserialize(byte[] bytes, out int messageId, out object message);
    }
}
