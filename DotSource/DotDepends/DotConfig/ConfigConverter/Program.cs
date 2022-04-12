using Colorful;
using CommandLine;
using DotEngine.Config.WDB;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace DotTool.Config
{
    public enum OriginFileType
    {
        Excel = 0,
    }

    public enum TargetFormatType
    {
        Lua = 0,
        Json,
        NDB,
    }

    public enum TargetPlatformType
    {
        Client = 0,
        Server,
    }

    public class Options
    {
        [Option('i',"input",Required =true,HelpText ="The path of the file")]
        public string InputFilePath { get; set; }
        [Option('r', "file-type", Required = true, HelpText = "The type of the file")]
        public OriginFileType FileType { get; set; }
        [Option('o',"output",Required =true,HelpText ="The direction of the output")]
        public string OutputFileDir { get; set; }
        [Option('e',"extension",Required =true,HelpText ="")]
        public string OutputFileExtension { get; set; }
        [Option('t',"target",Required =true,HelpText ="")]
        public TargetFormatType FormatType { get; set; }
        [Option('p',"platform",Required =true,HelpText ="")]
        public TargetPlatformType PlatformType { get; set; }

        [Option('l',"error",Required =false,HelpText ="")]
        public string ErrorLogPath { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //TextFieldParser parser = new TextFieldParser(@"D:\sticker.txt");
            //parser.SetDelimiters(new string[] { "\t" });
            //while(!parser.EndOfData)
            //{

            //    long lineNumber = parser.LineNumber;
            //    string[] fields = parser.ReadFields();
            //    Console.WriteLine(lineNumber.ToString() + " = " + (parser.LineNumber).ToString());
            //    Console.WriteLine(string.Join(",", fields));
            //}

            //parser.Dispose();
            Parser.Default.ParseArguments<Options>(args).WithParsed(OnSuccess).WithNotParsed(OnFaile);
            System.Console.ReadLine();
        }

        static void OnFaile(IEnumerable<Error> errors)
        {
            foreach(var error in errors)
            {
                Console.WriteLine(error.ToString(),Color.Red);
            }
        }

        static void OnSuccess(Options options)
        {
            if(string.IsNullOrEmpty(options.InputFilePath))
            {
                Console.WriteLine("The path of input is empty");
                return;
            }
            if(!File.Exists(options.InputFilePath))
            {
                Console.WriteLine($"The path({options.InputFilePath}) is not found");
                return;
            }

            if(!Directory.Exists(options.OutputFileDir))
            {
                var dInfo = Directory.CreateDirectory(options.OutputFileDir);
                if(dInfo == null || !dInfo.Exists)
                {
                    Console.WriteLine($"Create directory for outputFile({options.OutputFileDir}) failed");
                    return;
                }
            }

            WDBSheet[] sheets = null;
            StringBuilder errorStrBuilder = null;
            if(!string.IsNullOrEmpty(options.ErrorLogPath))
            {
                var logDir = Path.GetDirectoryName(options.ErrorLogPath);
                if(!Directory.Exists(logDir))
                {
                    var dInfo = Directory.CreateDirectory(logDir);
                    if(dInfo!=null && dInfo.Exists)
                    {
                        errorStrBuilder = new StringBuilder();
                    }
                }
            }

            System.Action<LogType, string> printLogHandler = (logType, message) =>
             {
                 PrintLog(logType, message);

                 errorStrBuilder?.AppendLine($"{logType.ToString()} -> {message}");
             };

            if (options.FileType == OriginFileType.Excel)
            {
                sheets = WDBExcelReader.ReadFromFile(options.InputFilePath, printLogHandler);
            }

            if(sheets == null || sheets.Length == 0)
            {
                printLogHandler(LogType.Error, "The wdbsheet is null");
            }
            else
            {
                WDBContext context = new WDBContext();
                foreach(var sheet in sheets)
                {
                    sheet.Check(context);

                    if(context.HasError())
                    {
                        foreach(var error in context.GetErrors())
                        {
                            printLogHandler(LogType.Error, error);
                        }
                        continue;
                    }

                    string targetContent = null;
                    if(options.FormatType == TargetFormatType.Json)
                    {
                        targetContent = WDBJsonWriter.WriteToJson(sheet, options.PlatformType);
                    }else if(options.FormatType == TargetFormatType.Lua)
                    {
                        targetContent = WDBLuaWriter.WriteToLua(sheet, options.PlatformType);
                    }
                    if(!string.IsNullOrEmpty(targetContent))
                    {
                        string filePath = $"{options.OutputFileDir}/{sheet.Name}{options.OutputFileExtension}";
                        File.WriteAllText(filePath, targetContent);
                    }
                    else
                    {
                        printLogHandler(LogType.Error, "Converter failed");
                    }
                }
            }

            if(errorStrBuilder!=null)
            {
                File.WriteAllText(options.ErrorLogPath, errorStrBuilder.ToString());
            }
        }

        private static void PrintLog(LogType logType,string message)
        {
            if(logType == LogType.Info)
            {
                Console.WriteLine(message, Color.White);
            }else if(logType == LogType.Warning)
            {
                Console.WriteLine(message, Color.Yellow);
            }else if(logType == LogType.Error)
            {
                Console.WriteLine(message, Color.Red);
            }
        }
    }
}
