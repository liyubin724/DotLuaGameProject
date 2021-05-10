using System.IO;

namespace DotEngine.Config.Ini
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
    }

    class IniStringBuffer
    {
        private TextReader textReader = null;

        private int currentLineNumber = -1;
        public int LineNumber => currentLineNumber;

        private string lineString = null;

        public IniStringBuffer(string iniString)
        {
            textReader = new StringReader(iniString);
        }

        public bool ReadLine(ref IniLineRange lineRange)
        {
            lineRange = null;

            currentLineNumber++;
            lineString = textReader.ReadLine();
            if (lineString == null)
            {
                return false;
            }

            lineRange = new IniLineRange();
            lineRange.Start = 0;
            lineRange.Size = lineString.Length;

            return true;
        }

        public bool IsEmpty(IniLineRange lineRange)
        {
            return lineRange.Size == 0;
        }

        public bool IsWhitespace(IniLineRange lineRange)
        {
            int index = lineRange.Start;
            while (index <= lineRange.End && char.IsWhiteSpace(lineString[index]))
            {
                ++index;
            }
            return index > lineRange.End;
        }

        public void TrimStart(IniLineRange lineRange)
        {
            if (string.IsNullOrEmpty(lineString))
            {
                return;
            }
            int index = lineRange.Start;
            while (index <= lineRange.End && char.IsWhiteSpace(lineString[index]))
            {
                ++index;
            }

            int endIndex = lineRange.End;
            lineRange.Start = index;
            lineRange.Size = endIndex - index + 1;
        }

        public void TrimEnd(IniLineRange lineRange)
        {
            if (string.IsNullOrEmpty(lineString))
            {
                return;
            }
            int index = lineRange.End;
            while (index >= lineRange.Start && char.IsWhiteSpace(lineString[index]))
            {
                --index;
            }
            lineRange.Size = index - lineRange.Start + 1;
        }

        public void Trim(IniLineRange lineRange)
        {
            TrimStart(lineRange);
            TrimEnd(lineRange);
        }

        public bool IsStartWith(IniLineRange lineRange, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            int strIndex = 0;
            int lineIndex = lineRange.Start;
            for (; strIndex < str.Length; ++strIndex, ++lineIndex)
            {
                if (str[strIndex] != lineString[lineIndex]) return false;
            }

            return true;
        }

        public bool IsEndWith(IniLineRange lineRange, string str)
        {
            if (string.IsNullOrEmpty(str)) return false;

            int strIndex = str.Length - 1;
            int lineIndex = lineRange.End;
            for (; strIndex >= 0; --strIndex, --lineIndex)
            {
                if (str[strIndex] != lineString[lineIndex]) return false;
            }

            return true;
        }

        public string GetString(IniLineRange lineRange)
        {
            if (string.IsNullOrEmpty(lineString)
                || lineRange.Size == 0
                || lineRange.Start < 0
                || lineRange.Start + lineRange.Size - 1 > lineString.Length)
            {
                return string.Empty;
            }

            return lineString.Substring(lineRange.Start, lineRange.Size);
        }

        public int FindString(IniLineRange lineRange, string str)
        {
            if (string.IsNullOrEmpty(str)) return -1;

            int strStartIndex = -1;

            int strIndex = 0;
            int lineIndex = lineRange.Start;
            for (; lineIndex <= lineRange.End && strIndex < str.Length; ++lineIndex)
            {
                if (str[strIndex] == lineString[lineIndex])
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
