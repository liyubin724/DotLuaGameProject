namespace DotEngine.Config.WDB
{
    public class WDBCell
    {
        private int row;
        private int col;
        public string value;

        public int Row => row;
        public int Col => col;

        public WDBCell(int row,int col,string value)
        {
            this.row = row;
            this.col = col;
            this.value = value;
        }
        
        public string GetContent(WDBField field)
        {
            return value;
        }

        public override string ToString()
        {
            return $"{{row = {row},col = {col},content = {(string.IsNullOrEmpty(value) ? "" : value)}}}";
        }
    }
}
