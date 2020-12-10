using DotEditor.GUIExtension;
using DotEditor.NativeDrawer;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BD
{
    public class PropertyAreaDrawer : AreaDrawer
    {
        private const float TITLE_HEIGHT = 20.0f;

        private Vector2 m_ScrollPos = Vector2.zero;

        private DrawerObject m_CutsceneDataDrawer = null;
        private DrawerObject m_TrackGroupDataDrawer = null;
        private DrawerObject m_TrackDataDrawer = null;
        private DrawerObject m_ActionDataDrawer = null;
        public PropertyAreaDrawer(EditorWindow win) : base(win)
        {
            CutsceneEditorData.EditorData.SelectedCutsceneChangedEvent += OnCutsceneChanged;
        }

        private void OnCutsceneChanged()
        {
            m_CutsceneDataDrawer = null;
            if(CutsceneEditorData.EditorData.Cutscene!=null)
            {
                m_CutsceneDataDrawer = new DrawerObject(CutsceneEditorData.EditorData.Cutscene)
                {
                    IsShowInherit = false,
                    IsShowScroll = false,
                    TitleContent = "Cutscene Data",
                };
            }
        }

        public override void OnGUI(Rect rect)
        {
            Rect titleRect = new Rect(rect.x, rect.y, rect.width, TITLE_HEIGHT);
            EGUI.DrawBoxHeader(titleRect, Contents.titleStr, EGUIStyles.BoxedHeaderCenterStyle);

            GUILayout.BeginArea(new Rect(rect.x, rect.y + TITLE_HEIGHT+2, rect.width-2, rect.height - TITLE_HEIGHT-2));
            {
                m_CutsceneDataDrawer?.OnGUILayout();
                EGUILayout.DrawHorizontalLine();
            }
            GUILayout.EndArea();
        }

        class Contents
        {
            public static string titleStr = "Property";

            public static GUIContent nameContent = new GUIContent("Name");
        }

        class Styles
        {

        }
    }
}
