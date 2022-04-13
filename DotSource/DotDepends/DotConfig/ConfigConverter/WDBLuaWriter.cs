using DotEngine.Config.WDB;
using System;
using System.Text;

namespace DotTool.Config
{
    public static class WDBLuaWriter
    {
        public static string WriteToLua(WDBSheet sheet, TargetPlatformType platformType)
        {
            if (sheet.FieldCount == 0 || sheet.RowCount == 0)
            {
                return string.Empty;
            }

            WDBFieldPlatform platform = platformType == TargetPlatformType.Client ? WDBFieldPlatform.Client : WDBFieldPlatform.Server;

            StringBuilder builder = new StringBuilder();
            int indent = 0;
            builder.AppendLine($"local {sheet.Name} = {{");
            for (int r = 0; r < sheet.RowCount; r++)
            {
                WDBRow row = sheet.GetRowAtIndex(r);

                WDBField keyField = sheet.GetFieldAtIndex(0);
                WDBCell keyCell = row.GetCellByIndex(0);
                indent++;
                string keyStr = keyCell.GetContent(keyField);
                builder.AppendLine($"{GetIndent(indent)}[{keyStr}] = {{");

                for (int f = 0; f < sheet.FieldCount; f++)
                {
                    WDBField field = sheet.GetFieldAtIndex(f);
                    if (string.IsNullOrEmpty(field.Name))
                    {
                        continue;
                    }

                    if (field.FieldPlatform != WDBFieldPlatform.All && field.FieldPlatform != platform)
                    {
                        continue;
                    }
                    indent++;
                    WDBCell cell = row.GetCellByIndex(f);
                    AppendValueLine(builder, indent, field, cell);
                    indent--;
                }

                builder.AppendLine($"{GetIndent(indent)}}},");
                indent--;
            }
            builder.AppendLine("}");
            builder.AppendLine($"return {sheet.Name}");

            return builder.ToString();
        }

        private static void AppendValueLine(StringBuilder builder, int indent, WDBField field, WDBCell cell)
        {
            string content = cell.GetContent(field);
            if (field.FieldType == WDBFieldType.Bool)
            {
                if (!bool.TryParse(content, out bool result))
                {
                    result = false;
                }
                builder.AppendLine($"{GetIndent(indent)}{field.Name} = {result.ToString().ToLower()},");
            }
            else if (field.FieldType == WDBFieldType.Float)
            {
                if (float.TryParse(content, out float result))
                {
                    builder.AppendLine($"{GetIndent(indent)}{field.Name} = {content},");
                }
                else
                {
                    builder.AppendLine($"{GetIndent(indent)}{field.Name} = 0,");
                }
            }
            else if (field.FieldType == WDBFieldType.Int || field.FieldType == WDBFieldType.Ref)
            {
                if (int.TryParse(content, out int result))
                {
                    builder.AppendLine($"{GetIndent(indent)}{field.Name} = {content},");
                }
                else
                {
                    builder.AppendLine($"{GetIndent(indent)}{field.Name} = 0,");
                }
            }
            else if (field.FieldType == WDBFieldType.Long)
            {
                if (long.TryParse(content, out long result))
                {
                    builder.AppendLine($"{GetIndent(indent)}{field.Name} = {content},");
                }
                else
                {
                    builder.AppendLine($"{GetIndent(indent)}{field.Name} = 0,");
                }
            }
            else if (field.FieldType == WDBFieldType.String || field.FieldType == WDBFieldType.UAsset)
            {
                string value = content ?? string.Empty;
                builder.AppendLine($"{GetIndent(indent)}{field.Name} = [[{value}]],");
            }else if(field.FieldType == WDBFieldType.DateTime)
            {
                if (!DateTime.TryParse(content,out var result))
                {
                    builder.AppendLine($"{GetIndent(indent)}{field.Name} = 0,");
                }
                else
                {
                    var timeSpan = result - new DateTime(1970, 1, 1, 0, 0, 0);
                    builder.AppendLine($"{GetIndent(indent)}{field.Name} = {(long)timeSpan.TotalMilliseconds},");
                }
            }
        }

        private static string GetIndent(int indent)
        {
            return new string(' ', indent * 4);
        }
    }
}
