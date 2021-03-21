using System;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Lua.Binder
{
    public enum LuaParamType
    {
        Integer,
        Float,
        String,
        UObject,
    }

    [Serializable]
    public class LuaParam
    {
        public string name;
        public LuaParamType paramType = LuaParamType.Integer;

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
                case LuaParamType.Integer:
                    return intValue;
                case LuaParamType.Float:
                    return floatValue;
                case LuaParamType.String:
                    return strValue;
                default:
                    return null;
            }
        }
    }
}
