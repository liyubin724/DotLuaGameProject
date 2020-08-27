using DotTool.NetMessage.Exporter;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DotTool.NetMessage
{
    [Serializable]
    public class TemplateConfig
    {
        [XmlAttribute("format")]
        public OutputFormatType formatType;
        
        [XmlAttribute("template_id")]
        public string idTemplateFilePath;
        
        [XmlAttribute("template_parser")]
        public string parserTemplateFilePath;
    }

    [Serializable]
    [XmlRoot("configs")]
    public class ToolConfig
    {
        [XmlAttribute("message_config")]
        public string messageConfigPath;

        [XmlAttribute("output")]
        public string outputDir;

        [XmlElement("template")]
        public List<TemplateConfig> templateConfigs = new List<TemplateConfig>();

        public TemplateConfig GetTemplate(OutputFormatType format)
        {
            foreach(var config in templateConfigs)
            {
                if(config.formatType == format)
                {
                    return config;
                }
            }
            return null;
        }

        public bool HasTemplate(OutputFormatType format)
        {
            foreach (var config in templateConfigs)
            {
                if (config.formatType == format)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
