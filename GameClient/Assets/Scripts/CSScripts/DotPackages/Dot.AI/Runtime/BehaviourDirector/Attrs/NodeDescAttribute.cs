using System;

namespace DotEngine.AI.BD
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class NodeDescAttribute : Attribute
    {
        public string Brief { get; private set; }
        public string Detail { get; private set; }

        public NodeDescAttribute(string brief,string detail)
        {
            Brief = brief;
            Detail = detail;
        }
    }
}
