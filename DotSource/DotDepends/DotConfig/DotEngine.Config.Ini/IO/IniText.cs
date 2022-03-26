using System.Collections.Generic;

namespace DotEngine.Config.Ini
{
    class IniLine : IDeepCopy<IniLine>
    {
        private int start = 0;
        private int end = 0;
        private int size = 0;
        public int Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
                if (start > end || start < 0)
                {
                    size = 0;
                }
                else
                {
                    size = end - start + 1;
                }
            }
        }
        public int End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
                if (end < start || end < 0)
                {
                    size = 0;
                }
                else
                {
                    size = end - start + 1;
                }
            }
        }
        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                if (size < 0)
                {
                    end = start;
                }
                else
                {
                    end = start + size - 1;
                }
            }
        }

        private int contentStart = 0;
        private int contentEnd = 0;
        private int contentSize = 0;
        public int ContentStart
        {
            get
            {
                return contentStart;
            }
            set
            {
                contentStart = value;
                if (contentStart > contentEnd || contentStart < 0)
                {
                    contentSize = 0;
                }
                else
                {
                    contentSize = contentEnd - contentStart + 1;
                }
            }
        }
        public int ContentEnd
        {
            get
            {
                return contentEnd;
            }
            set
            {
                contentEnd = value;
                if (contentEnd < contentStart || contentStart < 0)
                {
                    contentSize = 0;
                }
                else
                {
                    contentSize = contentEnd - contentStart + 1;
                }
            }
        }
        public int ContentSize
        {
            get
            {
                return contentSize;
            }
            set
            {
                contentSize = value;
                if (contentSize < 0)
                {
                    contentEnd = contentStart;
                }
                else
                {
                    contentEnd = contentStart + contentSize - 1;
                }
            }
        }

        public IniLine DeepCopy()
        {
            return new IniLine()
            {
                start = start,
                end = end,
                size = size,

                contentStart = contentStart,
                contentEnd = contentEnd,
                contentSize = contentSize,
            };
        }

        public void Reset()
        {
            start = 0;
            end = 0;
            size = 0;
            contentStart = 0;
            contentEnd = 0;
            contentSize = 0;
        }
    }


    class IniText
    {
        public string Text { get; set; }
        public IniText()
        {
        }

        public bool ReadLine(int start, ref IniLine line)
        {
            if (string.IsNullOrEmpty(Text) || start >= Text.Length)
            {
                return false;
            }

            int end = 0;
            while (end < Text.Length)
            {
                if (Text[end] == '\n')
                {
                    break;
                }
                end++;
            }
            line.Start = start;
            line.End = end;
            line.ContentStart = start;
            line.ContentEnd = end;
            if (line.ContentSize > 0)
            {
                if (Text[line.ContentEnd] == '\n')
                {
                    line.ContentSize--;
                    if (line.ContentSize > 0)
                    {
                        if (Text[line.ContentEnd] == '\r')
                        {
                            line.ContentSize--;
                        }
                    }
                }
            }

            return true;
        }

        public bool IsContentEmpty(IniLine line)
        {
            return line.ContentSize == 0;
        }

        public bool IsContentWhitespace(IniLine line)
        {
            if (line.ContentSize == 0)
            {
                return true;
            }
            int index = line.ContentStart;
            while (index <= line.ContentEnd && !char.IsWhiteSpace(Text[index]))
            {
                return false;
            }
            return true;
        }

        public void TrimContentStart(IniLine line)
        {
            if (line.ContentSize == 0)
            {
                return;
            }

            int index = line.ContentStart;
            while (index <= line.ContentEnd && char.IsWhiteSpace(Text[index]))
            {
                line.ContentStart++;
                line.ContentSize--;

                index++;
            }
        }

        public void TrimContentEnd(IniLine line)
        {
            if (line.ContentSize == 0)
            {
                return;
            }
            int index = line.ContentEnd;
            while (index >= line.ContentStart && char.IsWhiteSpace(Text[index]))
            {
                line.ContentSize--;

                index--;
            }
        }

        public void TrimContent(IniLine line)
        {
            TrimContentStart(line);
            TrimContentEnd(line);
        }

        public bool IsContentStartWith(IniLine line, string prefix)
        {
            if (line.ContentSize == 0 || string.IsNullOrEmpty(prefix))
            {
                return false;
            }
            if (line.ContentSize < prefix.Length)
            {
                return false;
            }

            for (int lineStartIndex = line.ContentStart, prefixIndex = 0;
                prefixIndex < prefix.Length;
                lineStartIndex++, prefixIndex++)
            {
                if (prefix[prefixIndex] != Text[lineStartIndex]) return false;
            }
            return true;
        }

        public bool IsContentEndWith(IniLine line, string postfix)
        {
            if (line.ContentSize == 0 || string.IsNullOrEmpty(postfix))
            {
                return false;
            }
            if (line.ContentSize < postfix.Length)
            {
                return false;
            }

            for (int lineEndIndex = line.ContentEnd, postfixIndex = postfix.Length - 1;
                postfixIndex >= 0;
                lineEndIndex--, postfixIndex--)
            {
                if (postfix[postfixIndex] != Text[lineEndIndex]) return false;
            }
            return true;
        }

        public string GetContent(IniLine line)
        {
            if (line.ContentSize == 0)
            {
                return string.Empty;
            }

            return Text.Substring(line.ContentStart, line.ContentSize);
        }

        public string GetContent(int start, int size)
        {
            if (size == 0 || start < 0 || start + size > Text.Length)
            {
                return string.Empty;
            }
            return Text.Substring(start, size);
        }

        public IniLine[] SplitContent(IniLine line, string splitStr)
        {
            if (line.ContentSize == 0)
            {
                return new IniLine[0];
            }

            List<IniLine> results = new List<IniLine>();

            IniLine preLine = line.DeepCopy();
            int splitIndex = FindContent(line.ContentStart, line.ContentEnd, splitStr);
            while (splitIndex >= 0)
            {
                preLine = new IniLine()
                {
                    Start = line.Start,
                    Size = line.Size,
                    ContentStart = preLine.ContentStart,
                    ContentSize = splitIndex - preLine.ContentStart,
                };
                results.Add(preLine);

                splitIndex = FindContent(preLine.ContentStart + splitStr.Length, line.ContentEnd, splitStr);
            }
            return results.ToArray();
        }

        public int FindContent(int start, int end, string splitStr)
        {
            if (start < 0 || end < start || string.IsNullOrEmpty(splitStr))
            {
                return -1;
            }
            int size = end - start + 1;
            if (size < splitStr.Length)
            {
                return -1;
            }

            int lineStartIndex = start;
            int splitIndex = 0;
            int resultIndex = -1;
            while (lineStartIndex <= end)
            {
                if (Text[lineStartIndex] == splitStr[splitIndex])
                {
                    if (splitIndex == 0)
                    {
                        resultIndex = lineStartIndex;
                    }

                    splitIndex++;
                    if (splitIndex > splitStr.Length)
                    {
                        break;
                    }
                }
                else
                {
                    splitIndex = 0;
                    resultIndex = -1;
                }
                lineStartIndex++;
            }

            return resultIndex;
        }
    }
}
