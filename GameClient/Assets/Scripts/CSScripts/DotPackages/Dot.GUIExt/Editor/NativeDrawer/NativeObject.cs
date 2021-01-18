using DotEditor.GUIExt.Layout;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    class TypeFieldInfo
    {
        public Type type;
        public List<NativeField> fields = new List<NativeField>();
    }

    public class NativeObject : ILayoutDrawable
    {
        public bool IsShowScroll { get; set; } = true;
        public bool IsShowInherit { get; set; } = true;

        public SystemObject Target { get; private set; }
        private List<TypeFieldInfo> typeFields = null;

        public NativeObject(SystemObject target)
        {
            Target = target;
        }

        private Vector2 scrollPos = Vector2.zero;
        public void OnGUILayout()
        {
            if(typeFields == null)
            {
                InitFields();
            }
            if (IsShowScroll)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(false));
            }
            EditorGUILayout.BeginVertical();
            {
                if (typeFields.Count == 0)
                {
                    if (Target == null)
                    {
                        EditorGUILayout.LabelField("Target is Null.");
                    }
                    else
                    {
                        if (TypeUtility.IsPrimitiveType(Target.GetType()))
                        {
                            DrawPrimitiveObject();
                        }
                        else if (TypeUtility.IsArrayOrList(Target.GetType()))
                        {
                            DrawListObject();
                        }
                        else
                        {
                            EditorGUILayout.LabelField($"Unkown Object.type={Target.GetType()}");
                        }
                    }
                }
                else
                {
                    DrawObject();
                }
            }
            EditorGUILayout.EndVertical();
            
            if (IsShowScroll)
            {
                EditorGUILayout.EndScrollView();
            }

            if(Target!=null && typeof(UnityObject).IsAssignableFrom(Target.GetType()))
            {
                if(GUI.changed)
                {
                    EditorUtility.SetDirty((UnityObject)Target);
                }
            }
        }

        private void DrawListObject()
        {

        }

        private void DrawPrimitiveObject()
        {

        }

        private void DrawObject()
        {
            foreach(var typeFieldInfo in typeFields)
            {
                if (IsShowInherit)
                {
                    EGUILayout.DrawBoxHeader(typeFieldInfo.type.Name, GUILayout.ExpandWidth(true));
                }

                foreach(var field in typeFieldInfo.fields)
                {
                    field.OnGUILayout();
                }

                if (IsShowInherit)
                {
                    EGUILayout.DrawHorizontalLine();
                }
            }
        }

        private void InitFields()
        {
            typeFields = new List<TypeFieldInfo>();
            if(Target ==null)
            {
                return;
            }
            Type[] allTypes = NativeUtility.GetAllBaseTypes(Target.GetType());
            if(allTypes!=null && allTypes.Length>0)
            {
                foreach (var type in allTypes)
                {
                    TypeFieldInfo typeFieldInfo = new TypeFieldInfo
                    {
                        type = type
                    };
                    typeFields.Add(typeFieldInfo);

                    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                    foreach (var field in fields)
                    {
                        NativeField nativeField = new NativeField(field, Target);
                        typeFieldInfo.fields.Add(nativeField);
                    }
                }
            }
        }
    }
}
