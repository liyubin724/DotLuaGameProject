using System;

namespace DotEngine.Config.Ini
{
    public class IniReaderLineFormatException : Exception
    {
        public IniReaderLineFormatException(string lineContent):base($"The format of the line is error.line = {lineContent}") { }
    }

    public class IniReaderLineSectionNullException : Exception
    {
        public IniReaderLineSectionNullException() : base("The section is null.") { }
    }

    public class IniReaderLinePropertyKeyEmptyException:Exception
    {
        public IniReaderLinePropertyKeyEmptyException():base("the key of the property is empty") { }
    }

    public class IniSectionRepeatException : Exception
    {
        public IniSectionRepeatException(string sectionName) : base($"the name of the section({sectionName}) has been added") { }
    }

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
