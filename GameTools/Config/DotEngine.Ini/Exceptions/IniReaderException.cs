using System;

namespace DotEngine.Ini
{
    public class IniReaderException : Exception
    {
        public int LineNumber { get; } 
        public string LineContent { get;}

        public IniReaderException(string msg,int lineNumber,string lineContent) : base($"{msg}\nnumber = {lineNumber},content = {lineContent}")
        {
            LineNumber = lineNumber;
            LineContent = lineContent;
        }
    }
}
