using Colorful;
using CommandLine;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DotTool.Net
{
    public enum LogType
    {
        Info = 0,
        Warning,
        Error,
    }

    public enum TargetFormatType
    {
        Lua = 0,
    }

    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "The path of the file")]
        public string InputFilePath { get; set; }
        [Option('f', "format-type", Required = true, HelpText = "")]
        public TargetFormatType FormatType { get; set; }
        [Option('e', "extension", Required = true, HelpText = "")]
        public string OutputFileExtension { get; set; }
        [Option('o', "output", Required = true, HelpText = "The dir of the output")]
        public string OutputFileDir { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(OnSuccess).WithNotParsed(OnFaile);
        }

        static void OnFaile(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                PrintLog(LogType.Error, error.ToString());
            }
        }

        static void OnSuccess(Options options)
        {
            if (string.IsNullOrEmpty(options.InputFilePath))
            {
                Console.WriteLine("The path of input is empty");
                return;
            }
            if (!File.Exists(options.InputFilePath))
            {
                Console.WriteLine($"The path({options.InputFilePath}) is not found");
                return;
            }

            if (!Directory.Exists(options.OutputFileDir))
            {
                var dInfo = Directory.CreateDirectory(options.OutputFileDir);
                if (dInfo == null || !dInfo.Exists)
                {
                    Console.WriteLine($"Create directory for outputFile({options.OutputFileDir}) failed");
                    return;
                }
            }

            PBMessageConfig config = XMLSerializeUtility.ReadFromFile<PBMessageConfig>(options.InputFilePath, false);
            if(config == null)
            {
                PrintLog(LogType.Error, "read config failed");
                return;
            }
            if(string.IsNullOrEmpty(config.ClassName))
            {
                PrintLog(LogType.Error, "the class-name of the config is empty");
                return;
            }

            string content = null;
            if(options.FormatType == TargetFormatType.Lua)
            {
                content = PBConfigLuaWriter.WriteToLua(config);
            }

            if(string.IsNullOrEmpty(content))
            {
                PrintLog(LogType.Error, "The content of the config is empty");
                return;
            }
            string contentFilePath = $"{options.OutputFileDir}/{config.ClassName}{options.OutputFileExtension}";
            File.WriteAllText(contentFilePath, content);
        }

        private static void PrintLog(LogType logType, string message)
        {
            if (logType == LogType.Info)
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
        }
    }
}
