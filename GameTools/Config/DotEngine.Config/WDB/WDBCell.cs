namespace DotEngine.Config.WDB
{
    public class WDBCell
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public string Value { get; private set; }

        public WDBCell(int row, int col, string value)
        {
            Row = row;
            Col = col;
            Value = value;
        }

        public string GetValue(WDBField field)
        {
            string result = Value;
            if (string.IsNullOrEmpty(result))
            {
                result = field.DefaultValue;
            }
            return result;
        }

        public override string ToString()
        {
            return $"{{row = {Row},col = {Col},value = {(string.IsNullOrEmpty(Value) ? "" : Value)}}}";
        }
    }
}
