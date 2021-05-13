using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.WDB
{
    public static class WDBConst
    {
        public const string CONTEXT_SHEET_NAME = "sheet";
        public const string CONTEXT_FIELD_NAME = "field";
        public const string CONTEXT_CELL_NAME = "cell";

        public const string VALIDATION_FIELD_RULE_PARAM_ERR = "The params of the rule({0}) is error.";

        public const string VALIDATION_CELL_COMMON_ERR = "[Error At (row = {0},col = {1})]";
        public const string VALIDATION_CELL_ARG_NULL_ERR = "The argument is null.";
        public const string VALIDATION_CELL_CONVERT_ERR = "The value({0}) of the cell can't convert to {1}";
        public const string VALIDATION_CELL_VAULE_EMPTY_ERR = "The value of the cell is empty";
        public const string VALIDATION_CELL_STRING_TOO_LONG_ERR = "The length of the value({0}) is ";

        private static StringBuilder tempMessageBuilder = new StringBuilder();
        public static string GetCellErrorMsg(int row,int col, string msgFormat, params object[] values)
        {
            tempMessageBuilder.Clear();
            if(!string.IsNullOrEmpty(msgFormat))
            {
                if(values == null || values.Length == 0)
                {
                    tempMessageBuilder.Append(msgFormat);
                }else
                {
                    tempMessageBuilder.Append(string.Format(msgFormat, values));
                }
            }
            tempMessageBuilder.AppendLine(string.Format(VALIDATION_CELL_COMMON_ERR, row, col));
            return tempMessageBuilder.ToString();
        }
    }
}
