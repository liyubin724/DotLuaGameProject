using System.IO;

namespace DotEngine.Net
{
    public class DataBuffer
    {
        private DataStream[] streams = null;
        private int activedIndex = 0;

        public long Length
        {
            get
            {
                return streams[activedIndex].Length;
            }
        }

        public DataBuffer()
        {
            streams = new DataStream[] { new DataStream(), new DataStream() };
        }

        public DataStream GetStream()
        {
            return streams[activedIndex];
        }

        public void WriteBytes(byte[] bytes, int startIndex, int size)
        {
            GetStream().Write(bytes, startIndex, size);
        }

        public byte[] ReadMessage(int startOffset = 0)
        {
            DataStream activedStream = GetStream();
            long streamLen = activedStream.Length;
            if (streamLen <= startOffset + sizeof(int))
            {
                return null;
            }
            if (activedStream.ReadInt(startOffset, out int msgByteLen))
            {
                if (msgByteLen + startOffset <= streamLen)
                {
                    activedStream.ReadBytes(startOffset + sizeof(int), msgByteLen, out var msgBytes);
                    MoveStream(startOffset + sizeof(int) + msgByteLen);

                    return msgBytes;
                }
            }
            return null;
        }

        public void MoveStream(int startIndex)
        {
            DataStream activedStream = GetStream();
            if (startIndex > 0)
            {
                if (startIndex < activedStream.Length)
                {
                    activedIndex = (activedIndex + 1) % streams.Length;
                    DataStream targetStream = GetStream();
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
            activedIndex = 0;
        }
    }
}
