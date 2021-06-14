using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.WDB
{
    [WDBFieldLink(WDBFieldType.String)]
    public class StringField : WDBField
    {
        public StringField(int col) : base(col)
        {
        }

        protected override string GetInnerDefaultValue()
        {
            return string.Empty;
        }
    }
}
