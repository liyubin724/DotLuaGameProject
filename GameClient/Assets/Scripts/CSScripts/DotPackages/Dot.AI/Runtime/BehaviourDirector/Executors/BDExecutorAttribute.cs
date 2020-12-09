using System;

namespace DotEngine.BD.Executors
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class BDExecutorAttribute : Attribute
    {
        public Type BindDataType { get; private set; }

        public BDExecutorAttribute(Type dataType)
        {
            BindDataType = dataType;
        }
    }
}
