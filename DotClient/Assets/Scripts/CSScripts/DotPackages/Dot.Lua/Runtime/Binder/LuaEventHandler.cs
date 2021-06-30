using System;
using SystemObject = System.Object;

namespace DotEngine.Lua.UI
{
    [Serializable]
    public class LuaEventHandler
    {
        public LuaBindBehaviour bindBehaviour = null;
        private string functionName = string.Empty;
        public LuaParamValue[] paramValues = new LuaParamValue[0];

        public void Invoke()
        {
            if (bindBehaviour != null && !string.IsNullOrEmpty(functionName))
            {
                bindBehaviour.CallActionWithParams(functionName, paramValues);
            }
        }

        public void InvokeWithParam1<T>(T value)
        {
            if (bindBehaviour != null && !string.IsNullOrEmpty(functionName))
            {
                SystemObject[] values = new SystemObject[paramValues.Length + 1];
                for (int i = 0; i < paramValues.Length; ++i)
                {
                    values[i] = paramValues[i].GetValue();
                }
                values[paramValues.Length] = value;

                bindBehaviour.CallActionWithObjects(functionName, values);
            }
        }

        public void InvokeWithParam2<T1, T2>(T1 value1, T2 value2)
        {
            if (bindBehaviour != null && !string.IsNullOrEmpty(functionName))
            {
                SystemObject[] values = new SystemObject[paramValues.Length + 2];
                for (int i = 0; i < paramValues.Length; ++i)
                {
                    values[i] = paramValues[i].GetValue();
                }
                values[paramValues.Length] = value1;
                values[paramValues.Length + 1] = value2;

                bindBehaviour.CallActionWithObjects(functionName, values);
            }
        }
    }
}
