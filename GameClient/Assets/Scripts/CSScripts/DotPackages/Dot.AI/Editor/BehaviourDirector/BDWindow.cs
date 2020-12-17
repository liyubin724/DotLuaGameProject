using DotEngine.BD.Datas;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BD
{
    public class BDWindow : EditorWindow
    {
        private const int TOOLBAR_HEIGHT = 20;

        [MenuItem("Game/AI/Behaviour Director")]
        static void ShowWin()
        {
            var win = GetWindow<BDWindow>();
            win.titleContent = new GUIContent("Behaviour Director");
            win.wantsMouseMove = true;
            win.Show();
        }

        private CutsceneAreaDrawer m_CutsceneAreaDrawer = null;

        private Rect m_ToolbarRect = new Rect();
        private Rect m_CutsceneRect = new Rect();

        private void OnEnable()
        {
            CutsceneData cutsceneData = new CutsceneData();
            cutsceneData.Name = "Test Cutscene";
            cutsceneData.Desc = "Just for test";

            m_CutsceneAreaDrawer = new CutsceneAreaDrawer(this, cutsceneData);
        }

        private void OnGUI()
        {
            m_ToolbarRect = new Rect(0, 0, position.width, TOOLBAR_HEIGHT);
            DrawToolbar(m_ToolbarRect);

            m_CutsceneRect = new Rect(0, TOOLBAR_HEIGHT, position.width, position.height - TOOLBAR_HEIGHT);
            m_CutsceneAreaDrawer.OnGUI(m_CutsceneRect);
        }

        private void DrawToolbar(Rect rect)
        {
            EditorGUI.LabelField(rect, GUIContent.none, EditorStyles.toolbar);
            EditorGUI.LabelField(rect, Contents.titleContent, Styles.titleStyle);

            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(Contents.createContent, EditorStyles.toolbarButton))
                    {

                    }

                    if (GUILayout.Button(Contents.openContent, EditorStyles.toolbarButton))
                    {

                    }

                    if (GUILayout.Button(Contents.saveContent, EditorStyles.toolbarButton))
                    {

                    }

                    if (GUILayout.Button(Contents.saveToContent, EditorStyles.toolbarButton))
                    {

                    }

                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button(Contents.zoomInContent, EditorStyles.toolbarButton))
                    {

                    }

                    if (GUILayout.Button(Contents.zoomOutContent, EditorStyles.toolbarButton))
                    {

                    }

                    if (GUILayout.Button(Contents.settingContent, EditorStyles.toolbarButton))
                    {

                    }

                    if (GUILayout.Button(Contents.helpContent, EditorStyles.toolbarButton))
                    {

                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

    }

    class Contents
    {
        public static GUIContent titleContent = new GUIContent("Director");

        public static GUIContent createContent = new GUIContent("Create", "Create New");
        public static GUIContent openContent = new GUIContent("Open", "Open");
        public static GUIContent saveContent = new GUIContent("Save", "Save");
        public static GUIContent saveToContent = new GUIContent("Save To", "Save To");

        public static GUIContent helpContent = new GUIContent("?", "Show help");
        public static GUIContent settingContent = new GUIContent("Setting", "Open Setting Window");
        public static GUIContent zoomInContent = new GUIContent("+", "Zoom in");
        public static GUIContent zoomOutContent = new GUIContent("-", "Zoom out");
    }

    class Styles
    {
        public static GUIStyle titleStyle = null;
        public static GUIStyle propertyStyle = null;

        static Styles()
        {
            titleStyle = new GUIStyle(EditorStyles.label);
            titleStyle.alignment = TextAnchor.MiddleCenter;
        }
    }
}
