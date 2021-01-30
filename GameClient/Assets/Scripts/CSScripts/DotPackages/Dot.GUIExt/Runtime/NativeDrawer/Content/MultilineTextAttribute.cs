using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MultilineTextAttribute : ContentAttribute
    {
        public int LineCount { get; private set; }

        public MultilineTextAttribute(int lineCount = 4)
        {
            LineCount = lineCount;
        }
    }
}
