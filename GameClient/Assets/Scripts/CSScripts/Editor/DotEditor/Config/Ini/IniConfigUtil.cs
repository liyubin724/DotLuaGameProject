using DotEngine.Config.Ini;
using ReflectionMagic;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotEditor.Config.Ini
{
    public static class IniConfigUtil
    {
        public static readonly string INI_HEAD_CONTENT = "**INI-FILE**";

        public static IniConfig ReadConfigFrom(string filePath)
        {
            if(!File.Exists(filePath))
            {
                return null;
            }
            string text = File.ReadAllText(filePath);
            if(string.IsNullOrEmpty(text))
            {
                return null;
            }

            IniConfig config = new IniConfig();
            config.ParseText(text);

            return config;
        }

        public static void WriteConfigTo(string filePath,IniConfig config)
        {
            StringBuilder configSB = new StringBuilder();
            configSB.AppendLine(INI_HEAD_CONTENT);

            dynamic dynamicConfig = config.AsDynamic();
            Dictionary<string, IniGroup> groupDic = dynamicConfig.groupDic;
            if(groupDic!=null && groupDic.Count>0)
            {
                foreach(var kvp in groupDic)
                {
                    IniGroup group = kvp.Value;
                    configSB.AppendLine($"#{group.Name}|{group.Comment}");

                    dynamic dynamicGroup = group.AsDynamic();
                    Dictionary<string, IniData> dataDic = dynamicGroup.dataDic;
                    foreach(var kvp2 in dataDic)
                    {
                        IniData data = kvp2.Value;
                        configSB.AppendLine($"-{data.Key}|{data.Value}|{data.Comment}|{string.Join(",", data.OptionValues)}");
                    }
                }

                configSB.AppendLine();
            }

            File.WriteAllText(filePath, configSB.ToString());
        }
    }
}
