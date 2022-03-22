using DotEngine.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEditor.FSM
{
    public class AssetDataEditorWindow : EditorWindow
    {
        [MenuItem("Game/FSM/Asset Window")]
        public static void Open()
        {
            var win = EditorWindow.GetWindow<AssetDataEditorWindow>();
            win.titleContent = new GUIContent("Asset Window");
            win.Show();
        }

        void OnEnable()
        {
            var root = rootVisualElement;

            BlackboardData data = new BlackboardData();

            BlackboardView bbView = new BlackboardView();
            bbView.BindedData = data;
            root.Add(bbView);
        }

    }
}
