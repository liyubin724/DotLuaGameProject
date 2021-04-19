namespace DotEngine.Net.Message
{
    public interface IMessageParser
    {
        string SecretKey { get; set; }

        byte[] EncodeMessage(int messageID, object message);
        
        object DecodeMessage(int messageID, byte[] bytes);
    }
}
