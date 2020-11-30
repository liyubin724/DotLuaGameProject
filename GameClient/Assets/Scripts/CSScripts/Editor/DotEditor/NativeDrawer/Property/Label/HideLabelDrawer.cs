using DotEngine.NativeDrawer.Property;

namespace DotEditor.NativeDrawer.Property
{
    [AttrBinder(typeof(HideLabelAttribute))]
    public class HideLabelDrawer : PropertyLabelDrawer
    {
        public override string GetLabel()
        {
            return null;
        }
    }
}
