using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.Config.WDB
{
    public static class LogMessage
    {
        public static readonly string ERROR_DIRECTOR_NOT_EXIST = "The directory is not found.path = {0}";
        public static readonly string ERROR_FILE_NOT_EXCEL = "The file is not a excel file.path = {0}";
        public static readonly string ERROR_WORKBOOK_EMPTY = "The workbook is empty.path = {0}";

        public static readonly string INFO_START_READ_WORKBOOK = "Start to read workbook.path = {0}";
        public static readonly string INFO_END_READ_WORKBOOK = "End to read workbook.path = {0}";
        public static readonly string WARN_SHEET_NAME_EMPTY = "The name of the sheet is empty.";
        public static readonly string INFO_SHEET_IGNORE = "The sheet which named ({0}) will be ingored,because of the name of sheet is start with '#'";
        public static readonly string INFO_START_READ_SHEET = "Start to convert the sheet({0})";
        public static readonly string INFO_END_READ_SHEET = "End to convert the sheet({0}).";
        public static readonly string ERROR_SHEET_ROW_START_EMTPY = "The row in sheet is empty.row = {0}";
        public static readonly string ERROR_SHEET_COLUMN_START_EMPTY = "The col in sheet is empty.col = {0}";
        public static readonly string ERROR_SHEET_MRAK_FLAG = "The content of the cell should be marked({0})";
        public static readonly string ERROR_SHEET_ROW_LESS = "The count of the row({0}) is less then min value({1}).";
        public static readonly string ERROR_SHEET_COL_LESS = "The count of the col({0}) is less then min value({1}).";
        public static readonly string INFO_START_READ_FIELD = "Start to read field from sheet";
        public static readonly string INFO_END_READ_FIELD = "End to read field from sheet";
        public static readonly string INFO_CREATE_FIELD = "Create a field({0})";
        public static readonly string INFO_START_READ_LINE = "Start read line from sheet";
        public static readonly string INFO_END_READ_LINE = "End read line from sheet";
        public static readonly string INFO_LINE_EMPTY = "the line({0}) of the sheet is null";
        public static readonly string INFO_CREATE_LINE = "Create a line({0})";








    }

}
