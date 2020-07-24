using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace DotEngine.Serialize
{
    public static class LuaSerializeWriter
    {
        /// <summary>
        /// 格式化Lua的字符串
        /// </summary>
        /// <param name="luaStr">需要格式化的Lua字符串</param>
        /// <returns>格式化后的Lua字符串</returns>
        public static string FormatLua(string luaStr)
        {
            StringBuilder luaSB = new StringBuilder();
            int indent = 0;
            for (int i = 0; i < luaStr.Length; i++)
            {
                char ch = luaStr[i];
                if (ch == '{')
                {
                    luaSB.Append(ch);
                    luaSB.AppendLine();

                    indent++;
                    luaSB.Append(GetIndentStr(indent));
                }
                else if (ch == '}')
                {
                    indent--;
                    luaSB.Append('\n');
                    luaSB.Append(GetIndentStr(indent));
                    luaSB.Append(ch);

                    if (i + 1 < luaStr.Length && luaStr[i + 1] != ',')
                    {
                        luaSB.Append('\n');
                        luaSB.Append(GetIndentStr(indent));
                    }
                }
                else if (ch == ',')
                {
                    luaSB.Append(ch);
                    if (i + 1 < luaStr.Length && luaStr[i + 1] != '}')
                    {
                        luaSB.Append('\n');
                        luaSB.Append(GetIndentStr(indent));
                    }
                }
                else
                {
                    luaSB.Append(ch);
                }
            }
            return luaSB.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="paramName"></param>
        /// <param name="isGlobal"></param>
        /// <param name="isReturn"></param>
        /// <returns></returns>
        public static string WriteToLua(object data, string paramName, bool isGlobal, bool isReturn)
        {
            if (data == null)
            {
                return null;
            }
            StringBuilder luaSB = new StringBuilder();
            if (isGlobal)
            {
                luaSB.Append(paramName + " = ");
            }
            else
            {
                luaSB.Append($"local {paramName} = ");
            }

            WriteValueToLua(luaSB, 0, data, false);

            if (isReturn)
            {
                luaSB.AppendLine($"return {paramName}");
            }
            return luaSB.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string WriteToLua(object data)
        {
            if (data == null)
            {
                return null;
            }

            StringBuilder luaSB = new StringBuilder();
            WriteValueToLua(luaSB, 0, data, false);
            return luaSB.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="luaSB"></param>
        /// <param name="indent"></param>
        /// <param name="data"></param>
        private static void WriteObjectToLua(StringBuilder luaSB, int indent, object data)
        {
            Type type = data.GetType();
            string indentStr = GetIndentStr(indent);
            luaSB.Append("{");

            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            if (fieldInfos.Length > 0)
            {
                string fieldIndentStr = GetIndentStr(indent + 1);
                foreach (var fieldInfo in fieldInfos)
                {
                    luaSB.Append("\n" + fieldIndentStr);
                    WriteValueToLua(luaSB, indent + 1, fieldInfo.Name, true);
                    luaSB.Append("=");
                    WriteValueToLua(luaSB, indent + 1, fieldInfo.GetValue(data), false);
                    luaSB.Append(",");
                }
            }

            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            if (propertyInfos.Length > 0)
            {
                string propertyIndentStr = GetIndentStr(indent + 1);
                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyInfo.GetGetMethod() == null)
                    {
                        continue;
                    }
                    luaSB.Append("\n" + propertyIndentStr);
                    WriteValueToLua(luaSB, indent + 1, propertyInfo.Name, true);
                    luaSB.Append("=");
                    WriteValueToLua(luaSB, indent + 1, propertyInfo.GetValue(data), false);
                    luaSB.Append(",");
                }
            }

            luaSB.Append("\n" + indentStr + "}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="luaSB"></param>
        /// <param name="indent"></param>
        /// <param name="data"></param>
        /// <param name="isAsKey"></param>
        private static void WriteValueToLua(StringBuilder luaSB, int indent, object data, bool isAsKey)
        {
            Type type = data.GetType();
            if (type.IsValueType && type.IsPrimitive)
            {
                string value = data.ToString().ToLower();
                if (isAsKey)
                {
                    luaSB.Append($"[{value}]");
                }
                else
                {
                    luaSB.Append(value);
                }
            }
            else if (type.IsValueType && type.IsEnum)
            {
                string value = ((int)data).ToString();
                if (isAsKey)
                {
                    luaSB.Append($"[{value}]");
                }
                else
                {
                    luaSB.Append(value);
                }
            }
            else if (type == typeof(string))
            {
                if (isAsKey)
                {
                    luaSB.Append(data.ToString());
                }
                else
                {
                    luaSB.Append($"[[{data.ToString()}]]");
                }
            }
            else if (typeof(IList).IsAssignableFrom(type) || type.IsArray)
            {
                WriteListToLua(luaSB, indent, (IList)data);
            }
            else if (typeof(IDictionary).IsAssignableFrom(type))
            {
                WriteDicToLua(luaSB, indent, (IDictionary)data);
            }
            else
            {
                WriteObjectToLua(luaSB, indent, data);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="luaSB"></param>
        /// <param name="indent"></param>
        /// <param name="list"></param>
        private static void WriteListToLua(StringBuilder luaSB, int indent, IList list)
        {
            string indentStr = GetIndentStr(indent + 1);
            luaSB.Append("{");
            foreach (var data in list)
            {
                luaSB.Append("\n" + indentStr);
                WriteValueToLua(luaSB, indent + 1, data, false);
                luaSB.Append(",");
            }
            indentStr = GetIndentStr(indent);
            luaSB.Append("\n" + indentStr + "}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="luaSB"></param>
        /// <param name="indent">缩进量</param>
        /// <param name="dic"></param>
        private static void WriteDicToLua(StringBuilder luaSB, int indent, IDictionary dic)
        {
            string indentStr = GetIndentStr(indent + 1);
            luaSB.Append("{");
            IDictionaryEnumerator enumerator = dic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                luaSB.Append("\n" + indentStr);
                WriteValueToLua(luaSB, indent + 1, enumerator.Key, true);
                luaSB.Append(" = ");
                WriteValueToLua(luaSB, indent + 1, enumerator.Value, false);
                luaSB.Append(",");
            }
            indentStr = GetIndentStr(indent);
            luaSB.Append("\n" + indentStr + "}");
        }

        /// <summary>
        /// 生成缩进空白字符，每4个空格为一个缩进量
        /// </summary>
        /// <param name="indent">缩进量</param>
        /// <returns></returns>
        private static string GetIndentStr(int indent)
        {
            StringBuilder indentSB = new StringBuilder();
            for (int i = 0; i < indent; i++)
            {
                indentSB.Append("    ");
            }
            return indentSB.ToString();
        }

    }
}
