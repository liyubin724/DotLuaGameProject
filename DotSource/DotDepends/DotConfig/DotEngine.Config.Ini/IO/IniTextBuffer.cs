using System.Collections.Generic;

namespace DotEngine.Config.Ini
{
    class IniLineRange : IDeepCopy<IniLineRange>
    {
        private int start = -1;
        public int Start { get; set; } = -1;
        private int end = -1;
        public int End { get; set; } = -1;
        public int Size
        {
            get
            {
                if (Start < 0 || End < 0 || Start > End)
                {
                    return 0;
                }
                else
                {
                    return End - Start + 1;
                }
            }
        }

        public int ContentStart { get; set; } = -1;
        public int ContentEnd { get; set; } = -1;
        public int ContentSize
        {
            get
            {
                if (ContentStart < 0 || ContentEnd < 0 || ContentStart > ContentEnd)
                {
                    return 0;
                }
                else
                {
                    return ContentEnd - ContentStart + 1;
                }
            }
        }

        public IniLineRange DeepCopy()
        {
            return new IniLineRange()
            {
                Start = Start,
                End = End,

                ContentStart = ContentStart,
                ContentEnd = ContentEnd,
            };
        }

        public void Reset()
        {
            Start = -1;
            End = -1;
            ContentStart = -1;
            ContentEnd = -1;
        }
    }


    class IniTextBuffer
    {
        public string Text { get; set; }
        public IniTextBuffer()
        {
        }

        public bool ReadLine(int start, ref IniLineRange line)
        {
            if (string.IsNullOrEmpty(Text) || start <0 ||start >= Text.Length)
            {
                return false;
            }

            int end = start;
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
                    line.ContentEnd--;
                    if (line.ContentSize > 0)
                    {
                        if (Text[line.ContentEnd] == '\r')
                        {
                            line.ContentEnd--;
                        }
                    }
                }
            }

            return true;
        }

        public bool IsContentEmpty(IniLineRange line)
        {
            return line.ContentSize == 0;
        }

        public bool IsContentWhitespace(IniLineRange line)
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

        public void TrimContentStart(IniLineRange line)
        {
            if (line.ContentSize == 0)
            {
                return;
            }

            while (line.ContentStart <= line.ContentEnd && char.IsWhiteSpace(Text[line.ContentStart]))
            {
                line.ContentStart++;
            }
            if(line.ContentStart>line.ContentEnd)
            {
                line.ContentStart = line.ContentEnd = -1;
            }
        }

        public void TrimContentEnd(IniLineRange line)
        {
            if (line.ContentSize == 0)
            {
                return;
            }

            while (line.ContentEnd >= line.ContentStart && char.IsWhiteSpace(Text[line.ContentEnd]))
            {
                line.ContentEnd--;
            }
            if(line.ContentEnd<line.ContentStart)
            {
                line.ContentStart = line.ContentEnd = -1;
            }
        }

        public void TrimContent(IniLineRange line)
        {
            TrimContentStart(line);
            TrimContentEnd(line);
        }

        public bool IsContentStartWith(IniLineRange line, string prefix)
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

        public bool IsContentEndWith(IniLineRange line, string postfix)
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

        public string GetContent(IniLineRange line)
        {
            if (line.ContentSize == 0)
            {
                return string.Empty;
            }

            return Text.Substring(line.ContentStart, line.ContentSize);
        }

        public string GetContent(int start, int size)
        {
            if (size <= 0 || start < 0 || start + size > Text.Length)
            {
                return string.Empty;
            }
            return Text.Substring(start, size);
        }

        //public IniLineRange[] SplitContent(IniLineRange line, string splitStr)
        //{
        //    if (line.ContentSize == 0)
        //    {
        //        return new IniLineRange[0];
        //    }

        //    List<IniLineRange> results = new List<IniLineRange>();

        //    IniLineRange preLine = line.DeepCopy();
        //    int splitIndex = FindContent(line.ContentStart, line.ContentEnd, splitStr);
        //    while (splitIndex >= 0)
        //    {
        //        preLine = new IniLineRange()
        //        {
        //            Start = line.Start,
        //            Size = line.Size,
        //            ContentStart = preLine.ContentStart,
        //            ContentSize = splitIndex - preLine.ContentStart,
        //        };
        //        results.Add(preLine);

        //        splitIndex = FindContent(preLine.ContentStart + splitStr.Length, line.ContentEnd, splitStr);
        //    }
        //    return results.ToArray();
        //}

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
                    if (splitIndex > splitStr.Length-1)
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
