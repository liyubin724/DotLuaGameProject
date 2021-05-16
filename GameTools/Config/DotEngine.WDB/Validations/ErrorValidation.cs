using System;

namespace DotEngine.WDB
{
    public class ErrorValidation : WDBValidation
    {
        protected override string[] DoVerify(WDBSheet sheet, WDBField field, WDBCell cell)
        {
            throw new NotImplementedException();
        }
    }
}
