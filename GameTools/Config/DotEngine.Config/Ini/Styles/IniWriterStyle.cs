using System;

namespace DotEngine.Config.Ini
{
    public enum ENewLine
    {
        Windows,
        Unix_Mac
    }
    public class IniWriterStyle
    {
        public ENewLine NewLineType { get; set; }
        public string NewLineString
        {
            get
            {
                switch (NewLineType)
                {
                    case ENewLine.Unix_Mac: return "\n";
                    case ENewLine.Windows: return "\r\n";
                    default: return "\n";
                }
            }
        }

        public string SpacesBetweenKeyAndAssigment { get; private set; }
        public uint NumSpacesBetweenKeyAndAssigment
        {
            set
            {
                SpacesBetweenKeyAndAssigment = new string(' ', (int)value);
            }
        }

        public string SpacesBetweenAssigmentAndValue { get; private set; }
        public uint NumSpacesBetweenAssigmentAndValue
        {
            set
            {
                SpacesBetweenAssigmentAndValue = new string(' ', (int)value);
            }
        }

        public bool IsNewLineBeforeSection { get; set; } = true;
        public bool IsNewLineAfterSection { get; set; } = false;
        public bool IsNewLineAfterProperty { get; set; } = false;
        public bool IsNewLineBeforeProperty { get; set; } = false;

        public IniWriterStyle()
        {
            NewLineType = Environment.NewLine == "\r\n" ? ENewLine.Windows : ENewLine.Unix_Mac;
            NumSpacesBetweenAssigmentAndValue = 1;
            NumSpacesBetweenKeyAndAssigment = 1;
        }
    }
}
