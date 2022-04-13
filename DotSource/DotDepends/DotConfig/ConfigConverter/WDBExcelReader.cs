using DotEngine.Config.WDB;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotTool.Config
{
    public static class WDBExcelReader
    {
        private static Action<LogType, string> sm_LogHandler;
        private static WDBStyle sm_Style;

        public static WDBSheet[] ReadFromFile(string excelPath, Action<LogType, string> logHandler, WDBStyle style = null)
        {
            sm_LogHandler = logHandler;
            sm_Style = style ?? new WDBStyle();

            if (!IsExcel(excelPath))
            {
                logHandler?.Invoke(LogType.Error, string.Format(LogMessage.ERROR_FILE_NOT_EXCEL, excelPath));
                return null;
            }

            List<WDBSheet> sheetList = new List<WDBSheet>();
            try
            {
                using (FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    IWorkbook workbook = null;
                    string ext = Path.GetExtension(excelPath).ToLower();
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

            sm_LogHandler = null;
            sm_Style = null;

            return sheetList.ToArray();
        }

        private static WDBSheet ReadFromSheet(ISheet sheet)
        {
            sm_LogHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_START_READ_SHEET, sheet.SheetName));

            int firstRowNum = sheet.FirstRowNum;
            int lastRowNum = sheet.LastRowNum;
            while (true)
            {
                if (lastRowNum - firstRowNum + 1 <= WDBConst.MinRowCount)
                {
                    break;
                }
                IRow row = sheet.GetRow(firstRowNum);
                int firstColumnNum = row.FirstCellNum;
                int lastColumnNum = row.LastCellNum;
                if (lastColumnNum - firstColumnNum + 1 <= WDBConst.MinColumnCount)
                {
                    firstRowNum++;
                    continue;
                }
                for (int c = firstColumnNum; c <= lastColumnNum; c++)
                {
                    ICell cell = row.GetCell(c);
                    string cellValue = GetCellValue(cell);
                    if (string.IsNullOrEmpty(cellValue) || cellValue != sm_Style.MarkFlag)
                    {
                        continue;
                    }
                    else
                    {
                        sm_Style.RowStartIndex = firstRowNum;
                        sm_Style.RowEndIndex = lastRowNum;
                        sm_Style.ColumnStartIndex = c;

                        for(int rc = lastColumnNum;rc>=c;rc--)
                        {
                            ICell rCell = row.GetCell(rc);
                            string rCellValue = GetCellValue(rCell);
                            if (string.IsNullOrEmpty(rCellValue))
                            {
                                continue;
                            }
                            sm_Style.ColumnEndIndex = rc;
                            break;
                        }

                        break;
                    }
                }
                firstRowNum++;
            }

            if (sm_Style.RowCount <= WDBConst.MinRowCount)
            {
                sm_LogHandler?.Invoke(LogType.Error, string.Format(LogMessage.ERROR_SHEET_ROW_LESS, lastRowNum - firstRowNum + 1, WDBConst.MinRowCount));
                return null;
            }
            if (sm_Style.ColumnCount <= WDBConst.MinColumnCount)
            {
                sm_LogHandler?.Invoke(LogType.Error, string.Format(LogMessage.ERROR_SHEET_COL_LESS, sm_Style.ColumnCount, WDBConst.MinColumnCount));
                return null;
            }

            WDBField[] fields = ReadFieldFromSheet(sheet);
            if (fields == null || fields.Length == 0)
            {

                return null;
            }

            WDBRow[] rows = ReadRowFromSheet(sheet,fields);
            if (rows == null || rows.Length == 0)
            {

                return null;
            }

            WDBSheet sheetData = new WDBSheet(sheet.SheetName);
            sheetData.AddFields(fields);
            sheetData.AddRows(rows);

            sm_LogHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_END_READ_SHEET, sheet.SheetName));

            return sheetData;
        }

        private static WDBField[] ReadFieldFromSheet(ISheet sheet)
        {
            List<WDBField> fields = new List<WDBField>();

            for (int i = sm_Style.ColumnStartIndex + 1; i <= sm_Style.ColumnEndIndex; i++)
            {
                WDBField field = ReadFieldFromSheet(sheet, i);
                if (field != null)
                {
                    fields.Add(field);
                }
            }

            return fields.ToArray();
        }

        private static WDBField ReadFieldFromSheet(ISheet sheet, int fieldColumn)
        {
            sm_LogHandler?.Invoke(LogType.Info, LogMessage.INFO_START_READ_FIELD);

            IRow typeRow = sheet.GetRow(sm_Style.RowStartIndex + WDBConst.FieldTypeOffset);
            ICell typeCell = typeRow.GetCell(fieldColumn);
            string typeCellValue = GetCellValue(typeCell);
            if (string.IsNullOrEmpty(typeCellValue))
            {
                sm_LogHandler?.Invoke(LogType.Warning, LogMessage.WARN_EMPTY_TYPE_FIELD);
                return null;
            }

            WDBField field = WDBFieldFactory.CreateField(fieldColumn, typeCellValue);
            if (field == null)
            {
                sm_LogHandler?.Invoke(LogType.Warning, string.Format(LogMessage.WARN_INVALID_TYPE_FIELD, typeCellValue));
                return null;
            }

            IRow nameRow = sheet.GetRow(sm_Style.RowStartIndex + WDBConst.FieldNameOffset);
            field.Name = GetCellValue(nameRow.GetCell(fieldColumn));
            IRow descRow = sheet.GetRow(sm_Style.RowStartIndex + WDBConst.FieldDescOffset);
            field.Desc = GetCellValue(descRow.GetCell(fieldColumn));
            IRow platformRow = sheet.GetRow(sm_Style.RowStartIndex + WDBConst.FieldPlatformOffset);
            field.Platform = GetCellValue(platformRow.GetCell(fieldColumn));
            IRow defaultRow = sheet.GetRow(sm_Style.RowStartIndex + WDBConst.FieldDefaultOffset);
            if(defaultRow!=null)
            {
                field.DefaultContent = GetCellValue(defaultRow.GetCell(fieldColumn));
            }
            IRow validationRow = sheet.GetRow(sm_Style.RowStartIndex + WDBConst.FieldValidationOffset);
            if(validationRow!=null)
            {
                field.Validation = GetCellValue(validationRow.GetCell(fieldColumn));
            }

            sm_LogHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_CREATE_FIELD, field.ToString()));

            sm_LogHandler?.Invoke(LogType.Info, LogMessage.INFO_END_READ_FIELD);

            return field;
        }

        private static WDBRow[] ReadRowFromSheet(ISheet sheet,WDBField[] fields)
        {
            List<WDBRow> rows = new List<WDBRow>();

            bool isStarted = false;
            for (int i = sm_Style.RowStartIndex + WDBConst.MinRowCount; i < sm_Style.RowEndIndex; i++)
            {
                var cellValue = GetCellValue(sheet.GetRow(i).GetCell(sm_Style.ColumnStartIndex));
                if (isStarted)
                {
                    if (string.IsNullOrEmpty(cellValue) && cellValue == sm_Style.LineEndFlag)
                    {
                        break;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(cellValue) && cellValue == sm_Style.LineStartFlag)
                    {
                        isStarted = true;
                    }
                }
                if(isStarted)
                {
                    WDBRow row = ReadRowFromSheet(sheet, i,fields);
                    if (row != null)
                    {
                        rows.Add(row);
                    }
                }
            }

            return rows.ToArray();
        }

        private static WDBRow ReadRowFromSheet(ISheet sheet, int dataRow,WDBField[] fields)
        {
            sm_LogHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_START_READ_LINE, dataRow));

            WDBRow row = new WDBRow(dataRow);
            IRow r = sheet.GetRow(dataRow);
            for(int i = 0;i<fields.Length;i++)
            {
                string cellValue = GetCellValue(r.GetCell(fields[i].Column));
                row.AddCell(i, cellValue);
            }

            sm_LogHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_CREATE_ROW, row));

            sm_LogHandler?.Invoke(LogType.Info, string.Format(LogMessage.INFO_END_READ_ROW, dataRow));

            return row;
        }

        private static string GetCellValue(ICell cell)
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

        private static bool IsExcel(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return false;
            }

            string ext = Path.GetExtension(filePath);
            return !string.IsNullOrEmpty(ext) && (ext == ".xls" || ext == ".xlsx");
        }
    }
}
