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

        protected virtual float GetLabelWith()
        {
            return 120;
        }

        public override void OnInspectorGUI()
        {
            EGUILayout.DrawScript(target);

            EditorGUILayout.Space();

            EGUI.BeginGUIColor(Color.cyan);
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Native Drawer Setting", EditorStyles.toolbar);
                    EGUI.BeginIndent();
                    {
                        NativeDrawerSetting.IsShowHelp = EditorGUILayout.Toggle("Is Show Help", NativeDrawerSetting.IsShowHelp);
                        EGUILayout.DrawHorizontalLine();
                    }
                    EGUI.EndIndent();
                }
                EditorGUILayout.EndVertical();
                
            }
            EGUI.EndGUIColor();

            EditorGUILayout.Space();

            EGUI.BeginLabelWidth(GetLabelWith());
            {
                drawerObject.OnGUILayout();
            }
            EGUI.EndLableWidth();
        }
    }
}
