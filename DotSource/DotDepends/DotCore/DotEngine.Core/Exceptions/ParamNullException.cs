using System;

namespace DotEngine.Core.Exceptions
{
    public class ParamNullException : Exception
    {
        public ParamNullException():base("the param is null") { }
    }
}
