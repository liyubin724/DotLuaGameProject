namespace DotEngine.Config.WDB
{
    public static class WDBErrorMessages
    {
        public const string SHEET_NAME_EMPTY_ERROR = "The name of sheet is empty";
        public const string SHEET_NAME_FORMAT_ERROR = "The name({0}) of the sheet is not valid";
        public const string SHEET_FIELD_EMPTY_ERROR = "The field of the sheet({0}) is empty";
        public const string SHEET_ROW_EMPTY_ERROR = "THe row of the sheet({0}) is empty";
        
        public const string FIELD_NAME_EMPTY_ERROR = "The name of the field({0}) is empty";
        public const string FIELD_NAME_FORMAT_ERROR = "the name({0}) of the field({1}) is not match the regex.the name should start with a letter,and the length should be in range(3-9)";

        public const string ROW_CELL_COUNT_ERROR = "The count({0}) in row({1}) is not equeal to the count of field({2})";

        public const string CELL_CONTENT_CONVERT_ERROR = "The content({0}) of cell(row:{1},col:{2}) can't parser to {3}";
    }
}
