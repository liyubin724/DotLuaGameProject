namespace DotTool.ETD.Fields
{
    public class RefField : IntField
    {
        public string RefName { get; private set; }

        public RefField(int c, string n, string d, string t, string p, string v, string r) : base(c, n, d, t, p, v, r)
        {
            if(string.IsNullOrEmpty(defaultValue))
            {
                defaultValue = "-1";
            }
            RefName = FieldTypeUtil.GetRefName(Type);
        }

        protected override string GetDefaultValidation()
        {
            return "int";
        }
    }
}
