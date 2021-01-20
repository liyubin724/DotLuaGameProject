using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class UnsupportedTypeDrawer : NLayoutDrawer
    {
        public string Label { get; set; } = null;
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