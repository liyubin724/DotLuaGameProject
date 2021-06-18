using DotEngine.Config.WDB;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DotEditor.Config.WDB
{
    public enum LogType
    {
        Info = 0,
        Warning,
        Error,
    }

    public delegate void OnPrintLog(LogType logType, string msg);
    public static class WDBFromExcelReader
    {
        public static OnPrintLog logHandler;

        private static WDBExcelStyle readerExcelStyle;

        public static WDBSheet[] ReadFromDirectory(string excelDir, WDBExcelStyle readerStyle = null)
        {
            if (string.IsNullOrEmpty(excelDir) || !Directory.Exists(excelDir))
            {
                logHandler?.Invoke(LogType.Error, string.Format(LogMessage.ERROR_DIRECTOR_NOT_EXIST, excelDir));
                return null;
            }

            string[] excelFields = (from file in Directory.GetFiles(excelDir, "*.*", SearchOption.AllDirectories)
                                    where IsExcel(file)
                                    select file).ToArray();

            List<WDBSheet> sheetList = new List<WDBSheet>();
            if (excelFields != null && excelFields.Length > 0)
            {
                foreach (var f in excelFields)
                {
                    WDBSheet[] sheets = ReadFromFile(f, readerStyle);
                    if (sheets != null && sheets.Length > 0)
                    {
                        sheetList.AddRange(sheets);
                    }
                }
            }
            return sheetList.ToArray();
        }

        public static WDBSheet[] ReadFromFile(string excelPath, WDBExcelStyle readerStyle = null)
        {
            readerExcelStyle = readerStyle;
            if (readerExcelStyle == null)
            {
                readerExcelStyle = WDBExcelStyle.DefaultStyle;
            }

            if (!IsExcel(excelPath))
            {
                logHandler?.Invoke(LogType.Error, string.Format(LogMessage.ERROR_FILE_NOT_EXCEL, excelPath));
                return null;
            }

            List<WDBSheet> sheetList = new List<WDBSheet>();

            string ext = Path.GetExtension(excelPath).ToLower();
            try
            {
                using (FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    IWorkbook workbook = null;
                    if (ext == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    else
                    {
                        workbook = new HSSFWorkbook(fs);
                    }

                    if (workbook == null || workbook.NumberOfSheets == 0)
                    {
                        logHandler?.Invoke(LogType.Error, string.Format(LogMessage.ERROR_WORKBOOK_EMPTY, excelPath));
                        return null;
                    }

                    logHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_START_READ_WORKBOOK, excelPath));

                    for (int i = 0; i < workbook.NumberOfSheets; i++)
                    {
                        ISheet sheet = workbook.GetSheetAt(i);

                        string sheetName = sheet.SheetName;
                        if (string.IsNullOrEmpty(sheetName))
                        {
                            logHandler?.Invoke(LogType.Warning, LogMessage.WARN_SHEET_NAME_EMPTY);
                            continue;
                        }
                        if (sheetName.StartsWith("#"))
                        {
                            logHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_SHEET_IGNORE, sheetName));
                            continue;
                        }

                        WDBSheet wdbSheet = ReadFromSheet(sheet);
                        sheetList.Add(wdbSheet);
                    }

                    logHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_END_READ_WORKBOOK, excelPath));
                }
            }
            catch (Exception e)
            {
                logHandler?.Invoke(LogType.Error, e.Message);
            }

            readerExcelStyle = null;
            return sheetList.ToArray();
        }

        private static WDBSheet ReadFromSheet(ISheet sheet)
        {
            logHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_START_READ_SHEET, sheet.SheetName));

            IRow firstRow = sheet.GetRow(readerExcelStyle.RowStartIndex);
            if (firstRow == null)
            {
                logHandler?.Invoke(LogType.Error, string.Format(LogMessage.ERROR_SHEET_ROW_START_EMTPY, readerExcelStyle.RowStartIndex));
                return null;
            }

            ICell firstCell = firstRow.GetCell(readerExcelStyle.ColumnStartIndex);
            if (firstCell == null)
            {
                logHandler?.Invoke(LogType.Error, string.Format(LogMessage.ERROR_SHEET_COLUMN_START_EMPTY, readerExcelStyle.ColumnStartIndex));
                return null;
            }
            string flagContent = GetCellStringValue(firstCell);
            if (string.IsNullOrEmpty(flagContent) || flagContent != readerExcelStyle.MarkFlag)
            {
                logHandler?.Invoke(LogType.Error, string.Format(LogMessage.ERROR_SHEET_MRAK_FLAG, readerExcelStyle.MarkFlag));
                return null;
            }

            int firstRowNum = readerExcelStyle.RowStartIndex;
            int lastRowNum = sheet.LastRowNum;

            int firstColNum = sheet.GetRow(firstRowNum).FirstCellNum;
            int lastColNum = sheet.GetRow(firstRowNum).LastCellNum;

            int rowCount = lastRowNum - firstRowNum + 1;
            int colCount = lastColNum - firstColNum + 1;

            if (rowCount < readerExcelStyle.FieldRowCount)
            {
                logHandler?.Invoke(LogType.Error, string.Format(LogMessage.ERROR_SHEET_ROW_LESS, rowCount, readerExcelStyle.FieldRowCount));
                return null;
            }
            if (colCount < readerExcelStyle.ColumnMinCount)
            {
                logHandler?.Invoke(LogType.Error, string.Format(LogMessage.ERROR_SHEET_COL_LESS, colCount, readerExcelStyle.ColumnMinCount));
                return null;
            }
            WDBSheet sheetData = new WDBSheet(sheet.SheetName);
            ReadFieldFromSheet(sheetData, sheet);
            ReadLineFromSheet(sheetData, sheet);

            logHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_END_READ_SHEET, sheet.SheetName));

            return sheetData;
        }

        private static void ReadFieldFromSheet(WDBSheet sheetData, ISheet sheet)
        {
            logHandler?.Invoke(LogType.Info, LogMessage.INFO_START_READ_FIELD);

            MethodInfo createFieldMI = typeof(WDBUtility).GetMethod("CreateField", BindingFlags.Public | BindingFlags.Static);

            int firstRowNum = readerExcelStyle.RowStartIndex;
            int lastRowNum = firstRowNum + readerExcelStyle.FieldRowCount;
            int firstColNum = sheet.GetRow(firstRowNum).FirstCellNum;
            int lastColNum = sheet.GetRow(firstRowNum).LastCellNum;

            for (int c = firstColNum + 1; c < lastColNum; ++c)
            {
                object[] datas = new object[readerExcelStyle.FieldRowCount + 1];
                datas[0] = c;
                for (int r = firstRowNum; r < lastRowNum; ++r)
                {
                    IRow row = sheet.GetRow(r);
                    string cellValue = null;
                    if (row != null)
                    {
                        cellValue = GetCellStringValue(row.GetCell(c));
                    }
                    datas[r - firstRowNum + 1] = cellValue;
                }

                WDBField field = (WDBField)createFieldMI.Invoke(null, datas);
                logHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_CREATE_FIELD, field));

                sheetData.AddField(field);
            }
            logHandler?.Invoke(LogType.Info, LogMessage.INFO_END_READ_FIELD);
        }

        private static void ReadLineFromSheet(WDBSheet sheetData, ISheet sheet)
        {
            logHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_START_READ_LINE));

            int firstRowNum = readerExcelStyle.RowStartIndex + readerExcelStyle.FieldRowCount;
            int lastRowNum = sheet.LastRowNum;

            int firstColNum = sheet.GetRow(readerExcelStyle.RowStartIndex).FirstCellNum;
            int lastColNum = sheet.GetRow(readerExcelStyle.RowStartIndex).LastCellNum;

            bool isStart = false;
            for (int r = firstRowNum; r < lastRowNum; ++r)
            {
                IRow row = sheet.GetRow(r);
                if (row == null)
                {
                    logHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_LINE_EMPTY, r));
                    continue;
                }
                string cellValue = GetCellStringValue(row.GetCell(firstColNum));
                if (string.IsNullOrEmpty(cellValue))
                {
                    if (!isStart)
                    {
                        continue;
                    }
                }
                else
                {
                    if (!isStart && cellValue == readerExcelStyle.LineStartFlag)
                    {
                        isStart = true;
                    }
                    else if (isStart && cellValue == readerExcelStyle.LineEndFlag)
                    {
                        isStart = false;
                        break;
                    }
                }

                WDBLine line = sheetData.AddLine(r);
                for (int c = firstColNum + 1; c < lastColNum; c++)
                {
                    ICell valueCell = row.GetCell(c);
                    line.AddCell(c, GetCellStringValue(valueCell));
                }
                logHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_CREATE_LINE, line));
            }

            logHandler?.Invoke(LogType.Info, LogMessage.INFO_END_READ_LINE);
        }

        private static bool IsExcel(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return false;
            }

            string ext = Path.GetExtension(filePath);
            return !string.IsNullOrEmpty(ext) && (ext == ".xls" || ext == ".xlsx");
        }

        private static string GetCellStringValue(ICell cell)
        {
            if (cell == null)
                return null;

            CellType cType = cell.CellType;
            if (cType == CellType.Formula)
            {
                cType = cell.CachedFormulaResultType;
            }

            if (cType == CellType.Unknown || cType == CellType.Blank || cType == CellType.Error)
            {
                return null;
            }
            else if (cType == CellType.String)
            {
                return cell.StringCellValue;
            }
            else if (cType == CellType.Numeric)
            {
                return cell.NumericCellValue.ToString();
            }
            else if (cType == CellType.Boolean)
            {
                return cell.BooleanCellValue.ToString();
            }

            return null;
        }
    }
}
