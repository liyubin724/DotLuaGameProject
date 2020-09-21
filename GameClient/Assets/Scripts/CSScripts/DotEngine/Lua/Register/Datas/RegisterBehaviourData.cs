using DotEngine.Log;
using System;
using XLua;

namespace DotEngine.Lua.Register
{
    [Serializable]
    public class RegisterBehaviourData
    {
        public BindBehaviourData[] behaviourDatas = new BindBehaviourData[0];
        public BindBehaviourArrayData[] behaviourArrayDatas = new BindBehaviourArrayData[0];

        public void RegisterToLua(LuaEnv luaEnv, LuaTable objTable)
        {
            if (luaEnv != null && objTable != null)
            {
                RegisterLuaBehaviour(objTable);
                RegisterLuaBehaviourArr(luaEnv, objTable);
            }
        }

        void RegisterLuaBehaviour(LuaTable objTable)
        {
            if (behaviourDatas != null && behaviourDatas.Length > 0)
            {
                for (int i = 0; i < behaviourDatas.Length; i++)
                {
                    if (behaviourDatas[i].behaviour == null)
                    {
                        LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaBehaviour::RegisterLuaBehaviour->behaviour is null.objName = " + behaviourDatas[i].name + "  index = " + i);
                        continue;
                    }
                    behaviourDatas[i].behaviour.InitBinder();

                    objTable.Set<string, LuaTable>(behaviourDatas[i].name, behaviourDatas[i].behaviour.Table);
                }
            }
        }

        void RegisterLuaBehaviourArr(LuaEnv luaEnv, LuaTable objTable)
        {
            if (behaviourArrayDatas != null && behaviourArrayDatas.Length > 0)
            {
                for (int i = 0; i < behaviourArrayDatas.Length; i++)
                {
                    if (string.IsNullOrEmpty(behaviourArrayDatas[i].name))
                    {
                        LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaBehaviour::RegisterLuaBehaviourArr->Group Name is Null, index = " + i);
                        continue;
                    }

                    LuaTable behTable = luaEnv.NewTable();

                    ScriptBinderBehaviour[] behs = behaviourArrayDatas[i].behaviours;
                    if (behs != null && behs.Length > 0)
                    {
                        for (int j = 0; j < behs.Length; j++)
                        {
                            if (behs[j] == null)
                            {
                                LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaBehaviour::RegisterLuaBehaviourArr->behaviour is Null, index = " + j);
                                continue;
                            }
                            behs[j].InitBinder();

                            behTable.Set<int, LuaTable>(j + 1, behs[j].Table);
                        }
                    }

                    objTable.Set<string, LuaTable>(behaviourArrayDatas[i].name, behTable);
                    behTable.Dispose();
                }
            }
        }

        [Serializable]
        public class BindBehaviourData
        {
            public string name;
            public ScriptBinderBehaviour behaviour;
        }

        [Serializable]
        public class BindBehaviourArrayData
        {
            public string name;
            public ScriptBinderBehaviour[] behaviours = new ScriptBinderBehaviour[0];
        }
    }
}
