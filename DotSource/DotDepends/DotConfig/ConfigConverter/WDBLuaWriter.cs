using DotEngine.Config.WDB;
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
                string keyStr = GetStringValue(keyField, keyCell);
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

        private static string GetStringValue(WDBField field, WDBCell cell)
        {
            string content = cell.GetContent(field);
            if (field.FieldType == WDBFieldType.Bool)
            {
                if (!bool.TryParse(content, out bool result))
                {
                    content = "false";
                }
            }
            else if (field.FieldType == WDBFieldType.Float)
            {
                if (!float.TryParse(content, out float result))
                {
                    content = "0";
                }
            }
            else if (field.FieldType == WDBFieldType.Int || field.FieldType == WDBFieldType.Ref)
            {
                if (!int.TryParse(content, out int result))
                {
                    content = "0";
                }
            }
            else if (field.FieldType == WDBFieldType.Long)
            {
                if (!long.TryParse(content, out long result))
                {
                    content = "0";
                }
            }
            else if (field.FieldType == WDBFieldType.String || field.FieldType == WDBFieldType.UAsset)
            {
                content = content ?? string.Empty;
            }

            return content;
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
            }
        }

        private static string GetIndent(int indent)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < indent * 4; i++)
            {
                builder.Append(" ");
            }
            return builder.ToString();
        }
    }
}
