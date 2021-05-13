using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.WDB
{
    [WDBFieldLink(WDBFieldType.Id)]
    public class IdField : WDBField
    {
        public IdField(int col) : base(col)
        {
        }

        protected override string[] GetInnerValidationRule()
        {
            return new string[] { "int", "NotEmpty"};
        }
    }
}
