using DotEditor.GUIExtension;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer
{
    class TypeDrawerProperty
    {
        public Type type;
        public List<DrawerProperty> drawerProperties = new List<DrawerProperty>();
    }

    public class DrawerObject
    {
        public bool IsShowScroll { get; set; } = true;
        public bool IsShowInherit { get; set; } = true;
        public float LabelWidth { get; set; } = 100;
        public string TitleContent { get; set; } = null;

        private object drawerObject;
        private List<TypeDrawerProperty> typeDrawerProperties = new List<TypeDrawerProperty>();

        public DrawerObject(object obj)
        {
            drawerObject = obj;

            InitField();
        }

        private void InitField()
        {
            Type[] allTypes = DrawerUtility.GetAllBaseTypes(drawerObject.GetType());
            if(allTypes!=null)
            {
                foreach (var type in allTypes)
                {
                    TypeDrawerProperty typeDrawerProperty = new TypeDrawerProperty()
                    {
                        type = type,
                    };

                    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                    foreach (var field in fields)
                    {
                        DrawerProperty drawerProperty = new DrawerProperty(drawerObject, field);
                        typeDrawerProperty.drawerProperties.Add(drawerProperty);
                    }

                    typeDrawerProperties.Add(typeDrawerProperty);
                }
            }
        }

        private Vector2 scrollPos = Vector2.zero;
        public void OnGUILayout()
        {
            if(!string.IsNullOrEmpty(TitleContent))
            {
                EGUILayout.DrawBoxHeader(TitleContent, EGUIStyles.BoxedHeaderCenterStyle,GUILayout.ExpandWidth(true));
            }

            if(IsShowScroll)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(false));
            }
            EditorGUILayout.BeginVertical();
            {
                foreach(var typeDrawProperty in typeDrawerProperties)
                {
                    if(IsShowInherit)
                    {
                        EGUILayout.DrawBoxHeader(typeDrawProperty.type.Name, GUILayout.ExpandWidth(true));
                    }
                    EGUI.BeginLabelWidth(LabelWidth);
                    {
                        foreach (var property in typeDrawProperty.drawerProperties)
                        {
                            property.OnGUILayout();
                        }
                    }
                    EGUI.EndLableWidth();

                    if(IsShowInherit)
                    {
                        EGUILayout.DrawHorizontalLine();
                    }
                }
            }
            EditorGUILayout.EndVertical();
            
            if(IsShowScroll)
            {
                EditorGUILayout.EndScrollView();
            }

            if (drawerObject != null && typeof(UnityEngine.Object).IsAssignableFrom(drawerObject.GetType()))
            {
                if(GUI.changed)
                {
                    EditorUtility.SetDirty((UnityEngine.Object)drawerObject);
                }
            }
        }
    }
}
