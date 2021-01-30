using System;
using System.IO;

namespace DotEngine.FS
{
    public class FileContent
    {
        private FileStream stream = null;

        public FileContent()
        { 
        }

        public FileSystemResultCode OpenContent(string filePath,FileSystemMode mode)
        {
            if(stream !=null)
            {
                return FileSystemResultCode.ContentHasBeenOpenedError;
            }

            FileMode fileMode;
            FileAccess fileAccess;
            FileShare fileShare;
            if (mode == FileSystemMode.Read)
            {
                fileMode = FileMode.Open;
                fileAccess = FileAccess.Read;
                fileShare = FileShare.Read;
            }
            else if (mode == FileSystemMode.Create)
            {
                fileMode = FileMode.Create;
                fileAccess = FileAccess.Write;
                fileShare = FileShare.None;
            }
            else if (mode == FileSystemMode.Update)
            {
                fileMode = FileMode.OpenOrCreate;
                fileAccess = FileAccess.ReadWrite;
                fileShare = FileShare.None;
            }
            else
            {
                return FileSystemResultCode.ModeNotExistError;
            }

            try
            {
                stream = new FileStream(filePath, fileMode, fileAccess, fileShare);
                return FileSystemResultCode.Success;
            }
            catch (Exception)
            {
                return FileSystemResultCode.ContentOpenError;
            }
        }

        public byte[] Read(long start,int length)
        {
            stream.Seek(start, SeekOrigin.Begin);
            byte[] result = new byte[length];
            if (stream.Read(result, 0, length) != length)
            {
                return null;
            }
            return result;
        }

        public void Write(long start,byte[] bytes)
        {
            stream.Seek(start, SeekOrigin.Begin);
            stream.Write(bytes, 0, bytes.Length);
        }

        public long Write(byte[] bytes)
        {
            stream.Seek(0, SeekOrigin.End);
            long start = stream.Length;
            stream.Write(bytes, 0, bytes.Length);
            return start;
        }

        public void Flush()
        {
            stream.Flush();
        }

        public void Close()
        {
            if(stream!=null)
            {
                stream.Flush();
                stream.Close();
            }
        }
    }
}
