using System.IO;

namespace DotEngine.Ini
{
    class IniLineRange : IDeepCopy<IniLineRange>
    {
        public int Start { get; set; }
        public int Size { get; set; }

        public int End
        {
            get => Start + Size - 1;
        }

        public IniLineRange DeepCopy()
        {
            IniLineRange result = new IniLineRange();
            result.Start = Start;
            result.Size = Size;
            return result;
        }

        public void Reset()
        {
            Start = 0;
            Size = 0;
        }
    }

    class IniTextBuffer
    {
        private TextReader textReader = null;

        private int currentLineNumber = 0;
        public int LineNumber => currentLineNumber;

        private string lineContent = null;
        public string LineContent => lineContent;

        private IniLineRange lineRange = new IniLineRange();
        public IniLineRange Range => lineRange;

        public IniTextBuffer(string iniString)
        {
            textReader = new StringReader(iniString);
        }

        public bool ReadLine()
        {
            lineContent = textReader.ReadLine();
            if (lineContent == null)
            {
                return false;
            }

            currentLineNumber++;
            lineRange.Start = 0;
            lineRange.Size = lineContent.Length;

            return true;
        }

        public bool IsEmpty(IniLineRange range)
        {
            return range.Size == 0;
        }

        public bool IsWhitespace(IniLineRange range)
        {
            int index = range.Start;
            while (index <= range.End && char.IsWhiteSpace(lineContent[index]))
            {
                ++index;
            }
            return index > range.End;
        }

        public void TrimStart(IniLineRange range)
        {
            if (string.IsNullOrEmpty(lineContent))
            {
                return;
            }
            int index = range.Start;
            while (index <= range.End && char.IsWhiteSpace(lineContent[index]))
            {
                ++index;
            }

            int endIndex = range.End;
            range.Start = index;
            range.Size = endIndex - index + 1;
        }

        public void TrimEnd(IniLineRange range)
        {
            if (string.IsNullOrEmpty(lineContent))
            {
                return;
            }
            int index = range.End;
            while (index >= range.Start && char.IsWhiteSpace(lineContent[index]))
            {
                --index;
            }
            range.Size = index - range.Start + 1;
        }

        public void Trim(IniLineRange range)
        {
            TrimStart(range);
            TrimEnd(range);
        }

        public bool IsStartWith(IniLineRange range, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            int strIndex = 0;
            int lineIndex = range.Start;
            for (; strIndex < str.Length; ++strIndex, ++lineIndex)
            {
                if (str[strIndex] != lineContent[lineIndex]) return false;
            }

            return true;
        }

        public bool IsEndWith(IniLineRange range, string str)
        {
            if (string.IsNullOrEmpty(str)) return false;

            int strIndex = str.Length - 1;
            int lineIndex = range.End;
            for (; strIndex >= 0; --strIndex, --lineIndex)
            {
                if (str[strIndex] != lineContent[lineIndex]) return false;
            }

            return true;
        }

        public string GetString(IniLineRange range)
        {
            if (string.IsNullOrEmpty(lineContent)
                || range.Size == 0
                || range.Start < 0
                || range.Start + range.Size - 1 > lineContent.Length)
            {
                return string.Empty;
            }

            return lineContent.Substring(range.Start, range.Size);
        }

        public int FindString(IniLineRange range, string str)
        {
            if (string.IsNullOrEmpty(str)) return -1;

            int strStartIndex = -1;

            int strIndex = 0;
            int lineIndex = range.Start;
            for (; lineIndex <= range.End && strIndex < str.Length; ++lineIndex)
            {
                if (str[strIndex] == lineContent[lineIndex])
                {
                    if (strStartIndex < 0)
                    {
                        strStartIndex = lineIndex;
                    }
                    ++strIndex;
                }
                else
                {
                    strStartIndex = -1;
                    strIndex = 0;
                }
            }

            if (strStartIndex >= 0 && strIndex == str.Length)
            {
                return strStartIndex;
            }

            return -1;
        }
    }
}
