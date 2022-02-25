using System.Collections.Generic;

namespace DotEngine.Pool
{
    public static class ListPool<T>
    {
        private static readonly ObjectPool<List<T>> sm_ListPool = new ObjectPool<List<T>>(
            () => new List<T>(),
            null,
            (list) => list.Clear()
            );

        public static List<T> Get()
        {
            return sm_ListPool.Get();
        }

        public static void Release(List<T> list)
        {
            sm_ListPool.Release(list);
        }
    }

    public static class StringListPool
    {
        public static List<string> Get()
        {
            return ListPool<string>.Get();
        }

        public static void Release(List<string> list)
        {
            ListPool<string>.Release(list);
        }
    }

    public static class FloatListPool
    {
        public static List<float> Get()
        {
            return ListPool<float>.Get();
        }

        public static void Release(List<float> list)
        {
            ListPool<float>.Release(list);
        }
    }

    public static class IntListPool
    {
        public static List<int> Get()
        {
            return ListPool<int>.Get();
        }

        public static void Release(List<int> list)
        {
            ListPool<int>.Release(list);
        }
    }
}
