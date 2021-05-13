using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.WDB
{
    [WDBFieldLink(WDBFieldType.Ref)]
    public class RefField : WDBField
    {
        public RefField(int col) : base(col)
        {
        }
    }
}
