using DotEditor.GUIExtension;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer
{
    public class NativeDrawerEditor : Editor
    {
        private NativeDrawerObject drawerObject = null;

        void OnEnable()
        {
            drawerObject = new NativeDrawerObject(target)
            {
                IsShowScroll = IsShowScroll(),
            };
        }

        protected virtual bool IsShowScroll()
        {
            return true;
        }

        protected virtual float GetLabelWidth()
        {
            return 120;
        }

        public override void OnInspectorGUI()
        {
            EGUILayout.DrawScript(target);

            EditorGUILayout.Space();

            EGUI.BeginGUIColor(Color.grey);
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EGUILayout.DrawBoxHeader("Native Drawer Setting", GUILayout.ExpandWidth(true));
                    EGUI.BeginIndent();
                    {
                        NativeDrawerSetting.IsShowHelp = EditorGUILayout.Toggle("Is Show Help", NativeDrawerSetting.IsShowHelp);
                    }
                    EGUI.EndIndent();
                }
                EditorGUILayout.EndVertical();
            }
            EGUI.EndGUIColor();

            EditorGUILayout.Space();

            EGUI.BeginLabelWidth(GetLabelWidth());
            {
                drawerObject.OnGUILayout();
            }
            EGUI.EndLableWidth();
        }
    }
}
