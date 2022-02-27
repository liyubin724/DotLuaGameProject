using System;

namespace DotEngine.AI.BT.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BTNodeSizeAttribute : Attribute
    {
        public float Width { get; private set; }
        public float Height { get; private set; }

        public BTNodeSizeAttribute(float width, float height)
        {
            Width = width;
            Height = height;
        }
    }
}
