using UnityObject = UnityEngine.Object;

namespace DotEngine.Core.Extensions
{
    public static class UObjectExtension
    {
        public static bool IsValid(this UnityObject obj)
        {
            if(obj != null && !obj.Equals(null))
            {
                return true;
            }
            return false;
        }

        public static bool IsNull(this UnityObject obj)
        {
            if (obj == null || obj.Equals(null))
            {
                return true;
            }
            return false;
        }
    }
}
