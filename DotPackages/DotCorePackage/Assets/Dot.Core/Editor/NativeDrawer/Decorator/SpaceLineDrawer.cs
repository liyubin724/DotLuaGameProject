using DotEngine.NativeDrawer.Decorator;
using UnityEditor;

namespace DotEditor.NativeDrawer.Decorator
{
    [CustomAttributeDrawer(typeof(SpaceLineAttribute))]
    public class SpaceLineDrawer : DecoratorDrawer
    {
        public SpaceLineDrawer(NativeDrawerProperty property, DecoratorAttribute attr) : base(property, attr)
        {
        }

        public override void OnGUILayout()
        {
            EditorGUILayout.Space();
        }
    }
}
