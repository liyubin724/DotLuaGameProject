using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    public enum CompareSymbol
    {
        Eq,
        Neq,
        Lt,
        Gt,
        Lte,
        Gte,
    }

    public abstract class DrawerAttribute : Attribute
    {
    }
}
