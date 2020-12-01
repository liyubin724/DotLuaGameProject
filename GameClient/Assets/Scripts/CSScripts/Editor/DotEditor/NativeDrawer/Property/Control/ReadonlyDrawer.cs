using DotEngine.NativeDrawer.Property;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [Binder(typeof(ReadonlyAttribute))]
    public class ReadonlyDrawer : PropertyControlDrawer
    {
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
