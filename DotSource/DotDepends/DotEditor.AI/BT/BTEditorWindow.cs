using DotEngine.AI.BT.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DotEditor.AI.BT
{
    public class BTEditorWindow : EditorWindow
    {
        [MenuItem("Game/Behaviour Tree")]
        public static void Open()
        {
            var win = GetWindow<BTEditorWindow>("Behaviour Tree Window");
            win.Show();
        }

        private static readonly string StyleSheetPath = "BehaviourTree/Styles/BTStyles";
        private static readonly string UXMLSheetPath = "BehaviourTree/UXML/BTEditorWindow";

        private BTGraphView graphView;
        private BTNodeData rootNodeData = null;

        //private VisualElement mainContainer;
        //private VisualElement graphViewContainer;
        //private Button openButton;
        //private Button newButton;
        //private Button saveButton;

        void OnEnable()
        {
            StyleSheet sheetAsset = Resources.Load<StyleSheet>(StyleSheetPath);
            rootVisualElement.styleSheets.Add(sheetAsset);

            #region UXML Content
            //var tpl = Resources.Load<VisualTreeAsset>(UXMLSheetPath);
            //mainContainer = tpl.Instantiate();
            //rootVisualElement.Add(mainContainer);

            //openButton = mainContainer.Q("openButton") as Button;
            //openButton.clickable.clicked += () =>
            //{

            //};
            //newButton = mainContainer.Q("newButton") as Button;
            //newButton.clickable.clicked += () =>
            //{

            //};
            //saveButton = mainContainer.Q("saveButton") as Button;
            //saveButton.style.backgroundColor = Color.green;
            //saveButton.clickable.clicked += () =>
            //{

            //};

            //graphViewContainer = mainContainer.Q("graphViewContainer");
            #endregion

            graphView = new BTGraphView(this);
            rootVisualElement.Add(graphView);
            graphView.StretchToParentSize();

        }
    }
}
