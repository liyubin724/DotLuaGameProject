using System.Collections.Generic;

namespace DotTool.ETD.Log
{
    public static class LogMessage
    {
        public static readonly int LOG_WARP_LINE = 0;

        public static readonly int LOG_FILE_NOT_EXIST = -1;
        public static readonly int LOG_FILE_NOT_EXCEL = -2;
        public static readonly int LOG_ARG_IS_NULL = -3;

        public static readonly int LOG_WORKBOOK_EMPTY = -100;
        public static readonly int LOG_WORKBOOK_VERIFY_FAILED = -101;

        public static readonly int LOG_SHEET_NAME_NULL = -201;
        public static readonly int LOG_SHEET_NAME_NOT_MATCH = -202;
        public static readonly int LOG_SHEET_ROW_LESS = -203;
        public static readonly int LOG_SHEET_COL_LESS = -204;

        public static readonly int LOG_WORKBOOK_START = 1100;
        public static readonly int LOG_WORKBOOK_END = 1101;

        public static readonly int LOG_SHEET_START = 2201;
        public static readonly int LOG_SHEET_END = 2202;
        public static readonly int LOG_IGNORE_SHEET = 2203;
        public static readonly int LOG_SHEET_FIELD_START = 2204;
        public static readonly int LOG_SHEET_FIELD_END = 2205;
        public static readonly int LOG_SHEET_LINE_START = 2206;
        public static readonly int LOG_SHEET_LINE_END = 2207;
        public static readonly int LOG_SHEET_FIELD_IGNORE = 2208;
        public static readonly int LOG_SHEET_FIELD_CREATE = 2209;
        public static readonly int LOG_SHEET_FIELD_DETAIL = 2210;
        public static readonly int LOG_SHEET_LINE_CREATE = 2211;
        public static readonly int LOG_SHEET_LINE_DETAIL = 2212;
        public static readonly int LOG_SHEET_FIELD_NAME_NULL = -2213;

        public static readonly int LOG_WORKBOOK_VERIFY_START = 3100;
        public static readonly int LOG_WORKBOOK_VERIFY_END = 3101;

        public static readonly int LOG_SHEET_VERIFY_START = 3201;
        public static readonly int LOG_SHEET_VERIFY_END = 3202;
        public static readonly int LOG_SHEET_FIELD_EMPTY = -3203;

        public static readonly int LOG_FIELD_VERIFY_START = 3301;
        public static readonly int LOG_FIELD_VERIFY_END = 3302;
        public static readonly int LOG_FIELD_VERIFY_NAME_NULL = -3303;
        public static readonly int LOG_FIELD_VERIFY_NAME_FORMAT = -3304;
        public static readonly int LOG_FIELD_VERIFY_NAME_REPEAT = -3305;
        public static readonly int LOG_FIELD_VERIFY_TYPE_ERROR = -3306;
        public static readonly int LOG_FIELD_VERIFY_PLATFORM_ERROR = -3307;
        public static readonly int LOG_FIELD_VERIFY_VALIDATION_ERROR = -3308;
        public static readonly int LOG_FIELD_VERIFY_ARRAY_VALUE_ERROR = -3311;

        public static readonly int LOG_LINE_VERIFY_START = 3401;
        public static readonly int LOG_LINE_VERIFY_END = 3402;
        public static readonly int LOG_LINE_COUNT_NOT_EQUAL = -3403;

        public static readonly int LOG_VALIDATION_CONVERT_ERROR = -4102;
        public static readonly int LOG_VALIDATION_NULL = -4106;
        public static readonly int LOG_VALIDATION_FORMAT_ERROR = -4107;
        public static readonly int LOG_VALIDATION_LEN_ERROR = -4108;
        public static readonly int LOG_VALIDATION_RANGE_MIN_ERROR = -4109;
        public static readonly int LOG_VALIDATION_RANGE_MAX_ERROR = -4110;
        public static readonly int LOG_VALIDATION_RANGE_MAX_LESS_MIN_ERROR = -4111;
        public static readonly int LOG_VALIDATION_CONTENT_REPEAT_ERROR = -4112;
        public static readonly int LOG_VALIDATION_TYPE_FOR_RANGE_ERROR = -4113;
        public static readonly int LOG_VALIDATION_TEXT_NOT_FOUND_ERROR = -4114;
        public static readonly int LOG_VALIDATION_TEXT_ID_NOT_FOUND_ERROR = -4115;
        public static readonly int LOG_VALIDATION_LUA_FORMAT_ERROR = -4116;

