using DotEngine.Config.WDB;
using DotEngine.Context;
using DotTool.ScriptGenerate;
using System;
using System.IO;

namespace DotEditor.Config.WDB
{
    public static class WDBToLuaWriter
    {
        public static void WriteToLuaFile(WDBSheet sheet, string filePath, string templateContent, EntryConfig config = null)
        {
            string content = WriteToLua(sheet, templateContent,config);
            if (string.IsNullOrEmpty(content))
            {
                throw new Exception("");
            }

            File.WriteAllText(filePath, content);
        }

        public static string WriteToLua(WDBSheet sheet, string templateContent, EntryConfig config = null)
        {
            string[] assemblyNames = new string[] { typeof(WDBSheet).Assembly.Location };
            return WriteToLua(sheet, templateContent, assemblyNames, config??EntryConfig.Default);
        }

        private static string WriteToLua(WDBSheet sheet, string templateContent, string[] assemblyNames, EntryConfig config)
        {
            StringContextContainer context = new StringContextContainer();
            context.Add("__sheet__", sheet);
            string content = TemplateEngine.Generate(context, templateContent, assemblyNames, config);
            context.Remove("__sheet__");
            return content;
        }
    }
}
