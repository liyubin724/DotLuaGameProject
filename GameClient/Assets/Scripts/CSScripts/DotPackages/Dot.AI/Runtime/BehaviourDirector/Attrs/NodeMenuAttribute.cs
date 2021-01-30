using System;

namespace DotEngine.AI.BD
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class NodeMenuAttribute : Attribute
    {
        public string Prefix { get; private set; }
        public string Name { get; private set; }

        public NodeMenuAttribute(string name,string prefix)
        {
            Name = name;
            Prefix = prefix;
        }
    }
}