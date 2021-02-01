using DotEngine.AI.BD.Tracks;
using System;

namespace DotEngine.AI.BD
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class NodeTrackAttribute : Attribute
    {
        public NodeTrackCategory Category { get; private set; }

        public NodeTrackAttribute(NodeTrackCategory category)
        {
            Category = category;
        }
    }
}
