using DotTool.ETD.Data;
using DotTool.ETD.Fields;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace DotTool.ETD.IO.Json
{
    public static class JsonWriter
    {
        public static string WriteTo(Sheet sheet, string targetDir)
        {
            JObject jsonObj = new JObject();
            for (int i = 0; i < sheet.LineCount; ++i)
            {
                Line line = sheet.GetLineByIndex(i);
                int id = int.Parse(sheet.GetLineIDByIndex(i));

                JObject lineJsonObj = new JObject();
                jsonObj.Add(id.ToString(), lineJsonObj);

                for (int j = 0; j < sheet.FieldCount; ++j)
                {
                    Field field = sheet.GetFieldByIndex(j);

                    object value = field.GetValue(line.GetCellByIndex(j));
                    if(value!=null)
                    {
                        lineJsonObj.Add(field.Name, JToken.FromObject(value));
                    }
                }
            }

            string jsonStr = jsonObj.ToString(Formatting.Indented);
            if(!string.IsNullOrEmpty(targetDir))
            {
                string filePath = $"{targetDir}/{sheet.Name}.json";
                File.WriteAllText(filePath, jsonStr);
            }
            return jsonStr;
        }
    }
}
