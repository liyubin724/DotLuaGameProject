using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.Lua.UI.Handler
{
    [RequireComponent(typeof(InputField))]
    public class LuaInputFieldHandler : MonoBehaviour
    {
        public InputField inputField;

        public LuaEventHandler changedEventHandler = new LuaEventHandler();
        public LuaEventHandler sumitedEventHandler = new LuaEventHandler();

        private void Awake()
        {
            if(inputField == null)
            {
                inputField = GetComponent<InputField>();
            }
            if(inputField!=null)
            {
                inputField.onValueChanged.AddListener(OnValueChanged);
                inputField.onEndEdit.AddListener(OnEndEdit);
            }
        }

        private void OnValueChanged(string text)
        {
            changedEventHandler.InvokeWithParam1(text);
        }

        private void OnEndEdit(string text)
        {
            sumitedEventHandler.InvokeWithParam1(text);
        }
    }
}
