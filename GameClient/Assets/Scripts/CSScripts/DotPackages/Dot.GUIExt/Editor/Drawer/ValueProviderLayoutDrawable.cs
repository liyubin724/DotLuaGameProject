using System;

namespace DotEditor.GUIExt
{
    public abstract class ValueProviderLayoutDrawable<T> : LayoutDrawable, IValueProvider<T>
    {
        private T value = default;
        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value == null || !this.value.Equals(value))
                {
                    this.value = value;
                    OnValueChanged?.Invoke(this.value);
                }
            }
        }

        public Action<T> OnValueChanged { get; set; } = null;
    }
}
