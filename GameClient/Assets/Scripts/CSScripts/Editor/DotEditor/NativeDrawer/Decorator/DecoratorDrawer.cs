using DotEngine.NativeDrawer.Decorator;

namespace DotEditor.NativeDrawer.Decorator
{
    public abstract class DecoratorDrawer : AttrDrawer
    {
        public DrawerProperty DrawerProperty { get; private set; }

        protected DecoratorDrawer(DrawerProperty property, DecoratorAttribute attr) : base(attr)
        {
            DrawerProperty = property;
        }

        public abstract void OnGUILayout();
    }
}
