using DotEditor.GUIExtension;
using DotEditor.NativeDrawer;
using UnityEditor;
using UnityEngine;
using static DotEditor.Fonts.BitmapFontConfig;

namespace DotEditor.Fonts
{
    [CustomTypeDrawer(typeof(BitmapFontChar))]
    public class BitmapFontCharDrawer : TypeDrawer
    {
        public BitmapFontCharDrawer(DrawerProperty property) : base(property)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(BitmapFontChar);
        }

        protected override void OnDrawProperty(string label)
        {
            BitmapFontChar value = DrawerProperty.GetValue<BitmapFontChar>();
            label = $"Font {(label ?? "")} ({value.fontName})";
            EditorGUI.BeginChangeCheck();
            {
                value.fontName = EditorGUILayout.TextField("Font Name", value.fontName);
                value.charSpace = EditorGUILayout.IntField("Char Space", value.charSpace);
                EditorGUILayout.BeginVertical(EGUIStyles.BoxStyle);
                {
                    EditorGUILayout.LabelField(GUIContent.none, EditorStyles.toolbar, GUILayout.ExpandWidth(true));

                    Rect lastRect = GUILayoutUtility.GetLastRect();
                    EditorGUI.LabelField(lastRect, label, EGUIStyles.BoldLabelStyle);

                    Rect clearBtnRect = new Rect(lastRect.x + lastRect.width - 40, lastRect.y, 40, lastRect.height);
                    if (GUI.Button(clearBtnRect, "C", EditorStyles.toolbarButton))
                    {
                        DrawerProperty.ClearArrayElement();
                        value.chars.Clear();
                        value.textures.Clear();
                    }

                    for (int i = 0; i < value.chars.Count; ++i)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.BeginVertical();
                            {
                                char c = value.chars[i];
                                string charText = EditorGUILayout.TextField("Char", "" + c);
                                if(!string.IsNullOrEmpty(charText) && charText[0]!=c)
                                {
                                    value.chars[i] = charText[0];
                                }
                                value.textures[i] = (Texture2D)EditorGUILayout.ObjectField("Texture", value.textures[i], typeof(Texture2D), false);
                            }
                            EditorGUILayout.EndVertical();

                            if (GUILayout.Button("-", GUILayout.Width(20)))
                            {
                                value.chars.RemoveAt(i);
                                value.textures.RemoveAt(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        EGUILayout.DrawHorizontalLine();
                    }
                    Rect addBtnRect = GUILayoutUtility.GetRect(lastRect.width, 20);
                    addBtnRect.x += addBtnRect.width - 40;
                    addBtnRect.width = 40;
                    if (GUI.Button(addBtnRect, "+"))
                    {
                        value.chars.Add('-');
                        value.textures.Add(null);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
