using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.Lua.UI
{
    [AddComponentMenu("DotEngine/UI/Lua Button", 100)]
    public class LuaButton : Button
    {
        public LuaEventHandler clickedEventHandler = new LuaEventHandler();
        protected override void Awake()
        {
            onClick.AddListener(OnClicked);
            base.Awake();
        }

        private void OnClicked()
        {
            clickedEventHandler.Invoke();
        }
    }
}
