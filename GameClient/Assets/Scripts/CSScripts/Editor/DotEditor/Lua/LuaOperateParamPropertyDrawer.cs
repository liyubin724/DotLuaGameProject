using DotEditor.GUIExtension;
using DotEngine.Lua;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Lua
{
    [CustomPropertyDrawer(typeof(LuaOperateParam))]
    public class LuaOperateParamPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty paramTypeProperty = property.FindPropertyRelative("paramType");
            float height = EditorGUIUtility.singleLineHeight * 3;
            if(paramTypeProperty.intValue == (int)LuaOperateParamType.UObject)
            {
                SerializedProperty gObjectProperty = property.FindPropertyRelative("gObject");
                if(gObjectProperty.objectReferenceValue == null)
                {
                    height += EditorGUIUtility.singleLineHeight;
                }else
                {
                    height += EditorGUIUtility.singleLineHeight * 3;
                }
            }else
            {
                height += EditorGUIUtility.singleLineHeight;
            }

            return height;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return false;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect propertyDrawRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(propertyDrawRect, label);

            propertyDrawRect.y += propertyDrawRect.height;
            propertyDrawRect.x += 40;
            propertyDrawRect.width -= 40;

            SerializedProperty nameProperty = property.FindPropertyRelative("name");
            EditorGUI.PropertyField(propertyDrawRect, nameProperty);

            propertyDrawRect.y += propertyDrawRect.height;
            SerializedProperty typeProperty = property.FindPropertyRelative("paramType");
            EditorGUI.PropertyField(propertyDrawRect, typeProperty);

            LuaOperateParamType paramType = (LuaOperateParamType)typeProperty.intValue;
            propertyDrawRect.y += propertyDrawRect.height;
            if (paramType == LuaOperateParamType.Integer)
            {
                SerializedProperty valueProperty = property.FindPropertyRelative("intValue");
                EditorGUI.PropertyField(propertyDrawRect, valueProperty);
            }else if(paramType == LuaOperateParamType.Float)
            {
                SerializedProperty valueProperty = property.FindPropertyRelative("floatValue");
                EditorGUI.PropertyField(propertyDrawRect, valueProperty);
            }
            else if(paramType == LuaOperateParamType.String)
            {
                SerializedProperty valueProperty = property.FindPropertyRelative("strValue");
                EditorGUI.PropertyField(propertyDrawRect, valueProperty);
            }
            else if(paramType == LuaOperateParamType.UObject)
            {
                SerializedProperty gObjectProperty = property.FindPropertyRelative("gObject");
                SerializedProperty uObjectProperty = property.FindPropertyRelative("uObject");

                EditorGUI.PropertyField(propertyDrawRect, gObjectProperty);
                propertyDrawRect.y += propertyDrawRect.height;

                if(gObjectProperty.objectReferenceValue == null)
                {
                    uObjectProperty.objectReferenceValue = null;
                }else 
                {
                    if (uObjectProperty.objectReferenceValue == null)
                    {
                        uObjectProperty.objectReferenceValue = gObjectProperty.objectReferenceValue;
                    }
                    EditorGUI.BeginDisabledGroup(true);
                    {
                        EditorGUI.PropertyField(propertyDrawRect, uObjectProperty);
                    }
                    EditorGUI.EndDisabledGroup();
                    List<string> names = new List<string>();
                    List<UnityObject> objects = new List<UnityObject>();
                    names.Add("GameObject");
                    objects.Add(gObjectProperty.objectReferenceValue);

                    GameObject gObject = (GameObject)gObjectProperty.objectReferenceValue;
                    Component[] gComponents = gObject.GetComponents<Component>();
                    foreach(var c in gComponents)
                    {
                        names.Add(c.GetType().Name);
                        objects.Add(c);
                    }

                    propertyDrawRect.y += propertyDrawRect.height;
                    uObjectProperty.objectReferenceValue = EGUI.DrawPopup<UnityObject>(propertyDrawRect, "Component", names.ToArray(), objects.ToArray(), uObjectProperty.objectReferenceValue);
                }

            }
        }
    }
}
