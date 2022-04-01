using DotEditor.UIElements;
using DotEngine.Config.Ini;
using DotEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DotEditor.Config.Ini
{
    public class IniConfigEditorWindow : EditorWindow
    {
        [MenuItem("Game/Config/Ini Config Window &G")]
        public static void Open()
        {
            var win = GetWindow<IniConfigEditorWindow>();
            win.titleContent = new GUIContent("Ini Config Window");
            win.Show();
        }

        private const string INI_WINDOW_UXML = "dot_config_ini_window_uxml";

        [Q("toolbar-create-btn")]
        private ToolbarButton toolbarCreateBtn;
        [Q("toolbar-open-btn")]
        private ToolbarButton toolbarOpenBtn;
        [Q("toolbar-save-btn")]
        private ToolbarButton toolbarSaveBtn;
        [Q("toolbar-saveas-btn")]
        private ToolbarButton toolbarSaveAsBtn;

        [Q("content-container")]
        private VisualElement m_ContentContainer;

        private IniSectionListView m_SectionListView;
        private IniSectionView m_SectionView;

        private IniConfig configData = null;
        private string configPath = null;

        void OnEnable()
        {
            var root = rootVisualElement;
            var visualTreeAsset = Resources.Load<VisualTreeAsset>(INI_WINDOW_UXML);
            var visualTree = visualTreeAsset.CloneTree();
            visualTree.SetWidth(100, LengthUnit.Percent);
            visualTree.SetHeight(100, LengthUnit.Percent);
            root.Add(visualTree);

            visualTree.AssignQueryResults(this);

            toolbarCreateBtn.clicked += () =>
            {
                var x = root.layout.x + toolbarCreateBtn.layout.x;
                var y = root.layout.y + toolbarCreateBtn.layout.y;

                x += Input.mousePosition.x + position.x;
                y += Input.mousePosition.y + position.y;

                x = toolbarCreateBtn.worldBound.x;
                y = toolbarCreateBtn.worldBound.y;

                CustomPopupWindow.OpenWindow(new Rect(x,y,400,300), null, true, false);
            };
            toolbarOpenBtn.clicked += () =>
            {

            };
            toolbarSaveBtn.clicked += () =>
            {

            };
            toolbarSaveAsBtn.clicked += () =>
            {

            };

            m_SectionListView = new IniSectionListView();
            m_SectionView = new IniSectionView();

            TwoPaneSplitView splitView = new TwoPaneSplitView(0,200,TwoPaneSplitViewOrientation.Horizontal);
            splitView.Add(m_SectionListView);
            splitView.Add(m_SectionView);
            m_ContentContainer.Add(splitView);
        }
    }
}
