using DotEngine.Context;
using DotTool.ScriptGenerate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DotTool.PBConfig
{
    public enum OutputPlatformType
    {
        Client = 0,
        Server,
        All,
    }

    public static class ProtoConfigUtil
    {
        public static ProtoConfig ReadConfig(string path, bool createIfNot = false)
        {
            ProtoConfig config = null;
            if (File.Exists(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProtoConfig));
                config = xmlSerializer.Deserialize(File.OpenRead(path)) as ProtoConfig;
            }

            if (config == null && createIfNot)
            {
                config = new ProtoConfig();
            }
            return config;
        }

        public static void WriterConfig(string path, ProtoConfig config)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProtoConfig));
            StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
            xmlSerializer.Serialize(writer, config);
        }

        public static void CreateProtoID(string outputDir, string spaceName, ProtoGroup protoGroup, string templateContent)
        {
            StringContext context = new StringContext();
            context.Add("spaceName", spaceName);
            context.Add("protoGroup", protoGroup);

            List<string> assemblies = new List<string>()
            {
                typeof(ProtoConfig).Assembly.Location,
            };

            string outputFilePath = $"{outputDir}/{protoGroup.Name}_IDs.cs";
            string outputContent = TemplateEngine.Execute(context, templateContent, assemblies.ToArray());
            File.WriteAllText(outputFilePath, outputContent);
        }

        public static void CreateParser(string outputDir, OutputPlatformType platformType,ProtoConfig protoConfig, string templateContent)
        {
            StringContext context = new StringContext();
            context.Add("spaceName", protoConfig.SpaceName);
            if(platformType == OutputPlatformType.Client)
            {
                context.Add("platform", OutputPlatformType.Client.ToString());
                context.Add("encodeProtoGroup", protoConfig.C2SGroup);
                context.Add("decodeProtoGroup", protoConfig.S2CGroup);
            }
            else if(platformType == OutputPlatformType.Server)
            {
                context.Add("platform", OutputPlatformType.Server.ToString());
                context.Add("encodeProtoGroup", protoConfig.S2CGroup);
                context.Add("decodeProtoGroup", protoConfig.C2SGroup);
            }
            else
            {
                return;
            }
            List<string> assemblies = new List<string>()
            {
                typeof(ProtoConfig).Assembly.Location,
            };

            string outputFilePath = $"{outputDir}/{platformType.ToString()}MessageParser.cs";
            string outputContent = TemplateEngine.Execute(context, templateContent, assemblies.ToArray());
            File.WriteAllText(outputFilePath, outputContent);
        }
    }
}
