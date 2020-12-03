using UnityObject = UnityEngine.Object;

namespace DotEngine.Utility
{
    public static class UnityObjectUtility
    {
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
