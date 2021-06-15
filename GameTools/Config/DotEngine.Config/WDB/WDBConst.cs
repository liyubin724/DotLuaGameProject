using System.Text;

namespace DotEngine.Config.WDB
{
    public static class WDBConst
    {
        public const string CONTEXT_ERRORS_NAME = "__errors__";
        public const string CONTEXT_SHEET_NAME = "__sheet__";
        public const string CONTEXT_FIELD_NAME = "__field__";
        public const string CONTEXT_CELL_NAME = "__cell__";

        public const string VERIFY_SHEET_NAME_REGEX = @"^[A-Z][A-Za-z]{2,9}";
        public const string VERIFY_FIELD_NAME_REGEX = @"^[A-Z][A-Za-z0-9]{2,9}";

        public const string VERIFY_SHEET_NAME_EMPTY_ERR = "the name of the sheet is empty";
        public const string VERIFY_SHEET_NAME_REGEX_ERR = "the name(name = {0}) of the sheet should match the regex";
        public const string VERIFY_SHEET_NO_FIELD_ERR = "the count of the field in sheet({0}) is 0.";
        public const string VERIFY_SHEET_NO_ROW_ERR = "the count of the row in sheet({0}) is 0.";
        public const string VERIFY_SHEET_FIELD_ROW_ERR = "the count({0}) of the field is not equal the count of cell in row({1})";

        public const string VERIFY_CELL_COL_NOTSAME_ERR = "the col of cell(row = {0},col={1}) is not same to the field";

        public const string VERIFY_FIELD_NAME_EMPTY_ERR = "the name of the field(col = {0}) is empty";
        public const string VERIFY_FIELD_NAME_REGEX_ERR = "the name of the field(col = {0}) is not match the regex";
        public const string VERIFY_FIELD_TYPE_NONE_ERR = "the type of the field(col={0},name = {1}) is none";
        public const string VERIFY_FIELD_VALIDATIONS_ERR = "the validation of the field(col={0},name = {1},rule = {2}) is invalid";

        public const string VALIDATION_FIELD_RULE_PARAM_ERR = "The params of the rule({0}) is error.";

        public const string VALIDATION_CELL_COMMON_ERR = "[Error At (row = {0},col = {1})]";
        public const string VALIDATION_CELL_ARG_NULL_ERR = "The argument is null.";
        public const string VALIDATION_CELL_CONVERT_ERR = "The value({0}) of the cell can't convert to {1}";
        public const string VALIDATION_CELL_VAULE_EMPTY_ERR = "The value of the cell is empty";
        public const string VALIDATION_CELL_STRLen_TOO_LONG_ERR = "The length of the value({0}) is too long.";
        public const string VALIDATION_CELL_STRING_TOO_SHORT_ERR = "The length of the value({0}) is too short.";

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
