using DotEngine.Config.Ndb;
using DotTool.ETD.Data;
using DotTool.ETD.Fields;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotTool.ETD.IO.Ndb
{
    public static class NdbWriter
    {

        public static void WriteTo(Sheet sheet,string targetDir)
        {
            string filePath = $"{targetDir}/{sheet.Name}.ndb";

            byte[] fieldBytes = GetFieldBytes(sheet.Fields,out int dataSize);
            byte[] strBytes = GetStringBytes(sheet, out Dictionary<string, int> strOffsetDic);
            byte[] arrayBytes = GetArrayBytes(sheet, strOffsetDic, out Dictionary<string, int> arrayOffsetDic);
            byte[] dataBytes = GetDataBytes(sheet, strOffsetDic, arrayOffsetDic);

            NDBHeader header = new NDBHeader();
            header.version = 1;
            header.dataCount = sheet.LineCount;
            header.fieldCount = sheet.FieldCount;
            header.dataSize = dataSize;
            header.dataOffset = NDBHeader.Size + fieldBytes.Length;
            header.stringOffset = header.dataOffset + dataBytes.Length;
            header.arrayOffset = header.stringOffset + strBytes.Length;

            byte[] headerBytes = NDBConst.StructToByte(header, NDBHeader.Size);

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.Write(headerBytes, 0, headerBytes.Length);
                fs.Write(fieldBytes, 0, fieldBytes.Length);
                fs.Write(dataBytes, 0, dataBytes.Length);
                fs.Write(strBytes, 0, strBytes.Length);
                fs.Write(arrayBytes, 0, arrayBytes.Length);

                fs.Flush();
            }
        }

        private static byte[] GetFieldBytes(List<Field> fields,out int dataSize)
        {
            dataSize = 0;

            MemoryStream stream = new MemoryStream();
            foreach(var field in fields)
            {
                NDBFieldType ndbFieldType = GetNDBFieldType(field);
                dataSize += NDBConst.GetFieldSize(ndbFieldType);
                string fieldNameValue = field.Name;

                byte[] fieldTypeBytes = BitConverter.GetBytes((int)ndbFieldType);
                byte[] fieldNameBytes = Encoding.UTF8.GetBytes(fieldNameValue);
                byte[] fieldNameLenBytes = BitConverter.GetBytes(fieldNameBytes.Length);

                stream.Write(fieldTypeBytes, 0, fieldTypeBytes.Length);
                stream.Write(fieldNameLenBytes, 0, fieldNameLenBytes.Length);
                stream.Write(fieldNameBytes, 0, fieldNameBytes.Length);
            }
            return stream.ToArray();
        }

        private static byte[] GetDataBytes(Sheet sheet, Dictionary<string, int> strOffsetDic, Dictionary<string, int> arrayOffsetDic)
        {
            MemoryStream stream = new MemoryStream();

            for(int i = 0;i<sheet.LineCount;++i)
            {
                Line line = sheet.GetLineByIndex(i);
                for (int j = 0; j < sheet.FieldCount; ++j)
                {
                    Field field = sheet.GetFieldByIndex(j);
                    Cell cell = line.GetCellByIndex(j);
                    Type fieldRealyType = FieldTypeUtil.GetRealyType(field.FieldType);

                    object value = field.GetValue(cell);
                    if(fieldRealyType == typeof(int))
                    {
                        byte[] bytes = BitConverter.GetBytes((int)value);
                        stream.Write(bytes, 0, bytes.Length);
                    }else if(fieldRealyType == typeof(float))
                    {
                        byte[] bytes = BitConverter.GetBytes((float)value);
                        stream.Write(bytes, 0, bytes.Length);
                    }else if(fieldRealyType == typeof(long))
                    {
                        byte[] bytes = BitConverter.GetBytes((long)value);
                        stream.Write(bytes, 0, bytes.Length);
                    }else if(fieldRealyType == typeof(bool))
                    {
                        byte[] bytes = BitConverter.GetBytes((bool)value);
                        stream.Write(bytes, 0, bytes.Length);
                    }else if(fieldRealyType == typeof(string))
                    {
                        string strValue = (string)value;
                        if(!strOffsetDic.TryGetValue(strValue,out int strIndex))
                        {
                            strIndex = -1;
                        }

                        byte[] bytes = BitConverter.GetBytes(strIndex);
                        stream.Write(bytes, 0, bytes.Length);
                    }else if(fieldRealyType == typeof(IList))
                    {
                        string strValue = (string)cell.GetContent(field);
                        if (!arrayOffsetDic.TryGetValue(strValue, out int arrayIndex))
                        {
                            arrayIndex = -1;
                        }

                        byte[] bytes = BitConverter.GetBytes(arrayIndex);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
            }

            return stream.ToArray();
        }

        private static byte[] GetStringBytes(Sheet sheet,out Dictionary<string,int> strOffsetDic)
        {
            strOffsetDic = new Dictionary<string, int>();

            MemoryStream stream = new MemoryStream();
            for(int i =0;i<sheet.LineCount;++i)
            {
                Line line = sheet.GetLineByIndex(i);
                for(int j = 0;j<sheet.FieldCount;++j)
                {
                    Field field = sheet.GetFieldByIndex(j);
                    Type fieldRealyType = FieldTypeUtil.GetRealyType(field.FieldType);
                    if (fieldRealyType == typeof(string))
                    {
                        string value = (string)field.GetValue(line.GetCellByIndex(j));
                        if(!string.IsNullOrEmpty(value) && !strOffsetDic.ContainsKey(value))
                        {
                            strOffsetDic.Add(value, (int)stream.Length);

                            byte[] strBytes = Encoding.UTF8.GetBytes(value);
                            byte[] strLenBytes = BitConverter.GetBytes(strBytes.Length);

                            stream.Write(strLenBytes, 0, strLenBytes.Length);
                            stream.Write(strBytes, 0, strBytes.Length);
                        }
                    }else if(fieldRealyType == typeof(IList))
                    {
                        ListField listField = (ListField)field;
                        Type valueRealyType = FieldTypeUtil.GetRealyType(listField.ValueType);
                        if (valueRealyType == typeof(string))
                        {
                            string[] values = (string[])field.GetValue(line.GetCellByIndex(j));
                            if(values!=null && values.Length>0)
                            {
                                foreach(var value in values)
                                {
                                    if (!string.IsNullOrEmpty(value) && !strOffsetDic.ContainsKey(value))
                                    {
                                        strOffsetDic.Add(value, (int)stream.Length);

                                        byte[] strBytes = Encoding.UTF8.GetBytes(value);
                                        byte[] strLenBytes = BitConverter.GetBytes(strBytes.Length);

                                        stream.Write(strLenBytes, 0, strLenBytes.Length);
                                        stream.Write(strBytes, 0, strBytes.Length);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return stream.ToArray();
        }

        private static byte[] GetArrayBytes(Sheet sheet, Dictionary<string,int> strOffsetDic, out Dictionary<string,int> arrayOffsetDic)
        {
            arrayOffsetDic = new Dictionary<string, int>();

            MemoryStream stream = new MemoryStream();

            for (int i = 0; i < sheet.LineCount; ++i)
            {
                Line line = sheet.GetLineByIndex(i);
                for (int j = 0; j < sheet.FieldCount; ++j)
                {
                    Field field = sheet.GetFieldByIndex(j);
                    Type fieldRealyType = FieldTypeUtil.GetRealyType(field.FieldType);
                    if (fieldRealyType == typeof(IList))
                    {
                        Cell cell = line.GetCellByIndex(j);
                        string content = cell.GetContent(field);
                        Array values = (Array)field.GetValue(cell);
                        if(!string.IsNullOrEmpty(content) && !arrayOffsetDic.ContainsKey(content) && values!=null && values.Length>0)
                        {
                            arrayOffsetDic.Add(content, (int)stream.Length);

                            byte[] valueLenBytes = BitConverter.GetBytes(values.Length);
                            stream.Write(valueLenBytes,0,valueLenBytes.Length);

                            ListField listField = (ListField)field;
                            FieldType valueType = listField.ValueType;
                            Type valueRealyType = FieldTypeUtil.GetRealyType(listField.ValueType);
                            if (valueRealyType == typeof(int))
                            {
                                int[] intValues = (int[])values;
                                foreach(var v in intValues)
                                {
                                    byte[] vBytes = BitConverter.GetBytes(v);
                                    stream.Write(vBytes, 0, vBytes.Length);
                                }    
                            }
                            else if (valueRealyType == typeof(float))
                            {
                                float[] floatValues = (float[])values;
                                foreach (var v in floatValues)
                                {
                                    byte[] vBytes = BitConverter.GetBytes(v);
                                    stream.Write(vBytes, 0, vBytes.Length);
                                }
                            }
                            else if (valueRealyType == typeof(long))
                            {
                                long[] longValues = (long[])values;
                                foreach (var v in longValues)
                                {
                                    byte[] vBytes = BitConverter.GetBytes(v);
                                    stream.Write(vBytes, 0, vBytes.Length);
                                }
                            }
                            else if (valueRealyType == typeof(bool))
                            {
                                bool[] boolValues = (bool[])values;
                                foreach (var v in boolValues)
                                {
                                    byte[] vBytes = BitConverter.GetBytes(v);
                                    stream.Write(vBytes, 0, vBytes.Length);
                                }
                            }
                            else if (valueRealyType == typeof(string))
                            {
                                string[] strValues = (string[])values;
                                foreach (var v in strValues)
                                {
                                    int offset = strOffsetDic[v];

                                    byte[] vBytes = BitConverter.GetBytes(offset);
                                    stream.Write(vBytes, 0, vBytes.Length);
                                }
                            }
                        }
                    }
                }
            }

            return stream.ToArray();
        }

        private static NDBFieldType GetNDBFieldType(Field field)
        {
            FieldType fieldType = field.FieldType;
            if (fieldType == FieldType.List)
            {
                ListField listField = (ListField)field;
                FieldType valueType = listField.ValueType;
                NDBFieldType ndbFieldType = GetNDBFieldType(valueType);
                return (NDBFieldType)(ndbFieldType + 100);
            }
            else
            {
                return GetNDBFieldType(fieldType);
            }
        }

        private static NDBFieldType GetNDBFieldType(FieldType fieldType)
        {
            Type fieldRealyType = FieldTypeUtil.GetRealyType(fieldType);
            if (fieldRealyType == typeof(int))
            {
                return NDBFieldType.Int;
            }
            else if (fieldRealyType == typeof(float))
            {
                return NDBFieldType.Float;
            }
            else if (fieldRealyType == typeof(long))
            {
                return NDBFieldType.Long;
            }
            else if (fieldRealyType == typeof(bool))
            {
                return NDBFieldType.Bool;
            }
            else if (fieldRealyType == typeof(string))
            {
                return NDBFieldType.String;
            }
            return NDBFieldType.Null;
        }



    }
}
