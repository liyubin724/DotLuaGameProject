using DotEngine.Lua.Binder;
using DotEngine.Utilities;
using UnityEngine;
using XLua;

namespace DotEngine.UI
{
    public class UIRoot : MonoBehaviour
    {
        public static UIRoot Root = null;
        public LuaBinderBehaviour binderBehaviour = null;

        private void Awake()
        {
            if(binderBehaviour == null)
            {
                binderBehaviour = GetComponent<LuaBinderBehaviour>();
            }
            Root = this;
            DontDestroyUtility.AddTransform(transform);
        }

        public LuaTable GetRootComponent()
        {
            binderBehaviour.InitBinder();
            return binderBehaviour.Table;
        }
    }
}
