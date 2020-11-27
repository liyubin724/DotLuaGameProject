using DotEngine.NativeDrawer.Property;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttributeDrawer(typeof(HideLabelAttribute))]
    public class HideLabelDrawer : PropertyLabelDrawer
    {
        public HideLabelDrawer(PropertyLabelAttribute attr) : base(attr)
        {
        }

        public override string GetLabel()
        {
            return null;
        }
    }
}
