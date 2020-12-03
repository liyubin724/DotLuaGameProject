namespace DotEngine.Net.Stream
{
    public class BufferStream
    {
        private MemoryStreamEx[] streamArr = null;
        private int activedStreamIndex = 0;

        public BufferStream()
        {
            streamArr = new MemoryStreamEx[]
            {
                new MemoryStreamEx(),
                new MemoryStreamEx(),
            };
            activedStreamIndex = 0;
        }

        public MemoryStreamEx GetActivedStream()
        {
            return streamArr[activedStreamIndex];
        }

        public void MoveStream(int startIndex)
        {
            MemoryStreamEx activedStream = GetActivedStream();
            if(startIndex>0)
            {
                if(startIndex<activedStream.Length)
                {
                    activedStreamIndex = (activedStreamIndex + 1) % streamArr.Length;

                    MemoryStreamEx newActivedStream = GetActivedStream();
                    newActivedStream.Clear();
                    activedStream.Write(activedStream.GetBuffer(), startIndex, ((int)activedStream.Length - startIndex));
                    activedStream.Clear();
                }else
                {
                    activedStream.Clear();
                }
            }
        }
        
        public void Reset()
        {
            foreach(var stream in streamArr)
            {
                stream.Clear();
            }
            activedStreamIndex = 0;
        }
    }
}
