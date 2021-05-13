using CommandLine;
using DotEngine.Context;
using DotTool.ETD.Data;
using DotTool.ETD.IO;
using DotTool.ETD.IO.Json;
using DotTool.ETD.IO.Lua;
using DotTool.ETD.IO.Ndb;
using DotTool.ETD.Log;
using System;
using System.Drawing;
using System.IO;

namespace ExcelToDataTool
{
    public enum OutputFormat
    {
        Ndb,
        Json,
        Lua,
    }

    class Options
    {
        [Option('i',"input",Required =true,HelpText ="Input Excel Dir")]
        public string InputDir { get; set; }
        [Option('o',"output",Required =true,HelpText ="Output Dir")]
        public string OutputDir { get; set; }
        [Option('f',"format",Required =true,HelpText ="Format")]
        public OutputFormat Format { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //CommandLine.Parser.Default.ParseArguments<Options>(args)
            //    .WithParsed(RunOptions);

            Console.ReadKey();
        }
        static void RunOptions(Options options)
        {
            if(!Directory.Exists(options.InputDir))
            {
                return;
            }

            LogHandler logHandler = new LogHandler((type, id, msg) =>
            {
                string msgType = "Info";
                Color color = Color.White;

                if (type == LogType.Error)
                {
                    msgType = "Error";
                    color = Color.Red;
                }
                else if (type == LogType.Warning)
                {
                    msgType = "Warning";
                    color = Color.Yellow;
                }

                Colorful.Console.WriteLine($"[{msgType}]    [{id}]  {msg}", color);
            });

            WorkbookReader reader = new WorkbookReader(logHandler);

            TypeContext context = new TypeContext();
            context.Add(typeof(LogHandler), logHandler);

            Workbook[] workbooks = reader.ReadExcelFromDir(options.InputDir);
            if(workbooks !=null && workbooks.Length>0)
            {
                foreach(var workbook in workbooks)
                {
                    string dir = $"{options.OutputDir}/{workbook.Name}";
                    if(!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    bool result = workbook.Verify(context);
                    if(result)
                    {
                        for (int i = 0; i < workbook.SheetCount; ++i)
                        {
                            Sheet sheet = workbook.GetSheeetByIndex(i);
                            if(options.Format == OutputFormat.Json)
                            {
                                JsonWriter.WriteTo(sheet, dir);
                            }else if(options.Format == OutputFormat.Ndb)
                            {
                                NdbWriter.WriteTo(sheet, dir);
                            }else if(options.Format == OutputFormat.Lua)
                            {
                                LuaWriter.WriteTo(sheet, dir);
                            }
                        }
                    }
                }
            }
            context.Clear();
        }
    }
}
