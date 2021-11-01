using System.Text;

namespace DotEngine.Net
{
    public enum StringEncodingType
    {
        UTF8 = 0,
        Unicode,
    }

    public class StringMessageHandler : AMessageHandler
    {
        private Encoding encoding;

        public StringMessageHandler(StringEncodingType encodingType = StringEncodingType.UTF8)
        {
            if (encodingType == StringEncodingType.UTF8)
            {
                encoding = Encoding.UTF8;
            }
            else if (encodingType == StringEncodingType.Unicode)
            {
                encoding = Encoding.Unicode;
            }
        }

        protected override object Decode(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return string.Empty;
            }
            return encoding.GetString(bytes);
        }

        protected override byte[] Encode(object message)
        {
            if (message == null)
            {
                return new byte[0];
            }
            string strMsg = (string)message;
            return encoding.GetBytes(strMsg);
        }
    }
}
