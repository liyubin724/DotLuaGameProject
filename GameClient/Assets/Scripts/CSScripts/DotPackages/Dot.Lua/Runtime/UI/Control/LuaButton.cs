﻿using DotEngine.Log;
using DotEngine.Lua.Binder;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.Lua.UI
{
    [AddComponentMenu("DotEngine/UI/Lua Button", 100)]
    public class LuaButton : Button
    {
        public LuaBinderBehaviour binderBehaviour = null;
        public string clickedFuncName = string.Empty;
        public LuaParam[] paramValues = new LuaParam[0];

        protected override void Awake()
        {
            base.Awake();

            onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            if(binderBehaviour == null)
            {
                LogUtil.Error("UI", "LuaButton:OnClicked->the behaviour is null");
                return;
            }
            if(string.IsNullOrEmpty(clickedFuncName))
            {
                LogUtil.Error("UI", "LuaButton:OnClicked->the funcName is empty");
                return;
            }
            if(paramValues == null || paramValues.Length == 0)
            {
                binderBehaviour.CallAction(clickedFuncName);
            }else
            {
                binderBehaviour.CallActionWith(clickedFuncName, paramValues);
            }
        }
    }
}
