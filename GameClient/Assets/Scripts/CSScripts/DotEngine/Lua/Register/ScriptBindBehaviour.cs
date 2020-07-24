using UnityEngine;
using XLua;

namespace DotEngine.Lua.Register
{
    public class ScriptBindBehaviour : MonoBehaviour
    {
        public LuaBindScript bindScript = new LuaBindScript();

        public LuaTable ObjTable
        {
            get
            {
                return bindScript.ObjTable;
            }
        }

        public void InitLua()
        {
            if(bindScript.InitLua())
            {
                OnInitFinished();
            }
        }

        protected virtual void OnInitFinished()
        {
            SetValue("gameObject", gameObject);
            SetValue("transform", transform);
        }

        protected virtual void Awake()
        {
            InitLua();

            CallAction(LuaConst.AWAKE_FUNCTION_NAME);
        }

        protected virtual void Start()
        {
            CallAction(LuaConst.START_FUNCTION_NAME);
        }

        protected virtual void OnEnable()
        {
            CallAction(LuaConst.ENABLE_FUNCTION_NAME);
        }

        protected virtual void OnDisable()
        {
            CallAction(LuaConst.DISABLE_FUNCTION_NAME);
        }

        protected virtual void OnDestroy()
        {
            if(bindScript.IsValid())
            {
                CallAction(LuaConst.DESTROY_FUNCTION_NAME);
                bindScript.Dispose();
            }
        }

        public void SetValue<T>(string name,T value)
        {
            bindScript.SetValue(name, value);
        }

        public void CallAction(string funcName)
        {
            bindScript.CallAction(funcName);
        }

        public void CallAction<T>(string funcName, T value)
        {
            bindScript.CallAction<T>(funcName, value);
        }

        public void CallAction<T1, T2>(string funcName, T1 value1, T2 value2)
        {
            bindScript.CallAction<T1, T2>(funcName, value1, value2);
        }

        public R CallFunc<R>(string funcName)
        {
            return bindScript.CallFunc<R>(funcName);
        }

        public R CallFunc<T, R>(string funcName, T value)
        {
            return bindScript.CallFunc<T, R>(funcName,value);
        }
        public R CallFunc<T1, T2, R>(string funcName, T1 value1, T2 value2)
        {
            return bindScript.CallFunc<T1, T2, R>(funcName, value1, value2);
        }    
    }
}
