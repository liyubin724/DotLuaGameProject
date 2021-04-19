using System;

namespace DotEngine.Utilities
{
    public static class ArrayUtility
    {
        public static void Add(ref Array array, object newone)
        {
            Insert(ref array, newone, array.Length);
        }

        public static void Insert(ref Array array, object newone, int at)
        {
            Array expanded = Array.CreateInstance(TypeUtility.GetElementTypeInArrayOrList(array.GetType()), array.Length + 1);
            Array.Copy(array, expanded, at);
            Array.Copy(array, at, expanded, at + 1, array.Length - at);
            expanded.SetValue(newone, at);
            array = expanded;
        }

        public static void Remove(ref Array array, int at)
        {
            Array expanded = Array.CreateInstance(TypeUtility.GetElementTypeInArrayOrList(array.GetType()), array.Length - 1);
            Array.Copy(array, expanded, at);
            Array.Copy(array, at + 1, expanded, at, array.Length - at - 1);
            array = expanded;
        }

        public static void Add<T>(ref T[] list, T newone)
        {
            Insert(ref list, newone, list.Length);
        }

        public static void Insert<T>(ref T[] list, T newone, int at)
        {
            var expanded = new T[list.Length + 1];
            System.Array.Copy(list, expanded, at);
            System.Array.Copy(list, at, expanded, at + 1, list.Length - at);
            expanded[at] = newone;
            list = expanded;
        }

        public static void Insert<T>(ref T[] list, T[] newList, int at)
        {
            var expanded = new T[list.Length + newList.Length];
            System.Array.Copy(list, expanded, at);
            System.Array.Copy(list, at, expanded, at + newList.Length, list.Length - at);
            for (int i = 0; i < newList.Length; i++)
            {
                expanded[at + i] = newList[i];
            }
            list = expanded;
        }

        public static T Remove<T>(ref T[] list, int at)
        {
            var oldone = list[at];
            System.Array.Copy(list, at + 1, list, at, list.Length - (at + 1));
            System.Array.Resize(ref list, list.Length - 1);
            return oldone;
        }

        public static void Remove<T>(ref T[] list, int start, int end)
        {
            if (list.Length > (end + 1))
            {
                System.Array.Copy(list, end + 1, list, start, list.Length - (end + 1));
            }
            System.Array.Resize(ref list, list.Length - (end - start + 1));
        }

        public static void Sub<T>(ref T[] list, int start)
        {
            Sub<T>(ref list, start, list.Length - start);
        }

        public static void Sub<T>(ref T[] list, int start, int length)
        {
            if (list.Length < (start + length))
            {
                length = list.Length - start;
            }
            if (length < 0)
            {
                length = 0;
            }

            var sub = new T[length];
            System.Array.Copy(list, start, sub, 0, length);
            list = sub;
        }

    }
}
