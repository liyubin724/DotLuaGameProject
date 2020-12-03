using System;
using System.Collections.Generic;

namespace DotEngine.Config.Ini
{
    public class IniData
    {
        public string Key { get; set; }
        public string Value { get; set; } = "";
        public string Comment { get; set; } = "";
        public string[] OptionValues { get; set; } = new string[0];
    }

    public class IniGroup
    {
        public string Name { get; set; }
        public string Comment { get; set; }

        private Dictionary<string, IniData> dataDic = new Dictionary<string, IniData>();
        public void AddData(string key, string value, string comment, string[] optionValues)
        {
            if(!dataDic.TryGetValue(key,out IniData data))
            {
                data = new IniData();
                data.Key = key;

                dataDic.Add(key, data);
            }
            data.Value = value;
            data.Comment = comment;
            data.OptionValues = optionValues;
        }

        public string GetDataValue(string key)
        {
            if(dataDic.TryGetValue(key,out IniData data))
            {
                return data.Value;
            }
            return null;
        }
        
        public void DeleteData(string key)
        {
            if(dataDic.ContainsKey(key))
            {
                dataDic.Remove(key);
            }
        }
    }

    public class IniConfig
    {
        private Dictionary<string, IniGroup> groupDic = new Dictionary<string, IniGroup>();
        public IniConfig()
        {
        }

        public void ParseText(string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                return;
            }

            string[] lines = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines == null || lines.Length == 0)
            {
                return;
            }
            IniGroup group = null;
            foreach(var line in lines)
            {
                string trimLine = line.Trim();
                if(trimLine.StartsWith("*") || trimLine.Length<=1)
                {
                    continue;
                }else if(trimLine.StartsWith("#"))
                {
                    group = new IniGroup();
                    string[] values = trimLine.Substring(1).Split(new char[]{ '|' });
                    if(values.Length!=2)
                    {
                        group = null;
                    }
                    else
                    {
                        group.Name = values[0];
                        group.Comment = values[1];

                        groupDic.Add(group.Name, group);
                    }
                }else if(trimLine.StartsWith("-") && group!=null)
                {
                    string[] values = trimLine.Substring(1).Split(new char[] { '|' });
                    if (values.Length == 4 && !string.IsNullOrEmpty(values[0]))
                    {
                        string[] optionValues = new string[0];
                        if(!string.IsNullOrEmpty(values[3]))
                        {
                            optionValues = values[3].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        }
                        group.AddData(values[0], values[1], values[2], optionValues);
                    }
                }
            }
        }

        public string GetValueInGroup(string group, string key, string defaultValue = null)
        {
            if (groupDic.TryGetValue(group, out IniGroup g))
            {
                string value = g.GetDataValue(key);
                if(value!=null)
                {
                    return value;
                }
            }
            return defaultValue;
        }

        public string GetValue(string key,string defaultValue = null)
        {
            foreach(var gKVP in groupDic)
            {
                string value = GetValueInGroup(gKVP.Key, key);
                if(value != null)
                {
                    return value;
                }
            }

            return defaultValue;
        }

        public bool GetBool(string key,bool defaultValue = false)
        {
            string value = GetValue(key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if (bool.TryParse(value, out bool result))
            {
                return result;
            }
            return defaultValue;
        }

        public bool GetBoolInGroup(string group,string key,bool defaultValue = false)
        {
            string value = GetValueInGroup(group, key);
            if(string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if(bool.TryParse(value,out bool result))
            {
                return result;
            }
            return defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            string value = GetValue(key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        public int GetIntInGroup(string group, string key, int defaultValue = 0)
        {
            string value = GetValueInGroup(group, key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        public float GetFloat(string key, float defaultValue = 0.0f)
        {
            string value = GetValue(key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if (float.TryParse(value, out float result))
            {
                return result;
            }
            return defaultValue;
        }

        public float GetFloatInGroup(string group, string key, float defaultValue = 0.0f)
        {
            string value = GetValueInGroup(group, key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if (float.TryParse(value, out float result))
            {
                return result;
            }
            return defaultValue;
        }
    }
}
