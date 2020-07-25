using DotTool.ETD.Data;
using Json2Lua.Lua;
using System.IO;

namespace DotTool.ETD.IO.Lua
{
    public static class LuaWriter
    {
        public static string WriteTo(Sheet sheet, string targetDir)
        {
            LuaTable luaTable = new LuaTable();
            for (int i = 0; i < sheet.LineCount; ++i)
            {
                Line line = sheet.GetLineByIndex(i);
                int id = int.Parse(sheet.GetLineIDByIndex(i));

                LuaTable lineTable = new LuaTable();
                luaTable.AddItem(id, lineTable);

                for (int j = 0; j < sheet.FieldCount; ++j)
                {
                    Field field = sheet.GetFieldByIndex(j);

                    object value = field.GetValue(line.GetCellByIndex(j));
                    if (value != null)
                    {
                        lineTable.AddItem(field.Name, value);
                    }
                }
            }

            string luaStr = luaTable.GetString(1);
            if (!string.IsNullOrEmpty(targetDir))
            {
                string filePath = $"{targetDir}/{sheet.Name}.txt";
                luaStr = $"local {sheet.Name} = {luaStr}\r\nreturn {sheet.Name}";

                File.WriteAllText(filePath, luaStr);
            }
            return luaStr;
        }
    }
}
