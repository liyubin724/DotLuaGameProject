using DotEditor.GUIExt.Field;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.Layout
{
    public class FieldValueLayoutDrawable : FieldValueProvider, ILayoutDrawable
    {
        public FieldValueLayoutDrawable(FieldInfo field, object target) : base(field, target)
        {
        }

        public void OnGUILayout()
        {
            SystemObject value = Value;
            if(value == null)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(FieldName);
                    if(GUILayout.Button("Create Instance"))
                    {
                        Value = FieldValueUtility.CreateInstance(ValueType);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }else
            {
                FieldValueDrawer drawer = FieldValueUtility.CreateDrawer(ValueType);
                if(drawer == null)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(FieldName);
                        EGUI.BeginGUIColor(Color.red);
                        {
                            EditorGUILayout.LabelField($"The drawer of \"{ValueType}\" is not found!");
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    float height = drawer.GetHeight();
                    Rect rect = EditorGUILayout.GetControlRect(false, height);
                    drawer.OnGUI(rect, FieldName, this);
                }
            }
        }
    }
}
