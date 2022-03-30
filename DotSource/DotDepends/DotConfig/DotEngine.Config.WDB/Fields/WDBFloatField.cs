using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Config.WDB
{
    [CustomField("float")]
    public class WDBFloatField : WDBField
    {
        public WDBFloatField(int column, string type) : base(column, type)
        {
        }

        protected override string GetTypeDefaultContent()
        {
            return "0.0";
        }

        protected override string[] GetTypeDefaultValidations()
        {
            return new string[] { WDBValidationNames.FLOAT_NAME };
        }
    }
}
