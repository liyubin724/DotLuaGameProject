using DotEngine.Log;
using System;
using UnityEngine;
using XLua;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Lua.Register
{
    [Serializable]
    public class RegisterObjectData
    {
        public ObjectData[] objectDatas = new ObjectData[0];
        public ObjectArrayData[] objectArrayDatas = new ObjectArrayData[0];

        public void RegisterToLua(LuaEnv luaEnv,LuaTable objTable)
        {
            if(luaEnv!=null && objTable !=null)
            {
                RegisterLuaObject(objTable);
                RegisterLuaObjectArr(luaEnv, objTable);
            }
        }

        void RegisterLuaObject(LuaTable objTable)
        {
            for (int i = 0; i < objectDatas.Length; i++)
            {
                if (objectDatas[i].obj == null || objectDatas[i].regObj == null)
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaBehaviour::RegisterLuaObjects->obj or regObj is Null");
                    continue;
                }
                string regName = objectDatas[i].name;
                if (string.IsNullOrEmpty(regName))
                {
                    regName = objectDatas[i].regObj.name;
                }

                objTable.Set(regName, objectDatas[i].regObj);
            }
        }

        void RegisterLuaObjectArr(LuaEnv luaEnv, LuaTable objTable)
        {
            if (objectArrayDatas != null && objectArrayDatas.Length > 0)
            {
                for (int i = 0; i < objectArrayDatas.Length; i++)
                {
                    if (string.IsNullOrEmpty(objectArrayDatas[i].name))
                    {
                        LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaBehaviour::RegisterLuaObjectArr->Group Name is Null, index = " + i);
                        continue;
                    }

                    LuaTable regTable = luaEnv.NewTable();

                    ObjectData[] luaObjs = objectArrayDatas[i].objects;
                    if (luaObjs != null && luaObjs.Length > 0)
                    {
                        for (int j = 0; j < luaObjs.Length; j++)
                        {
                            if (luaObjs[j].regObj == null)
                            {
                                LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaBehaviour::RegisterLuaObjectArr->obj or regObj is Null");
                                continue;
                            }

                            regTable.Set(j + 1, luaObjs[j].regObj);
                        }
                    }
                    objTable.Set<string, LuaTable>(objectArrayDatas[i].name, regTable);
                    regTable.Dispose();
                    regTable = null;
                }
            }
        }

        [Serializable]
        public class ObjectData
        {
            public string name;
            public GameObject obj;
            public UnityObject regObj;
            public string typeName;
        }

        [Serializable]
        public class ObjectArrayData
        {
            public string name;
            public ObjectData[] objects = new ObjectData[0];
        }
    }   
}
