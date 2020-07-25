using System;
using System.Collections.Generic;
using System.Text;

namespace Json2Lua.Lua
{
    public class LuaTable
    {
        private List<LuaObject> mItems_order = new List<LuaObject>(); //存放table中的有序部分

        private Dictionary<object, LuaObject> mItems_kv = new Dictionary<object, LuaObject>(); //存放table中的无序部分


        public void AddItem(object value)
        {
            mItems_order.Add(new LuaObject(value));
        }

        public void AddItem(object key,object value)
        {
            if (!mItems_kv.ContainsKey(key))
            {
                mItems_kv.Add(key, new LuaObject(value));
            }
        }

        public string GetString(int indent = 0)
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine($"{new string(' ', indent)}{{");
            if(mItems_order.Count>0)
            {
                indent++;
                {
                    foreach(var item in mItems_order)
                    {
                        text.AppendLine($"{new string(' ', indent)}{item.GetString(indent)},");
                    }
                } 
                indent--;
            }

            if(mItems_kv.Count>0)
            {
                indent++;
                {
                    foreach (var item in mItems_kv)
                    {
                        object key = item.Key;
                        LuaObject value = item.Value;

                        Type keyType = key.GetType();
                        if (keyType.IsPrimitive && keyType.IsValueType)
                        {
                            text.AppendLine($"{new string(' ', indent)}[{key}] = {value.GetString(indent)},");
                        }
                        else
                        {
                            text.AppendLine($"{new string(' ', indent)}{key} = {value.GetString(indent)},");
                        }
                    }
                }
                indent--;
                
            }
            text.Append($"{new string(' ', indent)}}}");

            return text.ToString();
        }
    }
}
