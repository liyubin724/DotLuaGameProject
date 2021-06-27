namespace DotTool.ScriptGenerate
{
    public class EntryConfig
    {
        public string ClassName { get; set; } = "TemplateRunner";
        public string MethodName { get; set; } = "Run";
        public string CodeOutputPath { get; set; }

        public static EntryConfig Default = new EntryConfig();
    }
}
