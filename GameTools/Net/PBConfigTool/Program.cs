using CommandLine;
using DotTool.PBConfig;
using System.IO;

namespace PBConfigTool
{
    public enum OutputFileType
    {
        Id = 0,
        Parser = 1,
    }

    class Options
    {
        [Option('c',"config-path",Required =true,HelpText ="")]
        public string ConfigFilePath { get; set; }

        [Option('o',"output-dir",Required =true,HelpText ="")]
        public string OutputDir { get; set; }

        [Option('t',"template-path",Required =true,HelpText ="")]
        public string TemplateFilePath { get; set; }

        [Option('f',"file-type",Required =true,HelpText ="")]
        public OutputFileType FileType { get; set; } 

        [Option('p',"platform",Required =false,HelpText ="")]
        public OutputPlatformType Platform { get; set; } = OutputPlatformType.All;
    }

    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions);
        }

        static void RunOptions(Options options)
        {
            if (!File.Exists(options.ConfigFilePath) || !File.Exists(options.TemplateFilePath))
            {
                return;
            }
            if (!Directory.Exists(options.OutputDir))
            {
                Directory.CreateDirectory(options.OutputDir);
            }
            string templateContent = File.ReadAllText(options.TemplateFilePath);
            if (string.IsNullOrEmpty(templateContent))
            {
                return;
            }

            ProtoConfig protoConfig = ProtoConfigUtil.ReadConfig(options.ConfigFilePath);
            if(protoConfig == null)
            {
                return;
            }
            if(options.FileType == OutputFileType.Id)
            {
                if (options.Platform == OutputPlatformType.Client || options.Platform == OutputPlatformType.All)
                {
                    ProtoConfigUtil.CreateProtoID(options.OutputDir, protoConfig.SpaceName, protoConfig.C2SGroup, templateContent);
                }
                if (options.Platform == OutputPlatformType.Server || options.Platform == OutputPlatformType.All)
                {
                    ProtoConfigUtil.CreateProtoID(options.OutputDir, protoConfig.SpaceName, protoConfig.S2CGroup, templateContent);
                }
            }else if(options.FileType == OutputFileType.Parser)
            {
                ProtoConfigUtil.CreateParser(options.OutputDir, options.Platform, protoConfig, templateContent);

                if (options.Platform == OutputPlatformType.Client || options.Platform == OutputPlatformType.All)
                {
                    ProtoConfigUtil.CreateParser(options.OutputDir, OutputPlatformType.Client, protoConfig, templateContent);
                }
                if (options.Platform == OutputPlatformType.Server || options.Platform == OutputPlatformType.All)
                {
                    ProtoConfigUtil.CreateParser(options.OutputDir, OutputPlatformType.Server, protoConfig, templateContent);
                }
            }

            
        }
    }
}
