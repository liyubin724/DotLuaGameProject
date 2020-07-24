using DotEngine.NativeDrawer.Property;
using DotEditor.GUIExtension;
using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttributeDrawer(typeof(StringPopupAttribute))]
    public class StringPopupDrawer : PropertyDrawer
    {
        private static EditorWindow lastSearchableWindow;

        public StringPopupDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
        }

        protected override bool IsValidProperty()
        {
            return typeof(string) == DrawerProperty.ValueType;
        }

        protected override void OnDrawProperty(string label)
        {
            var attr = GetAttr<StringPopupAttribute>();
            
            string[] options = attr.Options;
            if (!string.IsNullOrEmpty(attr.MemberName))
            {
                options = NativeDrawerUtility.GetMemberValue<string[]>(attr.MemberName, DrawerProperty.Target);
            }

            var value = DrawerProperty.GetValue<string>();

            label = label ?? "";

            if(attr.IsSearchable)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel(label);
                    Rect btnRect = GUILayoutUtility.GetRect(new GUIContent(value), "dropdownbutton");

                    if (EditorGUI.DropdownButton(btnRect,new GUIContent(value), FocusType.Keyboard))
                    {
                        try
                        {
                            SearchablePopup.Show(btnRect,new Vector2(200,400) ,Array.IndexOf(options, value), options, (selected) =>
                            {
                                DrawerProperty.Value = options[selected];
                            });
                        }
                        catch (ExitGUIException)
                        {
                            lastSearchableWindow = EditorWindow.focusedWindow;
                            throw;
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (lastSearchableWindow && lastSearchableWindow != EditorWindow.mouseOverWindow)
                {
                    if (Event.current.type == EventType.ScrollWheel)
                        Event.current.Use();
                }
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                {
                    value = EGUILayout.DrawPopup<string>(label, options, options, value);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    DrawerProperty.Value = value;
                }
            }
        }
    }
}
