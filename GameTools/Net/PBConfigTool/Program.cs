using CommandLine;
using DotEngine.ConsoleLog;
using DotEngine.Log;
using DotTool.NetMessage.Exporter;
using System.IO;

namespace DotTool.NetMessage
{
    class Program
    {
        public class Options
        {
            [Option('c', "config-path", Required = true, HelpText = "")]
            public string ConfigPath { get; set; }

            [Option('f', "format-type", Required = true, HelpText = "")]
            public OutputFormatType Format { get; set; }

            [Option('p', "platform", Required = true, HelpText = "")]
            public OutputPlatformType Platform { get; set; } = OutputPlatformType.All;
        }

        static void Main(string[] args)
        {
            LogUtil.SetLogger(new ConsoleLogger());

            Parser.Default.ParseArguments<Options>(args).WithParsed((options) =>
            {
                ToolConfig toolConfig = XMLConfigUtil.ReadConfig<ToolConfig>(options.ConfigPath);
                if (!File.Exists(toolConfig.messageConfigPath))
                {
                    LogUtil.LogError("ToolConfig", "the messageConfigPath is not found");
                    return;
                }

                TemplateConfig templateConfig = toolConfig.GetTemplate(options.Format);
                if(templateConfig == null 
                || string.IsNullOrEmpty(templateConfig.idTemplateFilePath) 
                || string.IsNullOrEmpty(templateConfig.parserTemplateFilePath))
                {
                    LogUtil.LogError("templateConfig", "the template is not found");
                    return;
                }

                if(!Directory.Exists(toolConfig.outputDir))
                {
                    Directory.CreateDirectory(toolConfig.outputDir);
                }

                ExportData exportData = new ExportData
                {
                    messageConfig = XMLConfigUtil.ReadConfig<MessageConfig>(toolConfig.messageConfigPath, false),
                    formatType = options.Format,
                    platformType = options.Platform,
                    outputDir = toolConfig.outputDir,
                    idTemplateContent = File.ReadAllText(templateConfig.idTemplateFilePath),
                    parserTemplateContent = File.ReadAllText(templateConfig.parserTemplateFilePath)
                };

                new ConfigExporter(exportData).Export();
            });
        }
    }
}
