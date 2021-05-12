using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Config.WDB
{
    [WDBFieldLink(WDBFieldType.Int)]
    public class IntField : WDBField
    {
        protected override string GetInnerDefaultValue()
        {
            return "0";
        }
    }
}
