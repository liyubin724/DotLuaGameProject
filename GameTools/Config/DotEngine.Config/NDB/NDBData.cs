using DotEngine.Config.IO;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DotEngine.Config.NDB
{
    public class NDBData
    {
        public string Name { get; private set; }

        private byte[] datas = null;

        private NDBHeader header = new NDBHeader();
        private NDBField[] fields = null;

        private NDBData textData = null;

        private Dictionary<int, int> dataIDToIndexDic = new Dictionary<int, int>();
        private Dictionary<string, int> fieldNameToIndexDic = new Dictionary<string, int>();

        public int FieldCount => header.fieldCount;
        public int LineCount => header.lineCount;

        public NDBData(string name)
        {
            Name = name;
        }

        public void SetText(NDBData text)
        {
            textData = text;
        }

        public void SetData(byte[] dataBytes)
        {
            SetDataInternal(dataBytes);
        }

        private void SetDataInternal(byte[] dataBytes)
        {
            datas = dataBytes;

            int headerSize = Marshal.SizeOf(typeof(NDBHeader));
            if (dataBytes == null || dataBytes.Length < headerSize)
            {
                throw new ArgumentNullException($"The databytes is empty. Name = {Name}");
            }
            header = MarshalUtility.ByteToStruct<NDBHeader>(dataBytes, 0, headerSize);
            
            fields = new NDBField[header.fieldCount];
            int fieldOffset = 0;
            int byteOffset = 0;
            for (int i = 0; i < header.fieldCount; ++i)
            {
                fields[i] = NDBByteUtility.ReadField(dataBytes, header.fieldOffset + byteOffset, out int offset);
                fieldOffset += NDBFieldUtil.GetFieldOffset(fields[i].Type);
                byteOffset += offset;
            }

            for (int i = 0; i < header.lineCount; ++i)
            {
                int id = ByteReader.ReadInt(dataBytes, header.lineOffset + header.lineSize * i,out int offset);

                dataIDToIndexDic.Add(id, i);
            }
        }

        public T GetDataByID<T>(int id, string fieldName)
        {
            if(dataIDToIndexDic.TryGetValue(id,out var dataIndex) 
                && fieldNameToIndexDic.TryGetValue(fieldName,out var fieldIndex) 
                && fieldIndex>=0&&fieldIndex<fields.Length)
            {
                object result = GetDataByIndexInternal(dataIndex, fields[fieldIndex]);
                if(result!=null)
                {
                    return (T)result;
                }    
            }
            return default;
        }

        public T GetDataByID<T>(int id, int fieldIndex)
        {
            if (dataIDToIndexDic.TryGetValue(id, out var dataIndex)
                && fieldIndex >= 0 && fieldIndex < fields.Length)
            {
                object result = GetDataByIndexInternal(dataIndex, fields[fieldIndex]);
                if (result != null)
                {
                    return (T)result;
                }
            }
            return default;
        }

        public T GetDataByIndex<T>(int index, string fieldName)
        {
            if (index>=0 && index <header.lineCount
                && fieldNameToIndexDic.TryGetValue(fieldName, out var fieldIndex)
                && fieldIndex >= 0 && fieldIndex < fields.Length)
            {
                object result = GetDataByIndexInternal(index, fields[fieldIndex]);
                if (result != null)
                {
                    return (T)result;
                }
            }
            return default;
        }

        public T GetDataByIndex<T>(int index, int fieldIndex)
        {
            if (index >= 0 && index < header.lineCount
                && fieldIndex >= 0 && fieldIndex < fields.Length)
            {
                object result = GetDataByIndexInternal(index, fields[fieldIndex]);
                if (result != null)
                {
                    return (T)result;
                }
            }
            return default;
        }

        private object GetDataByIndexInternal(int index, NDBField field)
        {
            int byteStartIndex = header.lineOffset + header.lineSize * index + field.Offset;
            if (field.Type == NDBFieldType.Bool)
            {
                return ByteReader.ReadBool(datas, byteStartIndex, out _);
            }
            else if (field.Type == NDBFieldType.Int)
            {
                return ByteReader.ReadInt(datas, byteStartIndex, out _);
            }
            else if (field.Type == NDBFieldType.Long)
            {
                return ByteReader.ReadLong(datas, byteStartIndex, out _);
            }
            else if (field.Type == NDBFieldType.Float)
            {
                return ByteReader.ReadFloat(datas, byteStartIndex, out _);
            }
            else if (field.Type == NDBFieldType.String)
            {
                int valueOffset = ByteReader.ReadInt(datas, byteStartIndex, out _);
                return ByteReader.ReadString(datas, header.stringOffset + valueOffset, out _);
            }
            else if (field.Type == NDBFieldType.Text)
            {
                int textID = ByteReader.ReadInt(datas, byteStartIndex, out _);
                if (textData != null)
                {
                    return textData.GetDataByID<string>(textID, 1);
                }
                else
                {
                    return textID.ToString();
                }
            }
            else
            {
                return null;
            }
        }
    }
}
