using System.Collections.Generic;

namespace DotEngine.NetCore.TCPNetwork
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

        public long Length
        {
            get
            {
                return streams[activedStreamIndex].Length;
            }
        }

        public void WriteBytes(byte[] bytes,int startIndex, int size)
        {
            GetStream().Write(bytes, startIndex, size);
        }

        public byte[] ReadMessage(int startOffset = 0)
        {
            NetMessageStream activedStream = GetStream();
            long streamLen = activedStream.Length;
            if(streamLen<=startOffset+sizeof(int))
            {
                return null;
            }
            if(activedStream.ReadInt(startOffset,out int msgByteLen))
            {
                if(msgByteLen + startOffset <= streamLen)
                {
                    byte[] msgBytes = new byte[msgByteLen];
                    activedStream.Read(msgBytes, startOffset + sizeof(int), msgByteLen);
                    MoveStream(startOffset + sizeof(int) + msgByteLen);

                    return msgBytes;
                }
            }
            return null;
        }
        
        private List<byte[]> tempBytesList = new List<byte[]>();
        public byte[][] ReadMessages()
        {
            NetMessageStream activedStream = GetStream();
            long streamLen = activedStream.Length;
            if (streamLen <= sizeof(int))
            {
                return null;
            }
            int offset = 0;
            while(true)
            {
                if (activedStream.ReadInt(offset, out int msgByteLen))
                {
                    offset += sizeof(int);
                    if (msgByteLen + offset <= streamLen)
                    {
                        activedStream.ReadBytes(offset, msgByteLen, out var msgBytes);

                        offset += msgByteLen;

                        tempBytesList.Add(msgBytes);
                    }else
                    {
                        break;
                    }
                }else
                {
                    break;
                }
            }

            byte[][] result = null;
            if(tempBytesList.Count>0)
            {
                result = tempBytesList.ToArray();
                tempBytesList.Clear();
                MoveStream(offset);
            }
            return result;
        }

        public void MoveStream(int startIndex)
        {
            NetMessageStream activedStream = GetStream();
            if (startIndex > 0)
            {
                if (startIndex < activedStream.Length)
                {
                    activedStreamIndex = (activedStreamIndex + 1) % streams.Length;
                    NetMessageStream targetStream = GetStream();
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
