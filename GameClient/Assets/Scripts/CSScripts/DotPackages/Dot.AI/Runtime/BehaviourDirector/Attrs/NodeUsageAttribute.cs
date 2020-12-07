using System;

namespace DotEngine.AI.BD
{
    public enum NodeCategory
    {
        None = 0,
        Action,
        Condition,
    }

    public enum NodePlatform
    {
        All = 0,
        Client,
        Server,
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class NodeUsageAttribute : Attribute
    {
        public NodeCategory Category { get; private set; }
        public NodePlatform Platform { get; private set; }

        public NodeUsageAttribute(NodeCategory category, NodePlatform platform)
        {
            Category = category;
            Platform = platform;
        }
    }
}
