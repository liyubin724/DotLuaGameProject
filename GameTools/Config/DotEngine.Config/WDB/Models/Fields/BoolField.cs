namespace DotEngine.Config.WDB
{
    [WDBFieldLink(WDBFieldType.Bool)]
    public class BoolField : WDBField
    {
        protected override string GetInnerDefaultValue()
        {
            return "false";
        }
    }
}
