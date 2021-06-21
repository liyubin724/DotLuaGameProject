using DotEditor.Config.WDB;
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
using DotEngine.Config.NDB;

using CommandLine;

namespace DotTool.Config
{
    public enum OutputTargetType
    {
        NDB,
        Lua,
    }

    public enum OutputPlatformType
    {
        ClientAndServer = 0,
        Client,
        Server,
    }

    public class Options
    {
        [Option('f', "input-file", Required = false, HelpText = "the path of the file which is a excel")]
        public string InputExcelFile { get; set; }
        [Option('d', "input-dir", Required = false, HelpText = "the path of the dir which is contains some excel")]
        public string InputExcelDir { get; set; }

        [Option('o', "output", Required = true, HelpText = "OutputDir")]
        public string OutputDir { get; set; }
        [Option('t', "target", Required = true, HelpText = "OutputTargetType")]
        public OutputTargetType TargetType { get; set; } = OutputTargetType.Lua;
        [Option('p', "platform", Required = true, HelpText = "WDBFieldPlatform")]
        public WDBFieldPlatform PlatformType { get; set; } = WDBFieldPlatform.All;

        [Option('e', "extension", Required = false, HelpText = "LuaExtension")]
        public string LuaExtension { get; set; } = "txt";
        [Option('m', "template", Required = false, HelpText = "LuaTemplatePath")]
        public string LuaTemplatePath { get; set; }

        [Option('v', "verify", Required = false, HelpText = "IsNeedVerify")]
        public bool IsNeedVerify { get; set; } = true;
        [Option('l', "print-log", Required = false, HelpText = "IsPrintLog")]
        public bool IsPrintLog { get; set; } = false;
    }

    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args).WithParsed(RunOptions);
            //Console.ReadKey();
        }

        static void RunOptions(Options options)
        {
            if (string.IsNullOrEmpty(options.InputExcelDir) && string.IsNullOrEmpty(options.InputExcelFile))
            {
                Console.WriteLine("the dir or the path of excel is empty", Color.Red);
                return;
            }
            if (!Directory.Exists(options.OutputDir))
            {
                Console.WriteLine($"the dir({options.OutputDir}) is not found", Color.Red);
                return;
            }
            if (options.TargetType == OutputTargetType.Lua)
            {
                if (string.IsNullOrEmpty(options.LuaTemplatePath))
                {
                    Console.WriteLine($"the path of LuaTemplatePath is empty", Color.Red);
                    return;
                }
                if (!File.Exists(options.LuaTemplatePath))
                {
                    Console.WriteLine($"the template({options.LuaTemplatePath}) is not found", Color.Red);
                    return;
                }
            }
            WDBFromExcelReader.logHandler = (logType, message) =>
            {
                if (options.IsPrintLog && logType == LogType.Info)
                {
                    Console.WriteLine(message, Color.White);
                }
                else if (logType == LogType.Warning)
                {
                    Console.WriteLine(message, Color.Yellow);
                }
                else if (logType == LogType.Error)
                {
                    Console.WriteLine(message, Color.Red);
                }
            };

            WDBExcelStyle readStyle = new WDBExcelStyle();
            readStyle.TargetPlatform = options.PlatformType;

            WDBSheet[] sheets = null;
            if (!string.IsNullOrEmpty(options.InputExcelDir))
            {
                sheets = WDBFromExcelReader.ReadFromDirectory(options.InputExcelDir, readStyle);
            }
            else if (!string.IsNullOrEmpty(options.InputExcelFile))
            {
                sheets = WDBFromExcelReader.ReadFromFile(options.InputExcelFile, readStyle);
            }
            WDBFromExcelReader.logHandler = null;
            if (sheets == null || sheets.Length == 0)
            {
                Console.WriteLine("the sheet is not found", Color.Red);
                return;
            }
            if (options.IsNeedVerify)
            {
                if (!WDBVerify.VerifySheets(sheets, out var errors))
                {
                    Console.WriteLine("Some error was found in sheets");
                    if (errors != null && errors.Length > 0)
                    {
                        foreach (var error in errors)
                        {
                            Console.WriteLine("    " + error, Color.Red);
                        }
                    }
                    return;
                }
            }

            foreach (var sheet in sheets)
            {
                if (options.TargetType == OutputTargetType.NDB)
                {

                    WDBToNDBWriter.WriteToNDBFile(options.OutputDir, sheet);
                }
                else if (options.TargetType == OutputTargetType.Lua)
                {
                    string luaPath = $"{options.OutputDir}/{sheet.Name}.{options.LuaExtension}";
                    WDBToLuaWriter.WriteToLuaFile(sheet, luaPath, options.LuaTemplatePath);
                }
            }
        }

        static void Mainw(string[] args)
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
            foreach (var sheet in sheets)
            {
                context.Add("__sheet__", sheet);
                TemplateEngine.GenerateFile($"D:/{sheet.Name}.txt", context, templateTxt, new string[] { typeof(WDBSheet).Assembly.Location }, EntryConfig.Default);
                context.Remove("__sheet__");
            }

            foreach (var sheet in sheets)
            {
                WDBToNDBWriter.WriteToNDBFile(@"D:\", sheet);
            }
            byte[] dataBytes = File.ReadAllBytes(@"D:\TestSheet1.ndb");
            NDBSheet ndbSheet = new NDBSheet("TestSheet1");
            ndbSheet.SetData(dataBytes);

            for (int i = 0; i < ndbSheet.LineCount; ++i)
            {
                for (int j = 0; j < ndbSheet.FieldCount; ++j)
                {
                    var v = ndbSheet.GetDataByIndex<object>(i, j);
                    Console.Write("    " + v);
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        static void PrintLog(LogType logType, string message)
        {
            if (logType == LogType.Error)
            {
                Colorful.Console.WriteLine(message, Color.Red);
            }
            else if (logType == LogType.Warning)
            {
                Colorful.Console.WriteLine(message, Color.Yellow);
            }
            else
            {
                Colorful.Console.WriteLine(message, Color.White);
            }
        }
    }
}
