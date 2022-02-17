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
}
