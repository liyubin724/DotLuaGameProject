using System;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.EditorDrawer.Provider
{
    public interface IValueProvider
    {
        event Action<SystemObject> OnValueChanged;

        SystemObject Value { get; set; }
    }
}
