using System;

namespace DotEditor.GUIExt.Layout
{
    public class EnumButtonDrawer : ValueProviderLayoutDrawable<int>
    {
        public Type EnumType { get; private set; }

        public EnumButtonDrawer(Type enumType)
        {
            EnumType = enumType;
        }

        protected override void OnLayoutDraw()
        {
            string enumName = Enum.GetName(EnumType, Value);

            Value = (int)EGUILayout.DrawEnumButton(Text,(Enum)Enum.Parse(EnumType,enumName), LayoutOptions);
        }
    }
}
