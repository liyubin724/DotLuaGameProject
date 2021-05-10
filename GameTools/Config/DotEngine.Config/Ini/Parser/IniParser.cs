using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Config.Ini
{
    struct IniLineRange
    {
        public int Start { get; set; }
        public int Size { get; set; }

        public int End => Start + Size - 1;

        public bool IsEmpty
        {
            get
            {
                return Size == 0;
            }
        }
    }

    class IniStringBuffer
    {
        private TextReader textReader = null;

        private int currentLineNumber = -1;
        public int LineNumber => currentLineNumber;

        private string lineString = null;

        private IniStringBuffer()
        {
        }

        public void Start(string iniString)
        {
            textReader = new StringReader(iniString);
        }

        public void End()
        {
            textReader.Close();
            textReader = null;
            currentLineNumber = -1;
            lineString = null;
        }

        public bool ReadLine(out IniLineRange lineRange)
        {
            lineRange = new IniLineRange();

            currentLineNumber++;
            lineString = textReader.ReadLine();
            if (lineString == null)
            {
                return false;
            }

            lineRange.Start = 0;
            lineRange.Size = lineString.Length;

            return true;
        }

        public bool IsEmpty(IniLineRange lineRange)
        {
            return lineRange.IsEmpty;
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

        public IniLineRange TrimStart(IniLineRange lineRange)
        {
            if (!string.IsNullOrEmpty(lineString))
            {
                int index = lineRange.Start;
                while (index <= lineRange.End && char.IsWhiteSpace(lineString[index]))
                {
                    ++index;
                }

                int endIndex = lineRange.End;
                lineRange.Start = index;
                lineRange.Size = endIndex - index + 1;
            }
            return lineRange;
        }

        public IniLineRange TrimEnd(IniLineRange lineRange)
        {
            if (!string.IsNullOrEmpty(lineString))
            {
                int index = lineRange.End;
                while (index >= lineRange.Start && char.IsWhiteSpace(lineString[index]))
                {
                    --index;
                }
                lineRange.Size = index - lineRange.Start + 1;
            }

            return lineRange;
        }

        public IniLineRange Trim(IniLineRange lineRange)
        {
            lineRange = TrimStart(lineRange);
            lineRange = TrimEnd(lineRange);
            return lineRange;
        }

        public bool IsStartWith(IniLineRange lineRange, string str)
        {
            if (string.IsNullOrEmpty(str)) return false;

            int strIndex = 0;
            int lineIndex = lineRange.Start;
            for (; strIndex < str.Length; ++strIndex, ++lineIndex)
            {
                if (str[strIndex] != lineString[lineIndex]) return false;
            }

            return true;
        }

        public bool IsEndWith(IniLineRange lineRange,string str)
        {
            if (string.IsNullOrEmpty(str)) return false;

            return false;
        }

        public string Substring(IniLineRange lineRange)
        {
            if (lineRange.IsEmpty
                || string.IsNullOrEmpty(lineString)
                || lineRange.Start < 0
                || lineRange.Start+lineRange.Size -1 > lineString.Length) 
                return string.Empty;

            return lineString.Substring(lineRange.Start, lineRange.Size);
        }

        public bool Search(IniLineRange lineRange,string str,out IniLineRange positonLineRange)
        {
            positonLineRange = new IniLineRange();
            if (string.IsNullOrEmpty(str)) return false;

            int strStartIndex = -1;

            int strIndex = 0;
            int lineIndex = lineRange.Start;
            for (; lineIndex <= lineRange.End && strIndex < str.Length; ++lineIndex)
            { 
                if(str[strIndex] == lineString[lineIndex])
                {
                    if(strStartIndex<0)
                    {
                        strStartIndex = lineIndex;
                    }
                    ++strIndex;
                }else
                {
                    strStartIndex = -1;
                    strIndex = 0;
                }
            }

            positonLineRange.Start = strStartIndex;
            positonLineRange.Size = str.Length;

            return strStartIndex >=0 && strIndex == str.Length;
        }

        
    }

    public class IniParser
    {
        private static IniLine line = new IniLine();
        private static List<Exception> exceptions = new List<Exception>();

        public static void Parse(string iniString, IniSchemeStyle schemeStyle = null, IniParserStyle parserStyle = null)
        {

        }

        private static void StartParser()
        {

        }

        private static void EndParse()
        {
            line.Reset();
        }

        private static void ProcessLine(IniLine line, IniData iniData)
        {

        }
    }
}
