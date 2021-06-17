using DotEditor.Config.WDB;
using DotEngine.Config.WDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;

namespace DotTool.Config
{
    class Program
    {
        static void Main(string[] args)
        {
            string excelPath = @"E:\WorkSpace\DotLuaGameProject\GameTools\Config\Test.xlsx";
            WDBFromExcelReader.logHandler = PrintLog;
            WDBSheet[] sheets = WDBFromExcelReader.ReadFromFile(excelPath, null);
            if (!WDBVerify.VerifySheets(sheets, out var errors))
            {
                foreach (var error in errors)
                {
                    PrintLog(LogType.Error, error);
                }
            }
            WDBFromExcelReader.logHandler = null;
            Console.ReadKey();
        }

        static void PrintLog(LogType logType,string message)
        {
            if(logType == LogType.Error)
            {
                Colorful.Console.WriteLine(message, Color.Red);
            }else if(logType == LogType.Warning)
            {
                Colorful.Console.WriteLine(message, Color.Yellow);
            }else
            {
                Colorful.Console.WriteLine(message, Color.White);
            }    
        }
    }
}
