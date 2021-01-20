using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.GUIExt.Layout
{
    public class SearchFieldDrawer : ValueProviderLayoutDrawable<string>, IStringFilter
    {
        private SearchField searchField = null;
        public bool IgnoreCase { get; set; } = true;
        public bool IsInToolbar { get; set; } = true;
        public bool IsExpandWidth { get; set; } = false;
        public float Width { get; set; } = 230.0f;

        public bool IsMatch(string value)
        {
            if(string.IsNullOrEmpty(Value))
            {
                return true;
            }

            if (value == null) return false;
            if(IgnoreCase)
            {
                return value.ToLower() == Value.ToLower();
            }else
            {
                return value == Value;
            }
        }

        protected override void OnLayoutDraw()
        {
            if(searchField == null)
            {
                Value = string.Empty;
                searchField = new SearchField();
            }

            if(IsInToolbar)
            {
                Value = searchField.OnToolbarGUI(Value,(IsExpandWidth?GUILayout.ExpandWidth(true):GUILayout.Width(Width)));
            }else
            {
                Value = searchField.OnGUI(Value, (IsExpandWidth ? GUILayout.ExpandWidth(true) : GUILayout.Width(Width)));
            }
        }
    }
}
