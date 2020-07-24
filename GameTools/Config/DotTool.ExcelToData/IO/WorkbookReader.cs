using DotTool.ETD.Data;
using DotTool.ETD.Fields;
using DotTool.ETD.Log;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ETDField = DotTool.ETD.Data.Field;
using ETDLine = DotTool.ETD.Data.Line;
using ETDSheet = DotTool.ETD.Data.Sheet;
using ETDWorkbook = DotTool.ETD.Data.Workbook;

namespace DotTool.ETD.IO
{
    public class WorkbookReader
    {
        private LogHandler logHandler = null;
        public WorkbookReader(LogHandler handler)
        {
            logHandler = handler;
        }

        private bool IsExcel(string filePath)
        {
            if(string.IsNullOrEmpty(filePath) && !File.Exists(filePath))
            {
                return false;
            }

            string ext = Path.GetExtension(filePath);
            return !string.IsNullOrEmpty(ext) && (ext == ".xls" || ext == ".xlsx");
        }

        public ETDWorkbook[] ReadExcelFromDir(string excelDir)
        {
            if (string.IsNullOrEmpty(excelDir) || !Directory.Exists(excelDir))
            {
                return null;
            }

            string[] excelFields = (from file in Directory.GetFiles(excelDir, "*.*", SearchOption.AllDirectories)
                                    where IsExcel(file)
                                    select file).ToArray();

            List<ETDWorkbook> books = new List<ETDWorkbook>();
            if (excelFields != null && excelFields.Length > 0)
            {
                foreach(var f in excelFields)
                {
                    ETDWorkbook book = ReadExcelFromFile(f);
                    if(book!=null)
                    {
                        books.Add(book);
                    }
                }
            }

            return books.ToArray();
        }

        public ETDWorkbook ReadExcelFromFile(string excelPath)
        {
            if(!IsExcel(excelPath))
            {
                return null;
            }
            string ext = Path.GetExtension(excelPath).ToLower();
            ETDWorkbook etdWorkbook = new ETDWorkbook(excelPath);
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
                    logHandler.Log(LogType.Warning, LogMessage.LOG_WORKBOOK_EMPTY, excelPath);
                    return null;
                }

                logHandler.Log(LogType.Info, LogMessage.LOG_WORKBOOK_START, excelPath);

                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    ISheet sheet = workbook.GetSheetAt(i);

                    string sheetName = sheet.SheetName;
                    if (string.IsNullOrEmpty(sheetName))
                    {
                        logHandler.Log(LogType.Warning, LogMessage.LOG_SHEET_NAME_NULL, i);
                        continue;
                    }
                    if (sheetName.StartsWith("#"))
                    {
                        logHandler.Log(LogType.Info, LogMessage.LOG_IGNORE_SHEET, sheetName);
                        continue;
                    }
                    if (!Regex.IsMatch(sheetName, WorkbookConst.SHEET_NAME_REGEX))
                    {
                        logHandler.Log(LogType.Error, LogMessage.LOG_SHEET_NAME_NOT_MATCH, sheetName, WorkbookConst.SHEET_NAME_REGEX);
                        continue;
                    }

                    Sheet dataSheet = ReadFromSheet(sheet);
                    etdWorkbook.AddSheet(dataSheet);
                }

