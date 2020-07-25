using System;
using System.Collections;
using System.Collections.Generic;

namespace Json2Lua.Lua
{
    public enum ELuaItemType
    {
        Nil = 0,

        Int,
        Float,
        Long,
        Boolean,
        String,

        Table,
    }

    public class LuaObject
    {
        public ELuaItemType type;

        public int value_int;
        public float value_float;
        public long value_long;

        public bool value_boolean;
        public string value_str;

        public LuaTable value_table;

        public LuaObject(object value)
        {
            if(value == null)
            {
                type = ELuaItemType.Nil;
            }else
            {
                Type valueType = value.GetType();
                if(valueType.IsArray || 
                    (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    type = ELuaItemType.Table;
                    value_table = new LuaTable();
                    foreach(var item in (IList)value)
                    {
                        value_table.AddItem(item);
                    }
                }else
                {
                    if (valueType == typeof(int))
                    {
                        type = ELuaItemType.Int;
                        value_int = (int)value;
                    }
                    else if (valueType == typeof(long))
                    {
                        type = ELuaItemType.Long;
                        value_long = (long)value;
                    }
                    else if (valueType == typeof(float))
                    {
                        type = ELuaItemType.Float;
                        value_float = (float)value;
                    }
                    else if (valueType == typeof(bool))
                    {
                        type = ELuaItemType.Boolean;
                        value_boolean = (bool)value;
                    }
                    else if (valueType == typeof(string))
                    {
                        type = ELuaItemType.String;
                        value_str = (string)value;
                    }
                    else if (valueType == typeof(LuaTable))
                    {
                        type = ELuaItemType.Table;
                        value_table = (LuaTable)value;
                    }
                    else
                    {
                        type = ELuaItemType.Nil;
                    }
                }
            }
        }

        public string GetString(int indent)
        {
            switch (type)
            {
                case ELuaItemType.Table:
                    return value_table.GetString(indent);
                case ELuaItemType.Boolean:
                    return value_boolean ? "true" : "false";
                case ELuaItemType.Float:
                    return value_float.ToString();
                case ELuaItemType.Int:
                    return value_int.ToString();
                case ELuaItemType.Long:
                    return value_long.ToString();
                case ELuaItemType.String:
                    return "[["+ value_str+"]]";
                case ELuaItemType.Nil:
                    return "nil";
                default:
                    return "";
            }
        }
    }
}
