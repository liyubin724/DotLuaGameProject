using DotEngine.NativeDrawer.Decorator;

namespace DotEditor.NativeDrawer.Decorator
{
    public abstract class DecoratorDrawer : AttrNativeDrawer
    {
        public NativeDrawerProperty DrawerProperty { get; private set; }

        protected DecoratorDrawer(NativeDrawerProperty property,DecoratorAttribute attr) : base(attr)
        {
            DrawerProperty = property;
        }

        public abstract void OnGUILayout();
    }
}
