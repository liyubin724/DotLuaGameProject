using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class NAttrDrawer : NLayoutDrawer
    {
    }

    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class CustomAttrDrawerAttribute : Attribute
    {
    }
}
