using System;

namespace DotEditor.GUIExt.IMGUI
{
    public interface IValueProvider<T>
    {
        Action<T> OnValueChanged { get; set; }
        T Value { get; set; }
    }
}