        private static Dictionary<int, string> logFormatDic = new Dictionary<int, string>();

        static LogMessage()
        {
            InitMsg();
        }

        private static void InitMsg()
        {
            logFormatDic.Add(LOG_WARP_LINE, "\r\n");
            logFormatDic.Add(LOG_FILE_NOT_EXIST, "File is not found.path = {0}");
            logFormatDic.Add(LOG_FILE_NOT_EXCEL, "File is not a excel file.path = {0}");
            logFormatDic.Add(LOG_ARG_IS_NULL, "The argument is null.");

            logFormatDic.Add(LOG_WORKBOOK_EMPTY, "Workbook is empty. path = {0}");
            logFormatDic.Add(LOG_WORKBOOK_VERIFY_FAILED, "Verify failed");

            logFormatDic.Add(LOG_SHEET_NAME_NULL, "The name of sheet is null.it will be ignored. index = {0}");
            logFormatDic.Add(LOG_SHEET_NAME_NOT_MATCH, "The sheet which named ({0}) will be ingored,because of the name should be like '{1}'");
            logFormatDic.Add(LOG_SHEET_ROW_LESS, "The count of the row({0}) is less then min value({1}).");
            logFormatDic.Add(LOG_SHEET_COL_LESS, "The count of the col({0}) is less then min value({1}).");
            logFormatDic.Add(LOG_WORKBOOK_START, "Start to convert excel to workbook.path = {0}");
            logFormatDic.Add(LOG_WORKBOOK_END, "End to convert excel to workbook.path = {0}");
            logFormatDic.Add(LOG_IGNORE_SHEET, "The sheet which named ({0}) will be ingored,because of the name of sheet is start with '#'");
            logFormatDic.Add(LOG_SHEET_START, "Start to convert the sheet({0})");
            logFormatDic.Add(LOG_SHEET_END, "End to convert the sheet({0}).");
            logFormatDic.Add(LOG_SHEET_FIELD_START, "Start to read field from sheet");
            logFormatDic.Add(LOG_SHEET_FIELD_IGNORE, "The field which named({0}) will be ignored,because of the name is start with '#' or '_'.");
            logFormatDic.Add(LOG_SHEET_FIELD_NAME_NULL, "The field will be ignored,because of the name is null.");
            logFormatDic.Add(LOG_SHEET_FIELD_CREATE, "The col of the sheet({0}) will be created as a field");
            logFormatDic.Add(LOG_SHEET_FIELD_DETAIL, "The detail content of the field is {0}");
            logFormatDic.Add(LOG_SHEET_FIELD_END, "End to read field from sheet");
            logFormatDic.Add(LOG_SHEET_LINE_START, "Start to read line from sheet");
            logFormatDic.Add(LOG_SHEET_LINE_END, "End to read line from sheet");
            logFormatDic.Add(LOG_SHEET_LINE_CREATE, "Read the line from sheet as a content.row = {0}");
            logFormatDic.Add(LOG_SHEET_LINE_DETAIL, "The content of the line is {0}.");

            logFormatDic.Add(LOG_WORKBOOK_VERIFY_START, "Start to verify workbook.path = {0}");
            logFormatDic.Add(LOG_WORKBOOK_VERIFY_END, "Verify workbook finish.path = {0}, result = {1}.");

            logFormatDic.Add(LOG_SHEET_VERIFY_START, "Start to verify Sheet.Name = {0}");
            logFormatDic.Add(LOG_SHEET_VERIFY_END, "Verify sheet finish.Name = {0}, Result = {1}.");
            logFormatDic.Add(LOG_SHEET_FIELD_EMPTY, "The count of the field is 0.");

            logFormatDic.Add(LOG_FIELD_VERIFY_START, "Start to verify Field.Detail = {0}");
            logFormatDic.Add(LOG_FIELD_VERIFY_END, "Verify Field finish. Result = {0}.");
            logFormatDic.Add(LOG_FIELD_VERIFY_NAME_NULL, "The name of the field is null");
            logFormatDic.Add(LOG_FIELD_VERIFY_NAME_FORMAT, "The name of the field is error.name={0},regex={1}");
            logFormatDic.Add(LOG_FIELD_VERIFY_NAME_REPEAT, "The name of the field is repeated.name={0}");
            logFormatDic.Add(LOG_FIELD_VERIFY_TYPE_ERROR, "The type of the field is error.");
            logFormatDic.Add(LOG_FIELD_VERIFY_PLATFORM_ERROR, "The platform of the field is error.");
            logFormatDic.Add(LOG_FIELD_VERIFY_VALIDATION_ERROR, "The validation of the field is error.rule = {0}");
            logFormatDic.Add(LOG_FIELD_VERIFY_ARRAY_VALUE_ERROR, "The type of value in array is not correct.type = {0}");

            logFormatDic.Add(LOG_LINE_VERIFY_START, "Start to verify line.row = {0}");
            logFormatDic.Add(LOG_LINE_VERIFY_END, "Verify line finish. Result = {0}.");
            logFormatDic.Add(LOG_LINE_COUNT_NOT_EQUAL, "The count of the cell in line is not equal to the count of the field");

            logFormatDic.Add(LOG_VALIDATION_CONVERT_ERROR, "The content of the cell can't convert to {0}. cell={1}");
            logFormatDic.Add(LOG_VALIDATION_NULL, "The content of the cell is null.row = {1},col={2}");
            logFormatDic.Add(LOG_VALIDATION_FORMAT_ERROR, "The format of the rule is error.col = {0},rule = {1}");
            logFormatDic.Add(LOG_VALIDATION_LEN_ERROR, "The lenght of the content is large then {0},row = {1},col = {2}.content = {3}");
            logFormatDic.Add(LOG_VALIDATION_TYPE_FOR_RANGE_ERROR, "The validation(Range) can only be used for number.row = {0},col = {1},type = {2}");
            logFormatDic.Add(LOG_VALIDATION_RANGE_MIN_ERROR, "The validation(Range) can't find the minValue.col = {0}");
            logFormatDic.Add(LOG_VALIDATION_RANGE_MAX_ERROR, "The validation(Range) can't find the maxValue.col = {0}");
            logFormatDic.Add(LOG_VALIDATION_RANGE_MAX_LESS_MIN_ERROR, "MaxValue is less then minValue.col = {0},min={1},max ={2}");
            logFormatDic.Add(LOG_VALIDATION_CONTENT_REPEAT_ERROR, "Content is Repeat.curCell = {0},targetCell={1}");
            logFormatDic.Add(LOG_VALIDATION_TEXT_NOT_FOUND_ERROR, "The sheet which named 'Text' not found.");
            logFormatDic.Add(LOG_VALIDATION_TEXT_ID_NOT_FOUND_ERROR, "The ID is not found in 'Text'.ID = {0}");
            logFormatDic.Add(LOG_VALIDATION_LUA_FORMAT_ERROR, "The content is not a function of lua.cell = {0}");
        }

        public static string GetLogMsg(int logID, params object[] datas)
        {
            if (logFormatDic.TryGetValue(logID, out string msg))
            {
                if (datas != null && datas.Length > 0)
                {
                    return string.Format(msg, datas);
                }
                else
                {
                    return msg;
                }
            }
            return string.Empty;
        }
    }
}
