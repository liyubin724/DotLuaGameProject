namespace DotTool.ETD.IO
{
    public static class WorkbookConst
    {
        public static string SHEET_MARK_FLAG = "#ETD#";
        public static string SHEET_NAME_REGEX = @"^[A-Z]\w{3,10}";
        public static int SHEET_ROW_START_INDEX = 0;
        public static int SHEET_COLUMN_START_INDEX = 0;
        public static int SHEET_FIELD_ROW_COUNT = 6;
        public static string SHEET_LINE_START_FLAG = "start";
        public static string SHEET_LINE_END_FLAG = "end";

        public static int MIN_COLUMN_COUNT = 2;

    }
}
