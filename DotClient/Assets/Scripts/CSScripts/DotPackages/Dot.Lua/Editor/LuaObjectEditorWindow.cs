using DotEngine.Lua;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties.UI;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua
{
    public class LuaObjectEditorWindow : EditorWindow
    {
        [MenuItem("Test/Lua/Lua Object Window")]
        static void ShowWin()
        {
            var win = GetWindow<LuaObjectEditorWindow>();
            win.titleContent = new GUIContent("Lua Object Window");
            win.Show();
        }

        LuaParamValue paramValue = new LuaParamValue();

        private void OnEnable()
        {
            var propertyElement = new PropertyElement();
            propertyElement.SetTarget(paramValue);
            rootVisualElement.Add(propertyElement);
        }
    }

}
