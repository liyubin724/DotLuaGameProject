using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExtension
{
    public class ToolbarSearchField
    {
        private static readonly int FIELD_WIDTH = 160;
        private static readonly int FIELD_HEIGHT = 16;

        public string Text { get; set; } = string.Empty;
        public string[] Categories { get; set; } = new string[0];
        public int CategoryIndex { get; set; } = -1;

        private Action<string> onTextChanged = null;
        private Action<string> onCategoryChagned = null;

        public ToolbarSearchField(Action<string> textChangedCallback, Action<string> categoryChangedCallback)
        {
            onTextChanged = textChangedCallback;
            onCategoryChagned = categoryChangedCallback;
        }

        public void OnGUI(Rect rect)
        {
            int categoryIndex = CategoryIndex;
            if (Categories == null || Categories.Length == 0)
            {
                if (CategoryIndex >= 0)
                {
                    categoryIndex = -1;
                }
            }
            else
            {
                if (CategoryIndex < 0 || CategoryIndex >= Categories.Length)
                {
                    categoryIndex = 0;
                }
            }
            if (categoryIndex != CategoryIndex)
            {
                CategoryIndex = categoryIndex;
                if (CategoryIndex >= 0)
                {
                    onCategoryChagned?.Invoke(Categories[CategoryIndex]);
                }
            }

            Rect textFieldRect;
            if (Categories == null || Categories.Length == 0)
            {
                textFieldRect = new Rect(rect.x, rect.y + 2, rect.width - 16, 14);
            }
            else
            {
                textFieldRect = new Rect(rect.x + 36, rect.y + 2, rect.width - 48, 14);
            }
            string searchText = UnityEngine.GUI.TextField(textFieldRect, Text, "toolbarSeachTextField");
            if (searchText != Text)
            {
                Text = searchText;
                onTextChanged?.Invoke(Text);
            }

            if (Categories != null && Categories.Length > 0)
            {
                Rect popRect = new Rect(rect.x, rect.y + 2, 48, 16);
                using (new UnityEngine.GUI.ClipScope(popRect))
                {
                    int index = EditorGUI.Popup(new Rect(0, 0, 48, 16), "", CategoryIndex, Categories, "ToolbarSeachTextFieldPopup");
                    if (index != CategoryIndex)
                    {
                        CategoryIndex = index;

                        onCategoryChagned?.Invoke(Categories[CategoryIndex]);
                    }
                }
            }

            Rect cancelRect = new Rect(rect.x + rect.width - 16, rect.y + 2, 16, 14);
            if (UnityEngine.GUI.Button(cancelRect, "", "ToolbarSeachCancelButton"))
            {
                if (Text != "")
                {
                    Text = "";
                    onTextChanged?.Invoke(Text);
                }
            }
        }

        public void OnGUILayout()
        {
            Rect rect = GUILayoutUtility.GetRect(FIELD_WIDTH, FIELD_HEIGHT, "toolbar");
            OnGUI(rect);
        }
    }
}
