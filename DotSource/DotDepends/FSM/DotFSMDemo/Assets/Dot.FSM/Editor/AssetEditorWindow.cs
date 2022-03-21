using DotEngine.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DotEditor.FSM
{
    public class AssetEditorWindow : EditorWindow
    {
        [MenuItem("Game/FSM/Asset Editor Window")]
        static void OpenWindow()
        {
            var win = GetWindow<AssetEditorWindow>();
            win.titleContent = new GUIContent("FSM Asset Window");
            win.Show();
        }

        private const string ASSET_WINDOW_UXML = "dot_fsm_asset_window";

        private AssetData assetData = null;

        private void OnEnable()
        {
            var root = rootVisualElement;

            var visualTreeAsset = Resources.Load<VisualTreeAsset>(ASSET_WINDOW_UXML);
            var visualTree = visualTreeAsset.CloneTree();
            root.Add(visualTree);
        }

        private void DrawToolbar()
        {

        }


    }
}
