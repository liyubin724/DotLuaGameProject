using DotEngine.NativeDrawer.Property;

namespace DotEditor.NativeDrawer.Property
{
    [Binder(typeof(HideLabelAttribute))]
    public class HideLabelDrawer : PropertyLabelDrawer
    {
        public override string GetLabel()
        {
            return null;
        }
    }
}
