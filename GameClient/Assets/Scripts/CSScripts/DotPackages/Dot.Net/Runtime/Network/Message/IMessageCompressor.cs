namespace DotEngine.Net
{
    public interface IMessageCompressor
    {
        byte[] Compress(byte[] datas);
        byte[] Uncompress(byte[] datas);
    }
}
