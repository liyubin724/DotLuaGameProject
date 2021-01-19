namespace DotEngine.GUIExt.NativeDrawer
{
    public enum LayoutOccasion
    {
        Before = 0,
        After,
    }

    public abstract class NLayoutAttribute : NDrawerAttribute
    {
        public LayoutOccasion Occasion { get; private set; }

        protected NLayoutAttribute(LayoutOccasion occasion = LayoutOccasion.Before)
        {
            Occasion = occasion;
        }
    }
}
