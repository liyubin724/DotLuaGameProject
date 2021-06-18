using DotEngine.Config.WDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.Config.WDB
{
    public static class WDBToLuaWriter
    {
        public static void WriteToLuaFile(string outputDirPath,WDBSheet sheet)
        {
            string luaContent = WriteToLua(sheet);
            if (!string.IsNullOrEmpty(luaContent))
            {
                string filePath = $"{outputDirPath}/{sheet.Name}.txt";
                File.WriteAllText(filePath, luaContent);
            }
        }

        public static string WriteToLua(WDBSheet sheet)
        { 

        }
    }
}
