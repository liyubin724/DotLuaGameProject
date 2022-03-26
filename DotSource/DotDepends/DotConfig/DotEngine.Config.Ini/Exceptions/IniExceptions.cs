using System;

namespace DotEngine.Config.Ini
{
    public class IniPropertyRepeatException : Exception
    {
        public IniPropertyRepeatException(string propertyKey)
            : base($"The key of property({propertyKey}) has been added")
        { }
    }

    public class IniReaderException : Exception
    {
        public int LineNumber { get; }
        public string LineContent { get; }

        public IniReaderException(string msg, int lineNumber, string lineContent) : base($"{msg}\nnumber = {lineNumber},content = {lineContent}")
        {
            LineNumber = lineNumber;
            LineContent = lineContent;
        }
    }
}
