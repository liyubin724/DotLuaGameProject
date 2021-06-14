namespace DotEngine.WDB
{
    public class WDBCell
    {
        private int row;
        private int col;
        private string value;

        public int Row => row;
        public int Col => col;
        public string Value => value;

        public WDBCell(int row,int col,string value)
        {
            this.row = row;
            this.col = col;
            this.value = value;
        }

        public override string ToString()
        {
            return $"{{row = {row},col = {col},value = {(string.IsNullOrEmpty(value) ? "" : value)}}}";
        }
    }
}
