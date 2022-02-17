using System.Collections.Generic;

namespace XLua
{
    public partial class LuaTable : LuaBase
    {
        public static LuaTable Create(LuaEnv luaEnv)
        {
            return luaEnv.NewTable();
        }

        public static LuaTable Create(LuaEnv luaEnv,string globalName)
        {
            LuaTable table = luaEnv.NewTable();
            luaEnv.Global.Set(globalName, table);
            return table;
        }

        public static LuaTable CreateFromList(LuaEnv luaEnv, IList<object> objList)
        {
            LuaTable table = luaEnv.NewTable();
            if (objList != null && objList.Count > 0)
            {
                for (int i = 0; i < objList.Count; ++i)
                {
                    table.Set(i + 1, objList[i]);
                }
            }
            return table;
        }

        public static LuaTable CreateFromDictionary(LuaEnv luaEnv, IDictionary<string, object> objDic)
        {
            LuaTable table = luaEnv.NewTable();
            if (objDic != null && objDic.Count > 0)
            {
                foreach (var kvp in objDic)
                {
                    table.Set(kvp.Key, kvp.Value);
                }
            }
            return table;
        }

        public void SetToGlobal(LuaEnv luaEnv,string globalName)
        {
            luaEnv.Global.Set(globalName, this);
        }

        public void SetByDictionary(IDictionary<string,object> objDic)
        {
            if (objDic != null && objDic.Count > 0)
            {
                foreach (var kvp in objDic)
                {
                    Set(kvp.Key, kvp.Value);
                }
            }
        }

    }
}
