using DotEngine.Config.WDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace DotTool.Config
{
    public static class WDBJsonWriter
    {
        public static string WriteToJson(WDBSheet sheet, TargetPlatformType platformType)
        {
            if (sheet.FieldCount == 0 || sheet.RowCount == 0)
            {
                return string.Empty;
            }

            WDBFieldPlatform platform = platformType == TargetPlatformType.Client ? WDBFieldPlatform.Client : WDBFieldPlatform.Server;

            JObject sheetObject = new JObject();

            for (int r = 0; r < sheet.RowCount; r++)
            {
                WDBRow row = sheet.GetRowAtIndex(r);

                JObject rowObject = new JObject();

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

                    object value = GetValue(field, row.GetCellByIndex(f));
                    rowObject.Add(field.Name, JToken.FromObject(value));

                    if (f == 0)
                    {
                        sheetObject.Add(value.ToString(), rowObject);
                    }
                }
            }

            return sheetObject.ToString(Formatting.Indented);
        }

        private static object GetValue(WDBField field, WDBCell cell)
        {
            string content = cell.GetContent(field);
            if (field.FieldType == WDBFieldType.Bool)
            {
                if (bool.TryParse(content, out bool result))
                {
                    return result;
                }
                return false;
            }
            else if (field.FieldType == WDBFieldType.Float)
            {
                if (float.TryParse(content, out float result))
                {
                    return result;
                }
                return 0;
            }
            else if (field.FieldType == WDBFieldType.Int || field.FieldType == WDBFieldType.Ref)
            {
                if (int.TryParse(content, out int result))
                {
                    return result;
                }
                return 0;
            }
            else if (field.FieldType == WDBFieldType.Long)
            {
                if (long.TryParse(content, out long result))
                {
                    return result;
                }
                return 0;
            }
            else if (field.FieldType == WDBFieldType.String || field.FieldType == WDBFieldType.UAsset)
            {
                return content ?? string.Empty;
            } else if (field.FieldType == WDBFieldType.DateTime)
            {
                var timeSpan = DateTime.Parse(content) - new DateTime(1970, 1, 1, 0, 0, 0);
                return (long)timeSpan.TotalMilliseconds;
            }
            return content;
        }
    }
}
