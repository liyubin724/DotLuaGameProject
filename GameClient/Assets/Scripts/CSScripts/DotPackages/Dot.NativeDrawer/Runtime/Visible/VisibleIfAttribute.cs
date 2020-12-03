namespace DotEngine.NativeDrawer.Visible
{
    public class VisibleIfAttribute : VisibleAtrribute
    {
        public string MemberName { get; private set; }

        public VisibleIfAttribute(string memberName)
        {
            MemberName = memberName;
        }
    }
}
