using DotEngine.Net.Stream;
using System;
using System.Net;

namespace DotEngine.Net.Message
{
    public class MessageWriter
    {
        private byte serialNumber = 0;
        private MemoryStreamEx bufferStream = new MemoryStreamEx();

        public MessageWriter()
        {
        }

        public byte[] EncodeMessage(int messageID)
        {
            return EncodeMessage(messageID, null);
        }

        public byte[] EncodeMessage(int messageID,byte[] dataBytes)
        {
            bufferStream.Clear();

            ++serialNumber;

            int messageTotalLen = MessageConst.MESSAGE_MIN_LENGTH + (dataBytes != null ? dataBytes.Length : 0);
            int netMessageTotalLen = IPAddress.HostToNetworkOrder(messageTotalLen);
            byte[] netSizeBytes = BitConverter.GetBytes(netMessageTotalLen);
            bufferStream.Write(netSizeBytes, 0, netSizeBytes.Length);

            bufferStream.WriteByte(serialNumber);

            int netMessageID = IPAddress.HostToNetworkOrder(messageID);
            byte[] netMessageIDBytes = BitConverter.GetBytes(netMessageID);

            bufferStream.Write(netMessageIDBytes, 0, netMessageIDBytes.Length);

            if(dataBytes!=null)
            {
                bufferStream.Write(dataBytes, 0, dataBytes.Length);
            }

            return bufferStream.ToArray();
        }

        public virtual void Reset()
        {
            serialNumber = 0;
            bufferStream.Clear();
        }
    }
}