                logHandler.Log(LogType.Info, LogMessage.LOG_WORKBOOK_END, excelPath);
            }
            return etdWorkbook;
        }

        private ETDSheet ReadFromSheet(ISheet sheet)
        {
            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_START, sheet.SheetName);

            IRow firstRow = sheet.GetRow(WorkbookConst.SHEET_ROW_START_INDEX);
            if(firstRow==null)
            {
                return null;
            }
            ICell firstCell = firstRow.GetCell(WorkbookConst.SHEET_COLUMN_START_INDEX);
            if(firstCell == null)
            {
                return null;
            }
            string flagContent = GetCellStringValue(firstCell);
            if(string.IsNullOrEmpty(flagContent) || flagContent != WorkbookConst.SHEET_MARK_FLAG)
            {
                return null;
            }

            int firstRowNum = WorkbookConst.SHEET_ROW_START_INDEX;
            int lastRowNum = sheet.LastRowNum;

            int firstColNum = sheet.GetRow(firstRowNum).FirstCellNum;
            int lastColNum = sheet.GetRow(firstRowNum).LastCellNum;

            int rowCount = lastRowNum - firstRowNum + 1;
            int colCount = lastColNum - firstColNum + 1;

            if (rowCount < WorkbookConst.SHEET_FIELD_ROW_COUNT)
            {
                logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_ROW_LESS, rowCount, WorkbookConst.SHEET_FIELD_ROW_COUNT);
                return null;
            }
            if (colCount < WorkbookConst.MIN_COLUMN_COUNT)
            {
                logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_COL_LESS, colCount, WorkbookConst.MIN_COLUMN_COUNT);
                return null;
            }

            ETDSheet sheetData = new ETDSheet(sheet.SheetName);
            ReadFieldFromSheet(sheetData, sheet);
            ReadLineFromSheet(sheetData, sheet);

            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_END, sheet.SheetName);

            return sheetData;
        }

        private void ReadFieldFromSheet(ETDSheet sheetData, ISheet sheet)
        {
            MethodInfo getFieldMI = typeof(FieldFactory).GetMethod("GetField", BindingFlags.Public | BindingFlags.Static);

            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_FIELD_START);

            int firstRowNum = sheet.FirstRowNum;
            int lastRowNum = firstRowNum + WorkbookConst.SHEET_FIELD_ROW_COUNT - 1;
            int firstColNum = sheet.GetRow(firstRowNum).FirstCellNum;
            int lastColNum = sheet.GetRow(firstRowNum).LastCellNum;

            for(int c = firstColNum+1;c<lastColNum;++c)
            {
                object[] datas = new object[WorkbookConst.SHEET_FIELD_ROW_COUNT + 1];
                datas[0] = c;
                for (int r = firstRowNum;r<lastRowNum;++r)
                {
                    IRow row = sheet.GetRow(r);
                    string cellValue = null;
                    if(row!=null)
                    {
                        cellValue = GetCellStringValue(row.GetCell(c));
                    }
                    datas[r - firstRowNum + 1] = cellValue;
                }

                string nameContent = (string)datas[1];
                if (string.IsNullOrEmpty(nameContent))
                {
                    logHandler.Log(LogType.Warning, LogMessage.LOG_SHEET_FIELD_NAME_NULL);
                    continue;
                }
                if (nameContent.StartsWith("#") || nameContent.StartsWith("_"))
                {
                    logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_FIELD_IGNORE, nameContent);
                    continue;
                }

                logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_FIELD_CREATE, c);

                Field field = (Field)getFieldMI.Invoke(null, datas);
                sheetData.AddField(field);

                logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_FIELD_DETAIL, field.ToString());
            }
            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_FIELD_END);
        }

        private void ReadLineFromSheet(ETDSheet sheetData, ISheet sheet)
        {
            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_LINE_START);

            int firstRowNum = WorkbookConst.SHEET_FIELD_ROW_COUNT;
            int lastRowNum = sheet.LastRowNum;

            int firstColNum = sheet.GetRow(WorkbookConst.SHEET_ROW_START_INDEX).FirstCellNum;
            int lastColNum = sheet.GetRow(WorkbookConst.SHEET_ROW_START_INDEX).LastCellNum;

            bool isStart = false;
            for(int r = firstRowNum;r< lastRowNum;++r)
            {
                IRow row = sheet.GetRow(r);
                if (row == null)
                {
                    continue;
                }
                string cellValue = GetCellStringValue(row.GetCell(firstColNum));
                if(string.IsNullOrEmpty(cellValue))
                {
                    if(!isStart)
                    {
                        continue;
                    }
                }else
                {
                    if(!isStart && cellValue == WorkbookConst.SHEET_LINE_START_FLAG)
                    {
                        isStart = true;
                    }else if(isStart && cellValue == WorkbookConst.SHEET_LINE_END_FLAG)
                    {
                        isStart = false;
                        break;
                    }
                }

                logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_LINE_CREATE, r);

                ETDLine line = new ETDLine(r);
                for(int c = firstColNum+1;c<lastColNum;c++)
                {
                    ETDField field = sheetData.GetFieldByCol(c);
                    ICell valueCell = row.GetCell(field.Col);
                    line.AddCell(field.Col, GetCellStringValue(valueCell));
                }
                sheetData.AddLine(line);

                logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_LINE_DETAIL, line.ToString());
            }

            logHandler.Log(LogType.Info, LogMessage.LOG_SHEET_LINE_END);
        }

        private string GetCellStringValue(ICell cell)
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
