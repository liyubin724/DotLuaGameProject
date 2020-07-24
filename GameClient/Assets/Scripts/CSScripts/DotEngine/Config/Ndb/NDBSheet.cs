using DotEngine.Utilities;
using System.Collections.Generic;
using System.Text;

//Native Data Buffer
namespace DotEngine.Config.Ndb
{
    public class NDBSheet
    {
        private string name;
        private Dictionary<int, int> dataIdIndexDic = new Dictionary<int, int>();
        private byte[] dataBytes = null;
        private NDBHeader header;
        private List<NDBField> fields = new List<NDBField>();
        private Dictionary<string, int> fieldIndexDic = new Dictionary<string, int>();

        public string Name { get => name; }

        public NDBSheet(string n)
        {
            name = n;
        }

        public unsafe void SetData(byte[] datas)
        {
            if(datas == null || datas.Length<=0)
            {
                return;
            }
            dataBytes = datas;

            header = StructUtility.ByteToStruct<NDBHeader>(dataBytes, 0, NDBHeader.Size);
            
            fixed(byte* b = &dataBytes[NDBHeader.Size])
            {
                int offset = 0;
                int byteOffset = 0;
                for (int i = 0; i < header.fieldCount; ++i)
                {
                    byte* start = b + offset;

                    NDBFieldType fieldType = (NDBFieldType)(*((int*)(start)));
                    int len = *((int*)(start + sizeof(int)));
                    string fName = Encoding.UTF8.GetString((start + sizeof(int) * 2), len);

                    offset += sizeof(int) * 2 + len;

                    fields.Add(new NDBField() { name = fName, type = fieldType, offset = byteOffset });
                    byteOffset += NDBConst.GetFieldSize(fieldType);

                    fieldIndexDic.Add(fName, i);
                }
            }

            fixed(byte *b = &dataBytes[header.dataOffset])
            {
                int offset = 0;
                for(int i = 0;i<header.dataCount;++i)
                {
                    int id = *((int*)(b+offset));
                    dataIdIndexDic.Add(id, i);

                    offset += header.dataSize;
                }
            }
        }

        public int DataCount()
        {
            return header.dataCount;
        }

        public int FieldCount()
        {
            return header.fieldCount;
        }

        public unsafe int GetDataIdByIndex(int index)
        {
            if (index >= 0 && index < header.dataCount)
            {
                int offset = header.dataOffset + index * header.dataSize;
                fixed (byte* b = &dataBytes[offset])
                {
                    return *((int*)b);
                }
            }
            return -1;
        }

        public object GetDataByIndex(int index,string fieldName)
        {
            if (fieldIndexDic.TryGetValue(fieldName, out int fieldIndex))
            {
                return GetDataByIndex(index, fieldIndex);
            }
            return null;
        }

        public T GetDataByIndex<T>(int index,string fieldName)
        {
            return (T)GetDataByIndex(index, fieldName);
        }

        public object GetDataByIndex(int index,int fieldIndex)
        {
            int id = GetDataIdByIndex(index);
            return GetDataById(id,fieldIndex);
        }

        public T GetDataByIndex<T>(int index,int fieldIndex)
        {
            return (T)GetDataByIndex(index, fieldIndex);
        }

        public object GetDataById(int id, string fieldName)
        {
            if(fieldIndexDic.TryGetValue(fieldName,out int index))
            {
                return GetDataById(id,index);
            }
            return null;
        }

        public T GetDataById<T>(int id,string fieldName)
        {
            return (T)GetDataById(id,fieldName);
        }

        public T GetDataById<T>(int id,int fieldIndex)
        {
            return (T)GetDataById(id, fieldIndex);
        }

        public unsafe object GetDataById(int id,int fieldIndex)
        {
            if(fieldIndex>=0&&fieldIndex<fields.Count)
            {
                if(dataIdIndexDic.TryGetValue(id,out int index))
                {
                    NDBField field = fields[fieldIndex];
                    int offset = header.dataOffset + header.dataSize * index + field.offset;
                    if (field.type == NDBFieldType.String)
                    {
                        int strOffset = -1;
                        fixed (byte* b = &dataBytes[offset])
                        {
                            strOffset = *((int*)b);
                        }
                        return GetStringValue(strOffset);
                    }
                    else if (field.type == NDBFieldType.BoolArray || field.type == NDBFieldType.FloatArray
                       || field.type == NDBFieldType.IntArray || field.type == NDBFieldType.LongArray
                       || field.type == NDBFieldType.StringArray)
                    {
                        int arrayOffset = -1;
                        fixed (byte* b = &dataBytes[offset])
                        {
                            arrayOffset = *((int*)b);
                        }
                        return GetArrayValue(field, arrayOffset);
                    }else
                    {
                        fixed (byte* b = &dataBytes[offset])
                        {
                            if (field.type == NDBFieldType.Int)
                            {
                                return *((int*)b);
                            }
                            else if (field.type == NDBFieldType.Float)
                            {
                                return *((float*)b);
                            }
                            else if (field.type == NDBFieldType.Long)
                            {
                                return *((long*)b);
                            }
                            else if (field.type == NDBFieldType.Bool)
                            {
                                return *((bool*)b);
                            }
                        }
                    }
                }
            }

            return null;
        }

        private unsafe string GetStringValue(int offset)
        {
            if(offset<0)
            {
                return null;
            }

            fixed(byte *b = &dataBytes[header.stringOffset+offset])
            {
                int len = *((int*)b);
                return Encoding.UTF8.GetString(b + sizeof(int), len);
            }
        }

        private unsafe object GetArrayValue(NDBField field,int offset)
        {
            if(offset<0)
            {
                return null;
            }

            if (field.type == NDBFieldType.StringArray)
            {
                int len = 0;
                fixed (byte* b = &dataBytes[header.arrayOffset + offset])
                {
                    len = *((int*)b);
                }
                string[] result = new string[len];

                for (int i = 0; i < len; ++i)
                {
                    int strIndex = -1;
                    fixed (byte* b = &dataBytes[header.arrayOffset + offset])
                    {
                        byte* startByte = b + sizeof(int);
                        strIndex = *((int*)(startByte + sizeof(int) * i));
                    }

                    result[i] = GetStringValue(strIndex);
                }
                return result;
            }else
            {
                fixed (byte* b = &dataBytes[header.arrayOffset + offset])
                {
                    int len = *((int*)b);
                    byte* startByte = b + sizeof(int);
                    if (field.type == NDBFieldType.BoolArray)
                    {
                        bool[] result = new bool[len];
                        for (int i = 0; i < len; ++i)
                        {
                            result[i] = *((bool*)(startByte + sizeof(bool) * i));
                        }
                        return result;
                    }
                    else if (field.type == NDBFieldType.FloatArray)
                    {
                        float[] result = new float[len];
                        for (int i = 0; i < len; ++i)
                        {
                            result[i] = *((float*)(startByte + sizeof(float) * i));
                        }
                        return result;
                    }
                    else if (field.type == NDBFieldType.IntArray)
                    {
                        int[] result = new int[len];
                        for (int i = 0; i < len; ++i)
                        {
                            result[i] = *((int*)(startByte + sizeof(int) * i));
                        }
                        return result;
                    }
                    else if (field.type == NDBFieldType.LongArray)
                    {
                        long[] result = new long[len];
                        for (int i = 0; i < len; ++i)
                        {
                            result[i] = *((long*)(startByte + sizeof(long) * i));
                        }
                        return result;
                    }
                }
            }

            return null;
        }
    }
}
