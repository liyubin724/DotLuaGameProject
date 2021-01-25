using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class UnsupportedTypeDrawer : TypeDrawer
    {
        public Type TargetType { get; set; } = null;

        public override void OnGUILayout()
        {
            EGUI.BeginGUIColor(Color.red);
            {
                EditorGUILayout.LabelField(Label, $"The \"{TargetType.Name}\" is not supported!");
            }
            EGUI.EndGUIColor();
        }
    }
}