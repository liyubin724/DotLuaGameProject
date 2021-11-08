namespace DotEngine.Net
{
    public interface IMessageEncryptor
    {
        byte[] Encrypt(byte[] dataBytes);
    }

    public interface IMessageCompressor
    {
        byte[] Compress(byte[] dataBytes);
    }

    public interface IMessageEncoder
    {
        IMessageEncryptor Encryptor { get; set; }
        IMessageCompressor Compressor { get; set; }

        byte[] Encode(int id, byte[] body,bool isEncrypt,bool isCompress);
    }
}
