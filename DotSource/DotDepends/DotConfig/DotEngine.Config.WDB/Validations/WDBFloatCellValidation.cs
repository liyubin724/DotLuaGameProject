namespace DotEngine.Config.WDB
{
    [CustomValidation(WDBValidationNames.FLOAT_NAME)]
    public class WDBFloatCellValidation : WDBCellValidation
    {
        public override void Verify(WDBContext context)
        {
            WDBField field = GetField(context);
            WDBCell cell = GetCell(context);

            string cellContent = cell.GetContent(field);
            if (!float.TryParse(cellContent, out _))
            {
                AddErrorMessage(context, string.Format(WDBErrorMessages.CELL_PARSE_ERROR, cellContent, cell.Row, cell.Column, typeof(float)));
            }
        }
    }
}
