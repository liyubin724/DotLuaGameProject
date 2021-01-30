using DotEngine.Net.Stream;
using System.Net;

namespace DotEngine.Net.Message
{
    public delegate void OnMessageReceived(int messageID, byte[] msgBytes);
    public delegate void OnMessageError(MessageErrorCode errorCode);

    public class MessageReader
    {
        public OnMessageReceived MessageReceived { get; set; } = null;
        public OnMessageError MessageError { get; set; } = null;

        private byte serialNumber = 0;
        private BufferStream bufferStream = new BufferStream();

        public MessageReader()
        {
        }

        public void OnDataReceived(byte[] bytes, int size)
        {
            MemoryStreamEx stream = bufferStream.GetActivedStream();
            stream.Write(bytes, 0, size);
        }

        public void DoReadData()
        {
            MemoryStreamEx stream = bufferStream.GetActivedStream();
            int streamLength = (int)stream.Length;
            if (streamLength >= MessageConst.MESSAGE_MIN_LENGTH)
            {
                int startIndex = 0;
                while (true)
                {
                    if (!stream.ReadInt(startIndex, out int totalLength))
                    {
                        break;
                    }
                    totalLength = IPAddress.NetworkToHostOrder(totalLength);
                    if (streamLength - startIndex < totalLength)
                    {
                        break;
                    }

                    int offsetIndex = startIndex;
                    offsetIndex += sizeof(int);

                    if (!stream.ReadByte(offsetIndex, out byte serialNum))
                    {
                        MessageError?.Invoke(MessageErrorCode.Reader_ReadSerialNumberError);
                        break;
                    }
                    if (serialNumber + 1 != serialNum)
                    {
                        MessageError?.Invoke(MessageErrorCode.Reader_CompareSerialNumberError);
                        break;
                    }

                    offsetIndex += sizeof(byte);
                    if (!stream.ReadInt(offsetIndex, out int messageID))
                    {
                        MessageError?.Invoke(MessageErrorCode.Reader_ReadMessageIDError);
                        break;
                    }

                    messageID = IPAddress.NetworkToHostOrder(messageID);

                    offsetIndex += sizeof(int);
                    if (offsetIndex < totalLength + startIndex)
                    {
                        int messageDataLength = totalLength + startIndex - offsetIndex;
                        byte[] messageDatas = new byte[messageDataLength];
                        if (stream.Read(messageDatas, 0, messageDataLength) != messageDataLength)
                        {
                            MessageError?.Invoke(MessageErrorCode.Reader_CompareMessageDataLengthError);
                            break;
                        }
                        MessageReceived?.Invoke(messageID, messageDatas);
                    }
                    else
                    {
                        MessageReceived?.Invoke(messageID, null);
                    }

                    startIndex += totalLength;
                    serialNumber = serialNum;
                }

                bufferStream.MoveStream(startIndex);
            }
        }

        public void Reset()
        {
            bufferStream.Reset();
            serialNumber = 0;
        }
    }
}
