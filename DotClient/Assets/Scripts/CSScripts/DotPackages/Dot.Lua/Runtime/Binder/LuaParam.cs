using System;
using System.Collections.Generic;
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
    public class LuaParamValue
    {
        public LuaParamType paramType = LuaParamType.Integer;
        public int intValue;
        public float floatValue;
        public string strValue;

        [HideInInspector]
        public GameObject gObject;
        public UnityObject uObject;

        public SystemObject GetValue()
        {
            switch (paramType)
            {
                case LuaParamType.Integer:
                    return intValue;
                case LuaParamType.Float:
                    return floatValue;
                case LuaParamType.String:
                    return strValue;
                case LuaParamType.UObject:
                    if(uObject!=null && uObject.GetType().IsAssignableFrom(typeof(LuaBinderBehaviour)))
                    {
                        LuaBinderBehaviour beh = uObject as LuaBinderBehaviour;
                        beh.InitBehaviour();
                        return beh.Table;
                    }else
                    {
                        return uObject;
                    }
                default:
                    return null;
            }
        }
    }

    [Serializable]
    public class LuaParam
    {
        public string name;
        public LuaParamValue value = new LuaParamValue();

        public SystemObject GetValue() => value.GetValue();
    }

    [Serializable]
    public class LuaParams
    {
        public string name;
        public List<LuaParamValue> values = new List<LuaParamValue>();

        public bool IsEmpty() => values.Count == 0;

        public SystemObject[] GetValues()
        {
            SystemObject[] arr = new SystemObject[values.Count];
            for (int i = 0; i < values.Count; ++i)
            {
                LuaParamValue lpv = values[i];
                if (lpv != null)
                {
                    arr[i] = lpv.GetValue();
                }
            }
            return arr;
        }
    }
}
