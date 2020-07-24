using DotEngine.NativeDrawer.Property;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttributeDrawer(typeof(NewLabelAttribute))]
    public class NewLabelDrawer : PropertyLabelDrawer
    {
        public NewLabelDrawer(PropertyLabelAttribute attr) : base(attr)
        {
        }

        public override string GetLabel()
        {
            NewLabelAttribute attr = GetAttr<NewLabelAttribute>();
            return attr.Label;
        }
    }
}
