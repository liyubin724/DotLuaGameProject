using System;
using UnityEngine;

namespace DotEngine.NativeDrawer.Decorator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class SeparatorLineAttribute : DecoratorAttribute
    {
        public Color Color { get; private set; }
        public float Thickness { get; private set; }
        public float Padding { get; private set; }

        public SeparatorLineAttribute(Color color ,float thickness = 0.75f,float padding = 3.0f)
        {
            Color = color;
            Thickness = thickness;
            Padding = padding;
        }

        public SeparatorLineAttribute(float thickness = 0.75f, float padding = 3.0f) : this(Color.grey,thickness,padding)
        {
        }
    }
}
