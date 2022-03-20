namespace DotEngine.FSM
{
    [CustomFSCondition("Always True", "")]
    public class FSTrueCondition : IFSCondition
    {
        public bool IsSatisfy()
        {
            return true;
        }
    }
}
