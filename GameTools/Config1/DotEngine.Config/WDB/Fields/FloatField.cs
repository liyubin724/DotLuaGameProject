using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.WDB
{

    [WDBFieldLink(WDBFieldType.Float)]
    public class FloatField : WDBField
    {
        public FloatField(int col) : base(col)
        {
        }

        protected override string GetInnerDefaultValue()
        {
            return "0.0";
        }
    }
}
