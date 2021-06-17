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
        static void RegexMatch()
        {
            string VALIDATION_RULE_PATTERN = @"(^(?<name>[A-Za-z]{1,})$|^(?<name>[A-Za-z]{1,})[\s]*\((?<params>\S*)\))";
            string rule = "StrMinMaxLen(2,10)"; 
            Match nameMatch = new Regex(VALIDATION_RULE_PATTERN).Match(rule);
            Group nameGroup = nameMatch.Groups["name"];
            Group paramsGroup = nameMatch.Groups["params"];

            string ruleName = nameGroup.Success ? nameGroup.Value.Trim() : null;
            string[] ruleParams = new string[0];
            if (paramsGroup.Success && !string.IsNullOrEmpty(paramsGroup.Value))
            {
                ruleParams = (from v in paramsGroup.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) where !string.IsNullOrEmpty(v) select v.Trim()).ToArray();
            }

            Console.WriteLine(string.IsNullOrEmpty(ruleName) ? "" : ruleName);
            Console.WriteLine(string.Join(",",ruleParams));
        }

        static void Main(string[] args)
        {
            string excelPath = @"E:\WorkSpace\DotLuaGameProject\GameTools\Config\Test.xlsx";
            ExcelReader.logHandler = PrintLog;
            WDBSheet[] sheets = ExcelReader.ReadFromFile(excelPath, null);
            if (!WDBVerify.VerifySheets(sheets, out var errors))
            {
                foreach (var error in errors)
                {
                    PrintLog(LogType.Error, error);
                }
            }
            ExcelReader.logHandler = null;
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
