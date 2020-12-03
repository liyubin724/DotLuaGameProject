using DotEditor.GUIExtension;
using DotEditor.NativeDrawer;
using DotEditor.NativeDrawer.Property;
using System;
using UnityEditor;
using UnityEngine;
using static DotEditor.Entity.Avatar.AvatarCreatorData;

namespace DotEditor.Entity.Avatar
{
    [CustomTypeDrawer(typeof(AvatarPartCreatorData))]
    public class AvatarPartCreatorDataDrawer : PropertyContentDrawer
    {
        public static Action<AvatarPartCreatorData> CreatePartBtnClick = null;
        public static Action<AvatarPartCreatorData> PreviewPartBtnClick = null;

        private DrawerObject drawerObject = null;

        protected override bool IsValidProperty()
        {
            return Property.ValueType == typeof(AvatarPartCreatorData);
        }

        protected override void OnDrawProperty(string label)
        {
            if (drawerObject == null)
            {
                drawerObject = new DrawerObject(Property.Value);
            }
            if (Property.IsArrayElement)
            {
                AvatarPartCreatorData partCreatorData = (AvatarPartCreatorData)Property.Value;
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(label, UnityEngine.GUILayout.Width(25));
                    EditorGUILayout.BeginVertical();
                    {
                        drawerObject.OnGUILayout();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EGUI.BeginGUIBackgroundColor(Color.cyan);
                {
                    if (GUILayout.Button("Create Part"))
                    {
                        CreatePartBtnClick?.Invoke(partCreatorData);
                    }
                    if (GUILayout.Button("Preview Part"))
                    {
                        PreviewPartBtnClick?.Invoke(partCreatorData);
                    }
                }
                EGUI.EndGUIBackgroundColor();
            }
            else
            {
                EditorGUILayout.LabelField(label);
                EditorGUI.indentLevel++;
                {
                    drawerObject.OnGUILayout();
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}
