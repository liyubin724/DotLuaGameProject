using DotEngine.Core;
using DotEngine.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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

            List<DependenceData<string>> dataList = new List<DependenceData<string>>();
            dataList.Add(new DependenceData<string>() { Main = "A", Depend = "B" });
            dataList.Add(new DependenceData<string>() { Main = "A", Depend = "B" });
            //dataList.Add(new DependenceData<string>() { Main = "C", Depend = "D" });
            //dataList.Add(new DependenceData<string>() { Main = "E", Depend = "B" });
            //dataList.Add(new DependenceData<string>() { Main = "F", Depend = "C" });
            //dataList.Add(new DependenceData<string>() { Main = "G", Depend = "E" });

            Button btn = new Button();
            btn.clicked += () =>
            {
                bool isTrue = CircularDependencyChecker.IsInCircular(dataList);
                Debug.LogError("Result = " + isTrue);
            };
            root.Add(btn);
        }

    }
}
