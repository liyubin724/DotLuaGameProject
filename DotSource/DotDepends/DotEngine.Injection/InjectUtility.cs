using System;
using System.Linq;
using System.Reflection;

namespace DotEngine.Injection
{
    static class InjectUtility
    {
        public static bool IsCastableTo(this Type from, Type to)
        {
            if(from == null || to == null)
            {
                return false;
            }

            if (to.IsAssignableFrom(from))
            {
                return true;
            }

            var methods = from.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(
                    m => m.ReturnType == to &&
                    (m.Name == "op_Implicit" ||
                        m.Name == "op_Explicit")
                );
            return methods.Count() > 0;
        }
    }
}
