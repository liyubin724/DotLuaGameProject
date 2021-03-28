using DotEngine.Lua.Binder;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DotEngine.Lua.UI
{
    public class LuaButton : Button
    {
        public LuaBinderBehaviour binderBehaviour = null;
        public string funcName = string.Empty;
        public LuaParam[] paramValues = new LuaParam[0];

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("LuaButton.onClick", this);
            if(binderBehaviour!=null && !string.IsNullOrEmpty(funcName))
            {
                binderBehaviour.CallActionWith(funcName, paramValues);
            }
        }
    }
}
