using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Config.WDB
{
    public static class WDBErrorMessages
    {
        public const string SHEET_NAME_EMPTY_ERROR = "";

        public const string FIELD_NAME_EMPTY_ERROR = "The name of the field({0}) is empty";
        public const string FIELD_NAME_REGEX_ERROR = "the name({0}) of the field({1}) is not match the regex.the name should start with a letter,and the length should be in range(3-9)";
        public const string FIELD_CONTENT_EMPTY_ERROR = "the content of the field({0}) is empty";

        public const string CELL_PARSE_ERROR = "The content({0}) of cell(row:{1},col:{2}) can't parser to {3}";
    }
}
