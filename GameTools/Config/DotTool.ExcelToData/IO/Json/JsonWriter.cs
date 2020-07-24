using DotTool.ETD.Data;
using DotTool.ETD.Fields;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotTool.ETD.IO.Json
{
    public static class JsonWriter
    {
        public static void WriteTo(Sheet sheet, string targetDir)
        {
            string filePath = $"{targetDir}/{sheet.Name}.json";

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
                    Type fieldRealyType = FieldTypeUtil.GetRealyType(field.FieldType);

                    object value = field.GetValue(line.GetCellByIndex(j));
                    if(value!=null)
                    {
                        lineJsonObj.Add(field.Name, JToken.FromObject(value));
                    }
                }
            }

            File.WriteAllText(filePath, jsonObj.ToString());

        }
    }
}
