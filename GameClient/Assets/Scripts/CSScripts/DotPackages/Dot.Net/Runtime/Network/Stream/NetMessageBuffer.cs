namespace DotEngine.Net
{
    public class NetMessageBuffer
    {
        private NetMessageStream[] streams = new NetMessageStream[]
        {
            new NetMessageStream(),
            new NetMessageStream(),
        };

        private int activedStreamIndex = 0;

        public NetMessageStream GetStream()
        {
            return streams[activedStreamIndex];
        }

        public void WriteBytes(byte[] bytes,int size)
        {
            GetStream().Write(bytes, 0, size);
        }

        public byte[] ReadMessage()
        {
            NetMessageStream activedStream = GetStream();
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
            NetMessageStream activedStream = GetStream();
            if(startIndex > 0)
            {
                if(startIndex < activedStream.Length)
                {
                    activedStreamIndex = (activedStreamIndex + 1) % streams.Length;
                    NetMessageStream targetStream = GetStream();
                    targetStream.Write(activedStream.GetBuffer(), startIndex, (int)(activedStream.Length - startIndex));
                    activedStream.Clear();
                }else
                {
                    activedStream.Clear();
                }
            }
        }

        public void ResetStream()
        {
            foreach(var stream in streams)
            {
                stream.Clear();
            }
            activedStreamIndex = 0;
        }
    }
}
