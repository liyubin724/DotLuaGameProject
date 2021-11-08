namespace DotEngine.Net
{
    public interface IMessageDecryptor
    {
        byte[] Decrypt(byte[] dataBytes);
    }

    public interface IMessageUncompressor
    {
        byte[] Uncompress(byte[] dataBytes);
    }

    public interface IMessageDecoder
    {
        IMessageDecryptor Decryptor { get; set; }
        IMessageUncompressor Uncompressor { get; set; }

        bool Decode(byte[] dataBytes, out int id, out byte[] body);
    }
}
