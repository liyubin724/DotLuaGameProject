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
        public List<NativeDrawerProperty> drawerProperties = new List<NativeDrawerProperty>();
    }

    public class NativeDrawerObject
    {
        public bool IsShowScroll { get; set; } = false;

        private object drawerObject;
        private List<TypeDrawerProperty> typeDrawerProperties = new List<TypeDrawerProperty>();

        public NativeDrawerObject(object obj)
        {
            drawerObject = obj;

            InitField();
        }

        private void InitField()
        {
            Type[] allTypes = NativeDrawerUtility.GetAllBaseTypes(drawerObject.GetType());
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
                        NativeDrawerProperty drawerProperty = new NativeDrawerProperty(drawerObject, field);
                        typeDrawerProperty.drawerProperties.Add(drawerProperty);
                    }

                    typeDrawerProperties.Add(typeDrawerProperty);
                }
            }
        }

        private Vector2 scrollPos = Vector2.zero;

        public void OnGUILayout()
        {
            if(IsShowScroll)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos,GUILayout.ExpandHeight(false));
            }
            EditorGUILayout.BeginVertical();
            {
                foreach(var typeDrawProperty in typeDrawerProperties)
                {
                    EGUILayout.DrawHorizontalSpace(10);
                    EGUILayout.DrawBoxHeader(typeDrawProperty.type.Name, GUILayout.ExpandWidth(true));
                    EGUILayout.DrawHorizontalLine();
                    foreach (var property in typeDrawProperty.drawerProperties)
                    {
                        property.OnGUILayout();
                    }
                }
            }
            EditorGUILayout.EndVertical();
            
            if(IsShowScroll)
            {
                EditorGUILayout.EndScrollView();
            }

            if(drawerObject!=null && typeof(UnityEngine.Object).IsAssignableFrom(drawerObject.GetType()))
            {
                if(GUI.changed)
                {
                    EditorUtility.SetDirty((UnityEngine.Object)drawerObject);
                }
            }
        }
    }
}
