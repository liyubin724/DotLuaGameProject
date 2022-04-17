using System;

namespace DotEngine.NativeDrawer.Decorator
{
    public enum SpaceDirection
    {
        Horizontal = 0,
        Vertical,
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class SpaceAttribute : DecoratorAttribute
    {
        public SpaceDirection Direction { get; private set; }
        public float Size { get; private set; }

        public SpaceAttribute(float size = 10.0f,SpaceDirection direction = SpaceDirection.Horizontal)
        {
            Direction = direction;
            Size = size;
        }
    }
}
