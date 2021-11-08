namespace DotEngine.Net
{ 
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
