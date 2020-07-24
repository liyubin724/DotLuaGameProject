using System.IO;
using System.Text;

namespace DotEngine.FS
{
    public enum FileSystemMode
    {
        Read,
        Create,
        Update,
    }

    public enum FileSystemResultCode
    {
        Success = 0,

        UnknownError = -1,
        ModeNotExistError = -2,
        CantModifyError = -3,

        ContentFileNotExistError = -100,
        ContentHasBeenOpenedError = -101,
        ContentOpenError = -102,

        ChunkByteLengthError = -200,
        ChunkPathSizeError = -201,
        ChunkByteTooLongError = -202,
        ChunkDataByteLengthError = -203,
        ChunkDataCountError = -204,

        FragmentByteLengthError = -300,
        FragmentDataByteLengthError = -301,
        FragmentDataCountError = -302,

        IndexFileNotExistError = -400,
        IndexStreamError = -401,
    }

    public class FileSystem
    {
        public string Name { get; private set; }
        public FileSystemMode Mode { get; private set; }

        public string ContentFilePath { get; private set; }
        public string IndexFilePath { get; private set; }

        private FileContent content = null;
        private FileChunk chunk = null;
        private FileFragment fragment = null;

        public FileSystem(string name)
        {
            Name = name;
        }

        public FileSystemResultCode Open(FileSystemMode mode, string contentPath, string indexPath)
        {
            Mode = mode;
            ContentFilePath = contentPath;
            IndexFilePath = indexPath;

            if (Mode != FileSystemMode.Create && !File.Exists(ContentFilePath))
            {
                return FileSystemResultCode.ContentFileNotExistError;
            }
            if (Mode != FileSystemMode.Create && !File.Exists(IndexFilePath))
            {
                return FileSystemResultCode.IndexFileNotExistError;
            }

            content = new FileContent();
            chunk = new FileChunk();
            if (Mode != FileSystemMode.Read)
            {
                fragment = new FileFragment();
            }

            FileSystemResultCode resultCode = FileSystemResultCode.Success;
            resultCode = content.OpenContent(ContentFilePath, Mode);
            if(resultCode == FileSystemResultCode.Success)
            {
                if (Mode != FileSystemMode.Create)
                {
                    FileStream indexStream = null;
                    try
                    {
                        indexStream = new FileStream(IndexFilePath, FileMode.Open, FileAccess.Read, FileShare.None);
                        resultCode = chunk.ReadFromStream(indexStream);
                        if (resultCode == FileSystemResultCode.Success && fragment != null)
                        {
                            resultCode = fragment.ReadFromStream(indexStream);
                        }
                    }
                    catch
                    {
                        resultCode = FileSystemResultCode.IndexFileNotExistError;
                    }
                    finally
                    {
                        if (indexStream != null)
                        {
                            indexStream.Close();
                        }
                    }
                }
            }

            if (resultCode != FileSystemResultCode.Success)
            {
                content.Close();
                content = null;
                fragment = null;
                chunk = null;
            }

            return resultCode;
        }

        public string[] GetAllFile()
        {
            return chunk.GetFiles();
        }

        public int GetFileCount()
        {
            return chunk.Count();
        }

        public bool HasFile(string filePath)
        {
            return chunk.Constains(filePath);
        }

        public byte[] GetFile(string filePath)
        {
            ChunkData chunkData = chunk.Get(filePath);
            if(chunkData == null)
            {
                return null;
            }

            return content.Read(chunkData.StartPosition, chunkData.ContentLength);
        }

        public bool GetFile(string filePath,out long start,out int length)
        {
            start = -1;
            length = 0;
            ChunkData chunkData = chunk.Get(filePath);
            if (chunkData != null)
            {
                start = chunkData.StartPosition;
                length = chunkData.ContentLength;
                return true;
            }
            return false;
        }

        public void AddFile(string filePath, byte[] fileBytes)
        {
            if(Mode == FileSystemMode.Read)
            {
                return;
            }

            int usageSize = GetFileUsageSize(fileBytes.Length);
            FragmentData fragmentData = fragment.Get(usageSize);
            if(fragmentData == null)
            {
                long start = content.Write(fileBytes);
                if(usageSize - fileBytes.Length>0)
                {
                    content.Write(new byte[usageSize - fileBytes.Length]);
                }

                chunk.Add(filePath, start, fileBytes.Length, usageSize);

            }else
            {
                content.Write(fragmentData.StartPosition, fileBytes);

                chunk.Add(filePath, fragmentData.StartPosition, fileBytes.Length, usageSize);

                fragmentData.StartPosition += usageSize;
                fragmentData.UsageSize -= usageSize;

                fragment.Update(fragmentData);
            }
        }

        public void DeleteFile(string filePath)
        {
            if (Mode == FileSystemMode.Read)
            {
                return;
            }

            ChunkData chunkData = chunk.Remove(filePath);
            if (chunkData != null)
            {
                fragment.Add(chunkData.StartPosition, chunkData.UsageSize);
            }
        }

        public FileSystemResultCode Save()
        {
            if (Mode == FileSystemMode.Read)
            {
                return FileSystemResultCode.CantModifyError;
            }
            content.Flush();

            FileSystemResultCode resultCode = FileSystemResultCode.Success;
            FileStream indexStream = null;
            try
            {
                indexStream = new FileStream(IndexFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                chunk.WriteToStream(indexStream);
                fragment.WriteToStream(indexStream);

                indexStream.Flush();
            }
            catch
            {
                resultCode = FileSystemResultCode.IndexFileNotExistError;
            }
            finally
            {
                if (indexStream != null)
                {
                    indexStream.Close();
                }
            }
            return resultCode;
        }

        public void Close()
        {
            if(Mode != FileSystemMode.Read)
            {
                Save();
            }

            content.Close();
            content = null;
            fragment = null;
            chunk = null;
        }

        private int GetFileUsageSize(int len)
        {
            int mod = len % 4;
            if(mod == 0)
            {
                return len;
            }else
            {
                return len + (4 - mod);
            }
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            if(chunk!=null)
            {
                text.Append(chunk.ToString());
            }
            if(fragment!=null)
            {
                text.AppendLine();
                text.AppendLine();

                text.Append(fragment.ToString());
            }
            return text.ToString();
        }
    }
}
