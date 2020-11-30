using DotEngine.NativeDrawer.Property;

namespace DotEditor.NativeDrawer.Property
{
    [AttrBinder(typeof(HideLabelAttribute))]
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
