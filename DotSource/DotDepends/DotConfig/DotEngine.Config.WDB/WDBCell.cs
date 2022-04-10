namespace DotEngine.Config.WDB
{
    public class WDBCell : IWDBValidationChecker
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

        public void Check(WDBContext context)
        {
            WDBField field = context.Get<WDBField>(WDBContextKey.CURRENT_FIELD_NAME);
            context.Add(WDBContextKey.CURRENT_CELL_NAME, this);
            {
                var cellValidations = field.GetValidations();
                for(int i =0;i<cellValidations.Length;i++)
                {
                    cellValidations[i].Verify(context);
                }
            }
            context.Remove(WDBContextKey.CURRENT_CELL_NAME);
        }
    }
}
