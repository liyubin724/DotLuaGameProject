using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Config.WDB
{
    public class WDBDateTimeCellValidation : WDBCellValidation
    {
        public override void Verify(WDBContext context)
        {
            WDBField field = GetField(context);
            WDBCell cell = GetCell(context);

            string cellContent = cell.GetContent(field);
            if (!DateTime.TryParse(cellContent, out _))
            {
                context.AppendError(string.Format(WDBErrorMessages.CELL_CONTENT_CONVERT_ERROR, cellContent, cell.Row, cell.Column, typeof(DateTime)));
            }
        }
    }
}
