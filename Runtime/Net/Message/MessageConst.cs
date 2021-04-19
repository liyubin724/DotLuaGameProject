namespace DotEngine.Net.Message
{
    public enum MessageErrorCode
    {
        Reader_ReadSerialNumberError = 100,
        Reader_ReadMessageIDError,
        Reader_CompareMessageDataLengthError,
        Reader_CompareSerialNumberError,
    }

    public class MessageConst
    {
        //最小的消息的长度，总长度+序号+消息ID
        public static readonly int MESSAGE_MIN_LENGTH = 0;

        static MessageConst()
        {
            MESSAGE_MIN_LENGTH = sizeof(int) //Total Length
                + sizeof(byte) //Serial Number
                + sizeof(int); //Message ID
        }
    }
}
