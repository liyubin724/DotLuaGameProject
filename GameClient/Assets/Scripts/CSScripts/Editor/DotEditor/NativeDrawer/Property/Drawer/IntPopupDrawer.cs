using DotEditor.GUIExtension;
using DotEngine.NativeDrawer.Property;
using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    [Binder(typeof(IntPopupAttribute))]
    public class IntPopupDrawer : PropertyContentDrawer
    {
        private static EditorWindow lastSearchableWindow;

        protected override bool IsValidProperty()
        {
            return typeof(int) == Property.ValueType;
        }

        protected override void OnDrawProperty(string label)
        {
            var attr = GetAttr<IntPopupAttribute>();

            string[] contents = attr.Contents;
            if (!string.IsNullOrEmpty(attr.ContentMemberName))
            {
                contents = DrawerUtility.GetMemberValue<string[]>(attr.ContentMemberName, Property.Target);
            }

            int[] values = attr.Values;
            if(!string.IsNullOrEmpty(attr.ValueMemberName))
            {
                values = DrawerUtility.GetMemberValue<int[]>(attr.ValueMemberName, Property.Target);
            }

            var value = Property.GetValue<int>();
            var valueIndex = Array.IndexOf(values, value);
            if(valueIndex<0 && values!=null && values.Length>0)
            {
                valueIndex = 0;
                value = values[0];

                Property.Value = value;
            }    

            label = label ?? "";

            if (attr.IsSearchable)
            {
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
                                Property.Value = values[selected];
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
                    Property.Value = value;
                }
            }

        }
    }
}
