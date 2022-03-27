using DotEngine.Config.Ini;
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

        private IniConfig configData = null;
        private string configPath = null;
        private VisualElement sectionListView = null;
        
        void OnEnable()
        {
            DrawToolbar();
            DrawContainer();
        }

        void DrawToolbar()
        {
            var root = rootVisualElement;

            Toolbar toolbar = new Toolbar();
            ToolbarButton openBtn = new ToolbarButton(() =>
            {
                
            });
            openBtn.name = "open-toolbar-button";
            openBtn.text = "Open";
            toolbar.Add(openBtn);

            ToolbarButton createBtn = new ToolbarButton(() =>
            {

            });
            openBtn.name = "create-toolbar-button";
            openBtn.text = "Create";
            toolbar.Add(openBtn);

            ToolbarButton saveBtn = new ToolbarButton(() =>
            {

            });
            saveBtn.name = "save-toolbar-button";
            saveBtn.text = "Save";
            toolbar.Add(saveBtn);

            root.Add(toolbar);
        }

        void DrawContainer()
        {
            TwoPaneSplitView splitView = new TwoPaneSplitView();
            splitView.fixedPaneInitialDimension = 200;
            splitView.fixedPaneIndex = 0;

            VisualElement sectionListContainer = new VisualElement();
            sectionListContainer.name = "section-list-container";
            splitView.Add(sectionListContainer);

            VisualElement sectionDetailContainer = new VisualElement();
            sectionDetailContainer.name = "section-detail-container";
            splitView.Add(sectionDetailContainer);

            sectionDetailContainer.Add(new IniPropertyView() { BindData = new IniProperty("Test1", "Value1")});

            rootVisualElement.Add(splitView);
        }

        void DrawSectionList()
        {

        }

        void DrawSectionDetail()
        {

        }
    }
}
