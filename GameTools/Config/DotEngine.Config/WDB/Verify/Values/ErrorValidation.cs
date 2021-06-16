using System;

namespace DotEngine.Config.WDB
{
    public class ErrorValidation : WDBValueValidation
    {
        protected override bool DoVerify()
        {
            return false;
        }
    }
}
