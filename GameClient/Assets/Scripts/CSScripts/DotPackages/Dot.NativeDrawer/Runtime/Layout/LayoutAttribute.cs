namespace DotEngine.NativeDrawer.Layout
{
    public enum LayoutOccasion
    {
        Before = 0,
        After,
    }

    public abstract class LayoutAttribute : DrawerAttribute
    {
        public LayoutOccasion Occasion { get; private set; }

        protected LayoutAttribute(LayoutOccasion occasion = LayoutOccasion.Before)
        {
            Occasion = occasion;
        }
    }
}
