namespace DotEngine.Config.WDB
{
    public abstract class WDBCellValidation :WDBValidation
    {
        protected WDBField GetField(WDBContext context)
        {
            return context.Get<WDBField>(WDBContextKey.CURRENT_FIELD_NAME);
        }

        protected WDBCell GetCell(WDBContext context)
        {
            return context.Get<WDBCell>(WDBContextKey.CURRENT_CELL_NAME);
        }
    }
}
