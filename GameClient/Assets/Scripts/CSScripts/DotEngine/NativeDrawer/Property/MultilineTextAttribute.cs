using System;

namespace DotEngine.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MultilineTextAttribute : PropertyDrawerAttribute
    {
        public int LineCount { get; private set; }
        public MultilineTextAttribute(int lineCount = 4)
        {
            LineCount = lineCount;
        }
    }
}
