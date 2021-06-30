using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.Lua.UI
{
    [AddComponentMenu("DotEngine/UI/Lua Input Field", 101)]
    public class LuaInputField : InputField
    {
        public LuaEventHandler changedEventHandler = new LuaEventHandler();
        public LuaEventHandler sumitedEventHandler = new LuaEventHandler();

        protected override void Awake()
        {
            onValueChanged.AddListener(OnValueChanged);
            onEndEdit.AddListener(OnValueEndEdit);
            base.Awake();
        }

        private void OnValueChanged(string value)
        {
            changedEventHandler.InvokeWithParam1(value);
        }

        private void OnValueEndEdit(string value)
        {
            sumitedEventHandler.InvokeWithParam1(value);
        }
    }
}
