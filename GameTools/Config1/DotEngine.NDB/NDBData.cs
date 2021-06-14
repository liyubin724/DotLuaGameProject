using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DotEngine.NDB
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

        public void SetText(byte[] textBytes)
        {
            textData = new NDBData($"{Name}(text)");
            textData.SetData(textBytes);
        }

        public void SetText(NDBData text)
        {
            textData = text;
        }

        public void SetData(byte[] dataBytes, byte[] textBytes)
        {
            SetText(textBytes);
            SetDataInternal(dataBytes);
        }

        public void SetData(byte[] dataBytes)
        {
            SetDataInternal(dataBytes);
        }

        private unsafe void SetDataInternal(byte[] dataBytes)
        {
            datas = dataBytes;

            int headerSize = Marshal.SizeOf(typeof(NDBHeader));
            if (dataBytes == null || dataBytes.Length < headerSize)
            {
                throw new ArgumentNullException($"The databytes is empty. Name = {Name}");
            }
            header = MarshalUtility.ByteToStruct<NDBHeader>(dataBytes, 0, headerSize);
            ReadFields();

        }

        private unsafe void ReadFields()
        {
            fields = new NDBField[header.fieldCount];
            fieldNameToIndexDic.Clear();

            fixed (byte* bytePtr = &datas[header.fieldOffset])
            {
                int fieldOffset = 0;
                int byteOffset = 0;
                for (int i = 0; i < header.fieldCount; ++i)
                {
                    NDBField field = new NDBField();
                    fields[i] = field;

                    NDBFieldType fieldType = (NDBFieldType)(*(bytePtr + byteOffset));
                    byteOffset += sizeof(byte);

                    int fieldNameLen = *((int*)(bytePtr + byteOffset));
                    byteOffset += sizeof(int);

                    string fieldName = Encoding.UTF8.GetString(datas, header.fieldOffset + byteOffset, fieldNameLen);
                    byteOffset += fieldNameLen;

                    field.Type = fieldType;
                    field.Name = fieldName;
                    field.Offset = fieldOffset;

                    fieldOffset += NDBFieldUtil.GetFieldOffset(fieldType);
                }
            }
        }

        private unsafe void ReadLines()
        {
            dataIDToIndexDic.Clear();

            int lineOffset = header.lineOffset;
            int lineSize = header.lineSize;

            fixed (byte* bytePtr = &datas[lineOffset])
            {
                for (int i = 0; i < header.lineCount; ++i)
                {
                    int id = *((int*)(bytePtr + lineSize * i));
                    dataIDToIndexDic.Add(id, i);
                }
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

        private unsafe object GetDataByIndexInternal(int index, NDBField field)
        {
            int byteStartIndex = header.lineOffset + header.lineSize * index + field.Offset;
            fixed (byte* bytePtr = &datas[byteStartIndex])
            {
                if (field.Type == NDBFieldType.Bool)
                {
                    return *((bool*)bytePtr);
                }
                else if (field.Type == NDBFieldType.Int)
                {
                    return *((int*)bytePtr);
                }
                else if (field.Type == NDBFieldType.Long)
                {
                    return *((long*)bytePtr);
                }
                else if (field.Type == NDBFieldType.Float)
                {
                    return *((float*)bytePtr);
                }
                else if (field.Type == NDBFieldType.String)
                {
                    int valueOffset = *((int*)bytePtr);

                    fixed (byte* strBytePtr = &datas[header.stringOffset])
                    {
                        int strLen = *((int*)(strBytePtr + valueOffset));
                        return Encoding.UTF8.GetString(datas, header.stringOffset + valueOffset + sizeof(int), strLen);
                    }
                }
                else if (field.Type == NDBFieldType.Text)
                {
                    int textID = *((int*)bytePtr);
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
}
