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

        public void WriteBytes(byte[] bytes,int start,int size)
        {
            GetStream().Write(bytes, start, size);
        }

        public byte[] ReadMessage()
        {
            MessageStream activedStream = GetStream();
            long messageLen = activedStream.Length;
            if (messageLen < sizeof(int))
            {
                return null;
            }
            if (activedStream.ReadInt(0, out int totalMessageLen) && totalMessageLen <= messageLen)
            {
                byte[] bytes = new byte[totalMessageLen];
                activedStream.Read(bytes, sizeof(int), totalMessageLen);

                MoveStream(sizeof(int) + totalMessageLen);

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
