using DotEngine.Context;
using DotEngine.Log;
using DotTool.ScriptGenerate;
using System.IO;

namespace DotTool.NetMessage.Exporter
{
    public class ConfigExporter
    {
        private const string CONTEXT_SPACE_KEY = "spaceName";
        private const string CONTEXT_PLATFORM_KEY = "platform";
        private const string CONTEXT_MESSAGE_GROUP_KEY = "messageGroup";
        private const string CONTEXT_ENCODE_MESSAGE_GROUP_KEY = "encodeMessageGroup";
        private const string CONTEXT_DECODE_MESSAGE_GROUP_KEY = "decodeMessageGroup";

        protected ExportData exportData;
        public ConfigExporter(ExportData data)
        {
            exportData = data;
        }

        public void Export()
        {
            string outputDir = CreateOutputDir();
            ExportMessageID(outputDir);
            ExportMessageParser(outputDir);
        }

        public void ExportMessageID()
        {
            string outputDir = CreateOutputDir();
            ExportMessageID(outputDir);
        }

        public void ExportMessageParser()
        {
            string outputDir = CreateOutputDir();
            ExportMessageParser(outputDir);
        }

        private string CreateOutputDir()
        {
            LogUtil.LogInfo("Exporter", $"Create outputDir({exportData.outputDir})");

            if (!Directory.Exists(exportData.outputDir))
            {
                DirectoryInfo outputDirInfo = Directory.CreateDirectory(exportData.outputDir);
                if (outputDirInfo == null || !outputDirInfo.Exists)
                {
                    LogUtil.LogError("Exporter", $"Create outputDir Failed.({exportData.outputDir})");
                    return string.Empty;
                }
            }
            return exportData.outputDir;
        }

        private void ExportMessageID(string outputDir)
        {
            StringContext context = new StringContext();
            context.Add(CONTEXT_SPACE_KEY, exportData.messageConfig.Space);

            ExportMessageID(outputDir, exportData.messageConfig.C2SGroup, context);
            ExportMessageID(outputDir, exportData.messageConfig.S2CGroup, context);

            context.Clear();
        }

        private void ExportMessageID(string outputDir, MessageGroup group, StringContext context)
        {
            context.Add(CONTEXT_MESSAGE_GROUP_KEY, group);

            string outputFilePath = $"{outputDir}/{group.Name}_ID{GetExtension()}";
            string outputContent = TemplateEngine.Execute(context, exportData.idTemplateContent, new string[]{
                typeof(MessageGroup).Assembly.Location
            });
            if (!string.IsNullOrEmpty(outputContent))
            {
                File.WriteAllText(outputFilePath, outputContent);
            }

            context.Remove(CONTEXT_MESSAGE_GROUP_KEY);
        }

        private void ExportMessageParser(string outputDir)
        {
            StringContext context = new StringContext();
            context.Add(CONTEXT_SPACE_KEY, exportData.messageConfig.Space);

            if (exportData.platformType == OutputPlatformType.All || exportData.platformType == OutputPlatformType.Client)
            {
                context.Add(CONTEXT_PLATFORM_KEY, "Client");
                ExportMessageParser(outputDir, exportData.messageConfig.C2SGroup, exportData.messageConfig.S2CGroup, context);
                context.Remove(CONTEXT_PLATFORM_KEY);
            }

            if (exportData.platformType == OutputPlatformType.All || exportData.platformType == OutputPlatformType.Server)
            {
                context.Add(CONTEXT_PLATFORM_KEY, "Server");
                ExportMessageParser(outputDir, exportData.messageConfig.S2CGroup, exportData.messageConfig.C2SGroup, context);
                context.Remove(CONTEXT_PLATFORM_KEY);
            }

            context.Clear();
        }

        private void ExportMessageParser(string outputDir, MessageGroup encodeGroup, MessageGroup decodeGroup, StringContext context)
        {
            context.Add(CONTEXT_ENCODE_MESSAGE_GROUP_KEY, encodeGroup);
            context.Add(CONTEXT_DECODE_MESSAGE_GROUP_KEY, decodeGroup);

            string outputFilePath = $"{outputDir}/{context.Get<string>(CONTEXT_PLATFORM_KEY)}MessageParser{GetExtension()}";
            string outputContent = TemplateEngine.Execute(context, exportData.parserTemplateContent, new string[]{
                typeof(MessageConfig).Assembly.Location
            });
            if (!string.IsNullOrEmpty(outputContent))
            {
                File.WriteAllText(outputFilePath, outputContent);
            }

            context.Remove(CONTEXT_ENCODE_MESSAGE_GROUP_KEY);
            context.Remove(CONTEXT_DECODE_MESSAGE_GROUP_KEY);
        }

        private string GetExtension()
        {
            if (exportData.formatType == OutputFormatType.CSharp)
            {
                return ".cs";
            }
            else if (exportData.formatType == OutputFormatType.Json)
            {
                return ".json";
            }
            else if (exportData.formatType == OutputFormatType.Lua)
            {
                return ".txt";
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
