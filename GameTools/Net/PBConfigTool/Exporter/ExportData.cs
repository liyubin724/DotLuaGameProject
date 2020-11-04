namespace DotTool.NetMessage.Exporter
{
    public enum OutputPlatformType
    {
        All = 0,
        Client,
        Server,
    }

    public enum OutputFormatType
    {
        CSharp = 0,
        Lua,
        Json,
    }

    public class ExportData
    {
        public MessageConfig messageConfig;
        public string idTemplateContent;
        public string parserTemplateContent;
        public string outputDir;
        public OutputPlatformType platformType;
        public OutputFormatType formatType;
    }
}
