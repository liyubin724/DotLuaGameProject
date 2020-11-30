using DotEngine.NativeDrawer.Property;

namespace DotEditor.NativeDrawer.Property
{
    [AttrBinder(typeof(NewLabelAttribute))]
    public class NewLabelDrawer : PropertyLabelDrawer
    {
        public override string GetLabel()
        {
            NewLabelAttribute attr = GetAttr<NewLabelAttribute>();
            return attr.Label;
        }
    }
}
