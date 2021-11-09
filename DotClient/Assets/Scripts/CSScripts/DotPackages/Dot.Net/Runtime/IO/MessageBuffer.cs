using System.IO;

namespace DotEngine.Net
{
    public class MessageBuffer
    {
        private int activedStreamIndex = 0;
        private MessageStream[] streams = new MessageStream[]
        {
            new MessageStream(),
            new MessageStream(),
        };

        public MessageStream GetStream()
        {
            return streams[activedStreamIndex];
        }

        public void WriteBytes(byte[] bytes)
        {
            GetStream().Write(bytes, 0, bytes.Length);
        }

        public void WriteBytes(byte[] bytes, int start, int size)
        {
            GetStream().Write(bytes, start, size);
        }

        public byte[] ReadMessage()
        {
            MessageStream activedStream = GetStream();
            long streamLength = activedStream.Length;
            if (streamLength < sizeof(int))
            {
                return null;
            }
            activedStream.Seek(0, SeekOrigin.Begin);
            if (activedStream.ReadInt(0, out int messageLength) && messageLength + sizeof(int) <= streamLength)
            {
                byte[] bytes = new byte[messageLength];
                activedStream.Read(bytes, 0, messageLength);

                MoveStream(sizeof(int) + messageLength);

                return bytes;
            }
            return null;
        }

        public void MoveStream(int startIndex)
        {
            MessageStream activedStream = GetStream();
            if (startIndex > 0)
            {
                if (startIndex < activedStream.Length)
                {
                    activedStreamIndex = (activedStreamIndex + 1) % streams.Length;
                    MessageStream targetStream = GetStream();
                    targetStream.Write(activedStream.GetBuffer(), startIndex, (int)(activedStream.Length - startIndex));
                    activedStream.Clear();
                }
                else
                {
                    activedStream.Clear();
                }
            }
        }

        public void ResetStream()
        {
            foreach (var stream in streams)
            {
                stream.Clear();
            }
            activedStreamIndex = 0;
        }
    }
}
