using System.Text;

namespace DotEditor.Config.WDB
{
    public static class WDBVerifyConst
    {
        public const string VERIFY_SHEET_NAME_REGEX = @"^[A-Z][A-Za-z]{2,9}";
        public const string VERIFY_FIELD_NAME_REGEX = @"^[A-Z][A-Za-z0-9]{1,9}";

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
    }
}
