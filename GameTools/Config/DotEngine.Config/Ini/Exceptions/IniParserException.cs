using System;

namespace DotEngine.Config.Ini
{
    public class IniParserException : Exception
    {
        public int LineNumber { get; } 
        public string LineContent { get;}

        public IniParserException(string msg,int lineNumber,string lineContent) : base($"{msg}\nnumber = {lineNumber},content = {lineContent}")
        {
            LineNumber = lineNumber;
            LineContent = lineContent;
        }
    }
}
