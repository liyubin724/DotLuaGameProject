using DotEngine.Lua.Binder;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.Lua.UI
{
    [AddComponentMenu("DotEngine/UI/Lua Input Field", 101)]
    public class LuaInputField : InputField
    {
        public LuaBinderBehaviour binderBehaviour = null;

        public string changedFuncName = string.Empty;
        public LuaParam[] changedParamValues = new LuaParam[0];

        public string submitedFuncName = string.Empty;
        public LuaParam[] submitedParamValues = new LuaParam[0];

        protected override void Awake()
        {
            base.Awake();

            onValueChanged.AddListener(OnValueChanged);
            onEndEdit.AddListener(OnValueSubmited);
        }

        private void OnValueChanged(string value)
        {
            if(binderBehaviour != null && !string.IsNullOrEmpty(changedFuncName))
            {
                if(changedParamValues == null || changedParamValues.Length == 0)
                {
                    binderBehaviour.CallAction(changedFuncName);
                }else
                {
                    binderBehaviour.CallAction(changedFuncName, changedParamValues);
                }
            }
        }

        private void OnValueSubmited(string value)
        {
            if(binderBehaviour !=null && !string.IsNullOrEmpty(submitedFuncName))
            {
                if(submitedParamValues == null || submitedParamValues.Length == 0)
                {
                    binderBehaviour.CallAction(submitedFuncName);
                }else
                {
                    binderBehaviour.CallAction(submitedFuncName, submitedParamValues);
                }
            }
        }
    }
}
