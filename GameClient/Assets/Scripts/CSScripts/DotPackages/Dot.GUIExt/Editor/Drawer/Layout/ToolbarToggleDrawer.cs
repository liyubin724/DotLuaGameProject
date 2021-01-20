using UnityEditor;

namespace DotEditor.GUIExt.Layout
{
    public class ToolbarToggleDrawer : ValueProviderLayoutDrawable<bool>
    {
        public ToolbarToggleDrawer()
        {
            LabelWidth = 120;
        }

        protected override void OnLayoutDraw()
        {
            Value = EditorGUILayout.ToggleLeft(Label, Value);
        }
    }
}
