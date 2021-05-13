namespace DotEngine.WDB
{
    public abstract class WDBValidation
    {
        public string Rule { get; private set; }

        public WDBValidation(string rule)
        {
            Rule = rule;
        }

        public abstract bool Verify(out string[] errors);
    }
}
