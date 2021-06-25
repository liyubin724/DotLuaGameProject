namespace DotEngine.NetCore.TCPNetwork
{
    public interface IMessageEncoder
    {
        byte[] EncodeMessage(int msgID, byte[] msgBytes);
    }
}
