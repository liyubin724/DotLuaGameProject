namespace DotEngine.NetCore.TCPNetwork
{
    public interface IMessageDecoder
    {
        bool DecodeMessage(byte[] dataBytes, out int msgID, out byte[] msgBytes);
    }
}
