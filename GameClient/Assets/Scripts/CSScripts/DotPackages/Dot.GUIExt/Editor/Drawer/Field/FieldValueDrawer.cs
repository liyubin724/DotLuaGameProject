using UnityEngine;

namespace DotEditor.GUIExt.Field
{
    public abstract class FieldValueDrawer
    {
        public abstract float GetHeight();
        public abstract void OnGUI(Rect rect, string label, FieldValueProvider provider);
    }
}
