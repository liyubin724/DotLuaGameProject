using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string SpacesBetweenKeyAndAssigment { get; private set; }
        public string SpacesBetweenAssigmentAndValue { get; private set; }
        public bool NewLineBeforeSection { get; set; } = false;
        public bool NewLineAfterSection { get; set; } = false;
        public bool NewLineAfterProperty { get; set; } = false;
        public bool NewLineBeforeProperty { get; set; } = false;
    }
}
