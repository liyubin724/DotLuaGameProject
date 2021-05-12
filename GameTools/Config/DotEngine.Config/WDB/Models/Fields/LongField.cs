using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Config.WDB
{
    [WDBFieldLink(WDBFieldType.Long)]
    public class LongField : WDBField
    {
        protected override string GetInnerDefaultValue()
        {
            return "0";
        }
    }
}
