using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.Lua.UI.Handler
{
    [RequireComponent(typeof(Button))]
    public class LuaButtonHandler : MonoBehaviour
    {
        public Button button = null;
        public LuaEventHandler clickedEventHandler = new LuaEventHandler();

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
            clickedEventHandler.Invoke();
        }
    }
}
