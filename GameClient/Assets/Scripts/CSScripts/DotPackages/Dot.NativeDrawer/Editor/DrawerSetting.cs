using DotEditor.GUIExtension;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer
{
    public static class DrawerSetting
    {
        public static bool IsShowHelp = true;

        internal static void OnDrawSetting()
        {
            EGUILayout.DrawHorizontalSpace(8);

            EGUI.BeginGUIColor(Color.grey);
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EGUILayout.DrawBoxHeader("Native Drawer Setting", GUILayout.ExpandWidth(true));
                    EGUI.BeginIndent();
                    {
                        IsShowHelp = EditorGUILayout.Toggle("Is Show Help", IsShowHelp);
                    }
                    EGUI.EndIndent();
                }
                EditorGUILayout.EndVertical();
            }
            EGUI.EndGUIColor();

            EGUILayout.DrawHorizontalSpace(8);
        }
    }
}
