using DotEngine.Config;
using DotEngine.Config.IO;
using DotEngine.Config.NDB;
using DotEngine.Config.WDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.Config.WDB
{
    public static class WDBToNDBWriter
    {
        public static void WriteToNDBFile(string outputDirPath, WDBSheet sheet)
        {
            byte[] dataBytes = WriteToNDB(sheet);
            if (dataBytes != null && dataBytes.Length > 0)
            {
                string filePath = $"{outputDirPath}/{sheet.Name}{NDBConst.NDB_FILE_EXTERSION}";
                File.WriteAllBytes(filePath, dataBytes);
            }
        }

        public static byte[] WriteToNDB(WDBSheet sheet)
        {
            NDBHeader header = new NDBHeader();
            header.fieldCount = sheet.FieldCount;
            header.lineCount = sheet.LineCount;

            int structSize = MarshalUtility.GetStructSize(typeof(NDBHeader));
            header.fieldOffset = structSize;

            byte[] fieldBytes = GetFieldBytes(sheet, out var lineSize);
            header.lineSize = lineSize;

            header.lineOffset = structSize + fieldBytes.Length;

            byte[] strBytes = GetStringBytes(sheet, out var strOffsetDic);
            byte[] lineBytes = GetLineBytes(sheet, strOffsetDic);

            header.stringOffset = structSize + fieldBytes.Length + lineBytes.Length;

            MemoryStream stream = new MemoryStream();

            byte[] headerBytes = MarshalUtility.StructToByte(header, structSize);
            stream.Write(headerBytes, 0, headerBytes.Length);
            stream.Write(fieldBytes, 0, fieldBytes.Length);
            stream.Write(lineBytes, 0, lineBytes.Length);
            stream.Write(strBytes, 0, strBytes.Length);

            return stream.ToArray(); ;
        }

        private static byte[] GetFieldBytes(WDBSheet sheet,out int lineSize)
        {
            NDBField[] fields = GetFields(sheet);
            lineSize = GetLineSize(fields);
            MemoryStream stream = new MemoryStream();
            foreach(var field in fields)
            {
                NDBByteUtility.WriteField(stream, field);
            }
            return stream.ToArray();
        }

        private static byte[] GetLineBytes(WDBSheet sheet,Dictionary<string,int> strOffsetDic)
        {
            MemoryStream stream = new MemoryStream();
            for (int i = 0; i < sheet.LineCount; ++i)
            {
                WDBLine line = sheet.GetLineAtIndex(i);
                for (int j = 0; j < sheet.FieldCount; ++j)
                {
                    WDBField field = sheet.GetFieldAtIndex(j);
                    WDBCell cell = line.GetCellByCol(field.Col);
                    string cellValue = cell.GetValue(field);

                    if (field.FieldType == WDBFieldType.String || field.FieldType == WDBFieldType.Address
                        || field.FieldType == WDBFieldType.Lua)
                    {
                        string content = cellValue ?? "";
                        if (!strOffsetDic.TryGetValue(content, out var offset))
                        {
                            throw new Exception();
                        }
                        ByteWriter.WriteInt(stream, offset);
                    }
                    else if (field.FieldType == WDBFieldType.Bool)
                    {
                        if (string.IsNullOrEmpty(cellValue) || !bool.TryParse(cellValue, out var result))
                        {
                            ByteWriter.WriteBool(stream, false);
                        }
                        else
                        {
                            ByteWriter.WriteBool(stream, result);
                        }
                    }
                    else if (field.FieldType == WDBFieldType.Int || field.FieldType == WDBFieldType.Text)
                    {
                        if (string.IsNullOrEmpty(cellValue) || !int.TryParse(cellValue, out var result))
                        {
                            ByteWriter.WriteInt(stream, 0);
                        }
                        else
                        {
                            ByteWriter.WriteInt(stream, result);
                        }
                    }
                    else if (field.FieldType == WDBFieldType.Id || field.FieldType == WDBFieldType.Ref)
                    {
                        if (string.IsNullOrEmpty(cellValue) || !int.TryParse(cellValue, out var result))
                        {
                            ByteWriter.WriteInt(stream, -1);
                        }
                        else
                        {
                            ByteWriter.WriteInt(stream, result);
                        }
                    }
                    else if (field.FieldType == WDBFieldType.Long)
                    {
                        if (string.IsNullOrEmpty(cellValue) || !long.TryParse(cellValue, out var result))
                        {
                            ByteWriter.WriteLong(stream, 0);
                        }
                        else
                        {
                            ByteWriter.WriteLong(stream, result);
                        }
                    }
                    else if (field.FieldType == WDBFieldType.Float)
                    {
                        if (string.IsNullOrEmpty(cellValue) || !float.TryParse(cellValue, out var result))
                        {
                            ByteWriter.WriteFloat(stream, 0);
                        }
                        else
                        {
                            ByteWriter.WriteFloat(stream, result);
                        }

                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            return stream.ToArray();
        }

        private static byte[] GetStringBytes(WDBSheet sheet,out Dictionary<string,int> strOffsetDic)
        {
            strOffsetDic = new Dictionary<string, int>();
            strOffsetDic.Add("", 0);
            MemoryStream stream = new MemoryStream();
            ByteWriter.WriteString(stream, "");
            for(int i =0;i<sheet.LineCount;++i)
            {
                WDBLine line = sheet.GetLineAtIndex(i);
                for(int j =0;j<sheet.FieldCount;++j)
                {
                    WDBField field = sheet.GetFieldAtIndex(j);
                    if(field.FieldType == WDBFieldType.String || field.FieldType == WDBFieldType.Address
                        || field.FieldType == WDBFieldType.Lua)
                    {
                        WDBCell cell = line.GetCellByCol(field.Col);
                        string cellValue = cell.GetValue(field);

                        string content = cellValue ?? "";
                        if(strOffsetDic.ContainsKey(content))
                        {
                            continue;
                        }
                        strOffsetDic.Add(content, (int)stream.Length);
                        if(field.FieldType == WDBFieldType.Lua)
                        {
                            ByteWriter.WriteString(stream, $"function () \n {content} \n end");
                        }else
                        {
                            ByteWriter.WriteString(stream, content);
                        }
                    }
                }
            }
            return stream.ToArray();
        }

        private static int GetLineSize(NDBField[] fields)
        {
            if(fields == null || fields.Length==0)
            {
                return 0;
            }
            int lineSize = 0;
            foreach(var field in fields)
            {
                lineSize += NDBFieldUtil.GetFieldOffset(field.FieldType);
            }
            return lineSize;
        }

        private static NDBField[] GetFields(WDBSheet sheet)
        {
            List<NDBField> fields = new List<NDBField>();

            for(int i =0;i<sheet.FieldCount;++i)
            {
                WDBField wdbField = sheet.GetFieldAtIndex(i);

                NDBField ndbField = new NDBField()
                {
                    FieldType = GetFieldType(wdbField.FieldType),
                    Name = wdbField.Name,
                };
                fields.Add(ndbField);
            }

            return fields.ToArray();
        }

        private static NDBFieldType GetFieldType(WDBFieldType fieldType)
        {
            if(fieldType == WDBFieldType.Bool)
            {
                return NDBFieldType.Bool;
            }else if(fieldType == WDBFieldType.Int || fieldType == WDBFieldType.Id
                || fieldType == WDBFieldType.Ref)
            {
                return NDBFieldType.Int;
            }else if(fieldType == WDBFieldType.Long)
            {
                return NDBFieldType.Long;
            }else if(fieldType == WDBFieldType.Float)
            {
                return NDBFieldType.Float;
            }else if(fieldType == WDBFieldType.String || fieldType == WDBFieldType.Address ||fieldType == WDBFieldType.Lua)
            {
                return NDBFieldType.String;
            }else if(fieldType == WDBFieldType.Text)
            {
                return NDBFieldType.Text;
            }else
            {
                return NDBFieldType.None;
            }
        }
    }
}
