using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.WDB
{
    [WDBFieldLink(WDBFieldType.Int)]
    public class IntField : WDBField
    {
        public IntField(int col) : base(col)
        {
        }

        protected override string GetInnerDefaultValue()
        {
            return "0";
        }
    }
}
