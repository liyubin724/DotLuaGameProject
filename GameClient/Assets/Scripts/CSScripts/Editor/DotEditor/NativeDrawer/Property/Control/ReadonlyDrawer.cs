using DotEngine.NativeDrawer.Property;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [AttrDrawBinder(typeof(ReadonlyAttribute))]
    public class ReadonlyDrawer : PropertyControlDrawer
    {
        public ReadonlyDrawer(PropertyControlAttribute attr) : base(attr)
        {
        }

        public override void OnStartGUILayout()
        {
            EditorGUI.BeginDisabledGroup(true);
        }

        public override void OnEndGUILayout()
        {
            EditorGUI.EndDisabledGroup();
        }
    }
}
