using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotEngine.FS
{
    public class ChunkData
    {
        public const int MAX_SIZE = 512;

        public string Path { get; set; }
        public long StartPosition { get; set; }
        public int ContentLength { get; set; }
        public int UsageSize { get; set; }
    }

    public class FileChunk
    {
        private Dictionary<string, ChunkData> chunkDic = new Dictionary<string, ChunkData>();
        public FileChunk()
        {
        }

        public unsafe FileSystemResultCode ReadFromStream(Stream stream)
        {
            byte[] bytes = new byte[ChunkData.MAX_SIZE];
            if (stream.Read(bytes, 0, sizeof(int)) != sizeof(int))
            {
                return FileSystemResultCode.ChunkByteLengthError;
            }

            int len = 0;
            fixed(byte *b = &bytes[0])
            {
                len = *((int*)b);
            }

            for (int i = 0; i < len; ++i)
            {
                if (stream.Read(bytes, 0, sizeof(int)) != sizeof(int))
                {
                    return FileSystemResultCode.ChunkPathSizeError;
                }

                int pathLen = 0;
                fixed(byte *l = &bytes[0])
                {
                    pathLen = *((int*)l);
                }

                int chunkLen = pathLen + sizeof(int) * 2+sizeof(long);
                if(chunkLen> ChunkData.MAX_SIZE)
                {
                    return FileSystemResultCode.ChunkByteTooLongError;
                }

                if (stream.Read(bytes, 0, chunkLen) != chunkLen)
                {
                    return FileSystemResultCode.ChunkDataByteLengthError;
                }

                fixed(byte *c = &bytes[0])
                {
                    int offset = 0;
                    string path = Encoding.UTF8.GetString(c+offset, pathLen);
                    offset += pathLen;
                    long start = *((long*)(c + offset));
                    offset += sizeof(long);
                    int length = *((int*)(c + offset));
                    offset += sizeof(int);
                    int size = *((int*)(c + offset));

                    ChunkData data = new ChunkData()
                    {
                        Path = path,
                        StartPosition = start,
                        ContentLength = length,
                        UsageSize = size,
                    };
                    chunkDic.Add(path, data);
                }
            }

            if (chunkDic.Count != len)
            {
                return FileSystemResultCode.ChunkDataCountError;
            }

            return FileSystemResultCode.Success;
        }

        public void WriteToStream(Stream stream)
        {
            int len = chunkDic.Count;
            byte[] lenBytes = BitConverter.GetBytes(len);
            stream.Write(lenBytes, 0, lenBytes.Length);

            foreach (var kvp in chunkDic)
            {
                ChunkData chunk = kvp.Value;
                byte[] pathBytes = Encoding.UTF8.GetBytes(chunk.Path);
                byte[] pathLenBytes = BitConverter.GetBytes(pathBytes.Length);
                byte[] startBytes = BitConverter.GetBytes(chunk.StartPosition);
                byte[] lengthBytes = BitConverter.GetBytes(chunk.ContentLength);
                byte[] sizeBytes = BitConverter.GetBytes(chunk.UsageSize);
                stream.Write(pathLenBytes, 0, pathLenBytes.Length);
                stream.Write(pathBytes, 0, pathBytes.Length);
                stream.Write(startBytes, 0, startBytes.Length);
                stream.Write(lengthBytes, 0, lengthBytes.Length);
                stream.Write(sizeBytes, 0, sizeBytes.Length);
            }
        }

        public string[] GetFiles()
        {
            return chunkDic.Keys.ToArray();
        }

        public int Count()
        {
            return chunkDic.Count;
        }

        public bool Constains(string path)
        {
            return chunkDic.ContainsKey(path);
        }

        public ChunkData Get(string path)
        {
            if(chunkDic.TryGetValue(path,out ChunkData data))
            {
                return data;
            }
            return null;
        }

        public ChunkData Remove(string path)
        {
            if (chunkDic.TryGetValue(path, out ChunkData data))
            {
                chunkDic.Remove(path);
                return data;
            }
            return null;
        }

        public void Add(string filePath,long start,int length,int size)
        {
            ChunkData chunkData = new ChunkData()
            {
                Path = filePath,
                StartPosition = start,
                ContentLength = length,
                UsageSize = size,
            };
            chunkDic.Add(filePath, chunkData);
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            foreach(var kvp in chunkDic)
            {
                text.AppendLine($"{kvp.Value.Path}    {kvp.Value.StartPosition}    {kvp.Value.ContentLength}    {kvp.Value.UsageSize}");
            }

            return text.ToString();
        }
    }

}
