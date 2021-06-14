using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.WDB
{
    [WDBFieldLink(WDBFieldType.Address)]
    public class AddressField : WDBField
    {
        public AddressField(int col) : base(col)
        {
        }
    }
}
