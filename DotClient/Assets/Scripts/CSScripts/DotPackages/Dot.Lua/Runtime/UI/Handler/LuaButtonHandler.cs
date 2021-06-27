using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.Lua.UI.Handler
{
    [RequireComponent(typeof(Button))]
    public class LuaButtonHandler : MonoBehaviour
    {
        public Button button = null;

        public EventHandlerData clickHandlerData = new EventHandlerData();

        private void Awake()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }
            if (button != null)
            {
                button.onClick.AddListener(OnClick);
            }
        }

        private void OnClick()
        {
            clickHandlerData.Invoke();
        }
    }
}
