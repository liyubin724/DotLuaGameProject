using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.Lua.UI.Handler
{
    [RequireComponent(typeof(InputField))]
    public class LuaInputFieldHandler : MonoBehaviour
    {
        public InputField inputField;

        public EventHandlerData valueChangedHandlerData = new EventHandlerData();
        public EventHandlerData endEditHandlerData = new EventHandlerData();

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
            valueChangedHandlerData.InvokeWithParam1<string>(text);
        }

        private void OnEndEdit(string text)
        {
            endEditHandlerData.InvokeWithParam1<string>(text);
        }
    }
}
