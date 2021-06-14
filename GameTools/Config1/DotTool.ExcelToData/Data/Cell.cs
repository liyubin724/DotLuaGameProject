namespace DotTool.ETD.Data
{
    public class Cell
    {
        private int row;
        private int col;
        private string content;

        public int Row { get => row; }
        public int Col { get => col; }

        public Cell(int r,int c,string v)
        {
            this.row = r;
            this.col = c;
            this.content = v;
        }

        public string GetContent(Field field)
        {
            return string.IsNullOrEmpty(content) ? field.DefaultValue : content;
        }

        public override string ToString()
        {
            return $"<row:{row},col:{col},value:{(string.IsNullOrEmpty(content) ? "" : content)}>";
        }
    }
}
