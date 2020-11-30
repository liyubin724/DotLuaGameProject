using DotEditor.GUIExtension;
using DotEngine.NativeDrawer.Property;
using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    [AttrBinder(typeof(IntPopupAttribute))]
    public class IntPopupDrawer : PropertyDrawer
    {
        private static EditorWindow lastSearchableWindow;

        public IntPopupDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
        }

        protected override bool IsValidProperty()
        {
            return typeof(int) == DrawerProperty.ValueType;
        }

        protected override void OnDrawProperty(string label)
        {
            var attr = GetAttr<IntPopupAttribute>();

            string[] contents = attr.Contents;
            if (!string.IsNullOrEmpty(attr.ContentMemberName))
            {
                contents = NativeDrawerUtility.GetMemberValue<string[]>(attr.ContentMemberName, DrawerProperty.Target);
            }

            int[] values = attr.Values;
            if(!string.IsNullOrEmpty(attr.ValueMemberName))
            {
                values = NativeDrawerUtility.GetMemberValue<int[]>(attr.ValueMemberName, DrawerProperty.Target);
            }

            var value = DrawerProperty.GetValue<int>();

            label = label ?? "";

            if (attr.IsSearchable)
            {
                var valueIndex = Array.IndexOf(values, value);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel(label);
                    Rect btnRect = GUILayoutUtility.GetRect(new GUIContent(contents[valueIndex]), "dropdownbutton");

                    if (EditorGUI.DropdownButton(btnRect, new GUIContent(contents[valueIndex]), FocusType.Keyboard))
                    {
                        try
                        {
                            SearchablePopup.Show(btnRect, new Vector2(200, 400), valueIndex, contents, (selected) =>
                            {
                                DrawerProperty.Value = values[selected];
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
                    value = EGUILayout.DrawPopup<int>(label, contents, values, value);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    DrawerProperty.Value = value;
                }
            }

        }
    }
}
