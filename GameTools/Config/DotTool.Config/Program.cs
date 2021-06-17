﻿using DotEditor.Config.WDB;
using DotEngine.Config.WDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;
using DotTool.ScriptGenerate;
using DotEngine.Context;

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

            string templateTxtPath = @"E:\WorkSpace\DotLuaGameProject\GameTools\Config\template.txt";
            string templateTxt = File.ReadAllText(templateTxtPath);

            StringContextContainer context = new StringContextContainer();
            foreach(var sheet in sheets)
            {
                context.Add("__sheet__", sheet);
                TemplateEngine.GenerateFile("D:/lua.txt", context, templateTxt, new string[] { typeof(WDBSheet).Assembly.Location},EntryConfig.Default);
                context.Remove("__sheet__");
            }    

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
