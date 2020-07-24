using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotEngine.FS
{
    public class FragmentData
    {
        public const int SIZE = sizeof(long) + sizeof(int);

        public long StartPosition { get; set; }
        public int UsageSize { get; set; }
    }

    public class FileFragment
    {
        private List<FragmentData> sortedStartFragments;
        private List<FragmentData> sortedSizeFragments;
        public FileFragment()
        {
            sortedStartFragments = new List<FragmentData>();
            sortedSizeFragments = new List<FragmentData>();
        }

        public unsafe FileSystemResultCode ReadFromStream(Stream stream)
        {
            byte[] bytes = new byte[FragmentData.SIZE];
            if (stream.Read(bytes, 0, sizeof(int)) != sizeof(int))
            {
                return FileSystemResultCode.FragmentByteLengthError;
            }

            int len = 0;
            fixed (byte* b = &bytes[0])
            {
                len = *((int*)b);
            }

            for (int i = 0; i < len; ++i)
            {
                if (stream.Read(bytes, 0, bytes.Length) != bytes.Length)
                {
                    return FileSystemResultCode.FragmentDataByteLengthError;
                }

                fixed(byte *c = &bytes[0])
                {
                    long start = *((long*)c);
                    int size = *((int*)(c + sizeof(long)));

                    FragmentData data = new FragmentData() { StartPosition = start, UsageSize = size };
                    sortedStartFragments.Add(data);
                }
            }

            if (sortedStartFragments.Count != len)
            {
                return FileSystemResultCode.FragmentDataCountError;
            }
            return FileSystemResultCode.Success;
        }

        public void WriteToStream(Stream stream)
        {
            int len = sortedStartFragments.Count;
            byte[] lenBytes = BitConverter.GetBytes(len);
            stream.Write(lenBytes, 0, lenBytes.Length);

            foreach (var fragment in sortedStartFragments)
            {
                byte[] startBytes = BitConverter.GetBytes(fragment.StartPosition);
                byte[] sizeBytes = BitConverter.GetBytes(fragment.UsageSize);
                stream.Write(startBytes, 0, startBytes.Length);
                stream.Write(sizeBytes, 0, sizeBytes.Length);
            }
        }

        public void Add(long start,int size)
        {
            if(sortedStartFragments.Count == 0)
            {
                FragmentData fragmentData = new FragmentData()
                {
                    StartPosition = start,
                    UsageSize = size,
                };

                sortedStartFragments.Add(fragmentData);
                sortedSizeFragments.Add(fragmentData);
            }else
            {
                int startInsertIndex = FindStartInsertIndex(0, sortedStartFragments.Count - 1, start);

                FragmentData preFragment = startInsertIndex == 0 ? null : sortedStartFragments[startInsertIndex - 1];
                FragmentData nextFragment = startInsertIndex == sortedStartFragments.Count ? null : sortedStartFragments[startInsertIndex];

                bool isConcatPre = false;
                if(preFragment!=null && preFragment.StartPosition+preFragment.UsageSize == start)
                {
                    isConcatPre = true;
                }
                bool isConcatNext = false;
                if(nextFragment!=null && start+size == nextFragment.StartPosition)
                {
                    isConcatNext = true;
                }

                FragmentData fragment = null;
                if (!isConcatPre && !isConcatNext)
                {
                    fragment = new FragmentData()
                    {
                        StartPosition = startInsertIndex,
                        UsageSize = size,
                    };
                    sortedStartFragments.Insert(startInsertIndex, fragment);
                }else if(isConcatNext && isConcatPre)
                {
                    preFragment.UsageSize += size + nextFragment.UsageSize;
                    sortedStartFragments.Remove(nextFragment);

                    sortedSizeFragments.Remove(preFragment);
                    sortedSizeFragments.Remove(nextFragment);

                    fragment = preFragment;
                }else if(isConcatNext)
                {
                    nextFragment.StartPosition -= size;
                    sortedSizeFragments.Remove(nextFragment);

                    fragment = nextFragment;
                }
                else if(isConcatPre)
                {
                    preFragment.UsageSize += size;
                    sortedSizeFragments.Remove(preFragment);

                    fragment = preFragment;
                }

                if(sortedSizeFragments.Count == 0)
                {
                    sortedSizeFragments.Add(fragment);
                }else
                {
                    int sizeInsertIndex = FindSizeInsertIndex(0, sortedSizeFragments.Count - 1, fragment.UsageSize);
                    sortedSizeFragments.Insert(sizeInsertIndex, fragment);
                }
            }
        }

        public FragmentData Get(int size)
        {
            if(sortedSizeFragments.Count == 0)
            {
                return null;
            }
            int sizeInsertIndex = FindSizeInsertIndex(0, sortedSizeFragments.Count - 1, size);
            return sortedSizeFragments[sizeInsertIndex];
        }

        public void Update(FragmentData fragmentData)
        {
            if(fragmentData.UsageSize > 0)
            {
                sortedSizeFragments.Remove(fragmentData);

                int sizeInsertIndex = FindSizeInsertIndex(0, sortedSizeFragments.Count - 1, fragmentData.UsageSize);
                sortedSizeFragments.Insert(sizeInsertIndex, fragmentData);
            }
            else
            {
                sortedStartFragments.Remove(fragmentData);
                sortedSizeFragments.Remove(fragmentData);
            }
        }

        private int FindSizeInsertIndex(int left,int right,int size)
        {
            int leftSize = sortedStartFragments[left].UsageSize;
            if (leftSize > size)
            {
                return left;
            }
            int rightSize = sortedStartFragments[right].UsageSize;
            if (rightSize < size)
            {
                return right + 1;
            }

            int mid = (left + right) / 2;
            int midSize = sortedStartFragments[mid].UsageSize;
            if (mid == left)
            {
                if (midSize > size)
                {
                    return left;
                }
                else if (midSize < size)
                {
                    return right;
                }
                else
                {
                    return mid;
                }
            }
            if (midSize > size)
            {
                return FindStartInsertIndex(left, mid, size);
            }
            else if (midSize < size)
            {
                return FindStartInsertIndex(mid, right, size);
            }
            else
            {
                return mid;
            }
        }

        private int FindStartInsertIndex(int left,int right,long start)
        {
            long leftStart = sortedStartFragments[left].StartPosition;
            if(leftStart>start)
            {
                return left;
            }
            long rightStart = sortedStartFragments[right].StartPosition;
            if(rightStart<start)
            {
                return right + 1;
            }

            int mid = (left + right) / 2;
            long midStart = sortedStartFragments[mid].StartPosition;
            if(mid == left)
            {
                if(midStart>start)
                {
                    return left;
                }else if(midStart<start)
                {
                    return right;
                }else
                {
                    return mid;
                }
            }
            if(midStart>start)
            {
                return FindStartInsertIndex(left, mid, start);
            }else if(midStart <start)
            {
                return FindStartInsertIndex(mid, right, start);
            }
            else
            {
                return mid;
            }
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            foreach(var fragment in sortedStartFragments)
            {
                text.AppendLine($"{fragment.StartPosition}    {fragment.UsageSize}");
            }
            return text.ToString();
        }
    }
}
