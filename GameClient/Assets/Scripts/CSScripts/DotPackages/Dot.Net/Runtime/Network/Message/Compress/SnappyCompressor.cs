namespace DotEngine.Net
{
    public class SnappyCompressor : IMessageCompressor
    {
        public byte[] Compress(byte[] datas)
        {
            return datas;
        }

        public byte[] Uncompress(byte[] datas)
        {
            return datas;
        }
    }
}
