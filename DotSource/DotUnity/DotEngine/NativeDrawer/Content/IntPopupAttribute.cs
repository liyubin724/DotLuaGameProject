using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class IntPopupAttribute : ContentAttribute
    {
        public int[] Values { get; private set; }
        public string[] Contents { get; private set; }

        public string ValueMemberName { get; private set; }
        public string ContentMemberName { get; private set; }

        public bool IsSearchable { get; set; } = false;

        public IntPopupAttribute(string[] contents, int[] values)
        {
            Contents = contents;
            Values = values;
        }

        public IntPopupAttribute(string contentMemberName, string valueMemberName)
        {
            ContentMemberName = contentMemberName;
            ValueMemberName = valueMemberName;
        }
    }
}

