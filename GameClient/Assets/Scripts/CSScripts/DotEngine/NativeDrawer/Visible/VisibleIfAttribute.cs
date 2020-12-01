namespace DotEngine.NativeDrawer.Visible
{
    public class VisibleIfAttribute : VisibleAtrribute
    {
        public string VisibleMemberName { get; private set; }

        public VisibleIfAttribute(string memberName)
        {
            VisibleMemberName = memberName;
        }
    }
}
