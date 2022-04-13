using System;

namespace DotEditor.GUIExt.IMGUI
{
    public class EnumButtonDrawer : ValueProviderLayoutDrawable<Enum>
    {
        public Type EnumType { get; private set; }

        public EnumButtonDrawer(Type enumType)
        {
            EnumType = enumType;
        }

        protected override void OnLayoutDraw()
        {
            Enum enumValue = (Enum)Enum.ToObject(EnumType, Value);
            Value = (Enum)EGUILayout.DrawEnumButton(Text, enumValue, LayoutOptions);
        }
    }
}
