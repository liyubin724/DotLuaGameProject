using System;

namespace DotEngine.BehaviourLine.Action
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ActionItemBindDataAttribute : Attribute
    {
        public Type DataType { get; set; }

        public ActionItemBindDataAttribute(Type dataType)
        {
            DataType = dataType;
        }
    }
}
