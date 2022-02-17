using UnityObject = UnityEngine.Object;

namespace DotEngine.Utilities
{
    public static class UObjectUtility
    {
        public static bool IsValid(this UnityObject obj)
        {
            if (obj != null && !obj.Equals(null))
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
