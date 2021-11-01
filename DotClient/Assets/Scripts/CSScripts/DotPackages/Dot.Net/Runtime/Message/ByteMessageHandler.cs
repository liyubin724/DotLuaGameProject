namespace DotEngine.Net
{
    public class ByteMessageHandler : AMessageHandler
    {
        protected override object Decode(byte[] bytes)
        {
            return bytes;
        }

        protected override byte[] Encode(object message)
        {
            return (byte[])message;
        }
    }
}
