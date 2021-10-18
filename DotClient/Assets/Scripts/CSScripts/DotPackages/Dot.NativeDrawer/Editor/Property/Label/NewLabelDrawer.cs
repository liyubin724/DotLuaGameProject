using DotEngine.NativeDrawer.Property;

namespace DotEditor.NativeDrawer.Property
{
    [Binder(typeof(NewLabelAttribute))]
    public class NewLabelDrawer : PropertyLabelDrawer
    {
        public override string GetLabel()
        {
            NewLabelAttribute attr = GetAttr<NewLabelAttribute>();
            return attr.Label;
        }
    }
}
