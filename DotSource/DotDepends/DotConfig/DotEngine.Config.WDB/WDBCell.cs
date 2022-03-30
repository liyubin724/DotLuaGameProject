namespace DotEngine.Config.WDB
{
    public class WDBCell
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public string Content { get; set; }

        public WDBCell(int row,int column)
        {
            Row = row;
            Column = column;
        }

        public string GetContent(WDBField field)
        {
            string content = Content;
            if (string.IsNullOrEmpty(content))
            {
                content = field.GetContent();
            }

            return content;
        }

        public override string ToString()
        {
            return $"{{row = {Row},column = {Column},content = {Content}}}";
        }
    }
}
