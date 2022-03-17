using System.Text;
using UnityEngine;

namespace DotEngine.Extensions
{
    public static class TransformExtension
    {
        public static Transform GetChild(this Transform tran, string name)
        {
            if (tran.name == name)
            {
                return tran;
            }

            for (int i = 0; i < tran.childCount; ++i)
            {
                Transform target = GetChild(tran.GetChild(i), name);
                if (target != null)
                {
                    return target;
                }
            }

            return null;
        }

        public static string GetPath(this Transform tran, Transform root = null)
        {
            StringBuilder nameStrBuilder = new StringBuilder();
            Transform parent = tran;
            while (parent != root && parent != null)
            {
                if (nameStrBuilder.Length > 0)
                {
                    nameStrBuilder.Insert(0, "/");
                }
                nameStrBuilder.Insert(0, parent.name);
                parent = tran.parent;
            }
            return nameStrBuilder.ToString();
        }
    }
}
