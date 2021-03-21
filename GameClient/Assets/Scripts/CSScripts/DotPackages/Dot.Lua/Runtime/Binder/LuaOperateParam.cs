using System;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Lua.Binder
{
    public enum LuaOperateParamType
    {
        Integer,
        Float,
        String,
        UObject,
    }

    [Serializable]
    public class LuaOperateParam
    {
        public string name;
        public LuaOperateParamType paramType = LuaOperateParamType.Integer;

        public int intValue;
        public float floatValue;
        public string strValue;

        [HideInInspector]
        public GameObject gObject;
        public UnityObject uObject;

        public string GetName()
        {
            return name;
        }

        public SystemObject GetValue()
        {
            switch(paramType)
            {
                case LuaOperateParamType.Integer:
                    return intValue;
                case LuaOperateParamType.Float:
                    return floatValue;
                case LuaOperateParamType.String:
                    return strValue;
                default:
                    return null;
            }
        }
    }
}
