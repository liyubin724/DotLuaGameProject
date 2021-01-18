namespace DotEngine.EGUIP.NativeDrawer
{
    public enum LayoutOccasion
    {
        Before = 0,
        After,
    }

    public abstract class LayoutAttribute : DrawableAttribute
    {
        public LayoutOccasion Occasion { get; private set; }

        protected LayoutAttribute(LayoutOccasion occasion = LayoutOccasion.Before)
        {
            Occasion = occasion;
        }
    }
}
