namespace DotEngine.Net
{
    public interface IMessageSerializer
    {
        byte[] Serialize(int messageID);
        byte[] Serialize(int messageID, byte[] dataBytes);
    }
}
