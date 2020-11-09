using DotEngine.Pool;
using System.Collections.Generic;

namespace DotEngine.World.QT
{
    internal static class QuadPool
    {
        private static GenericObjectPool<List<IQuadObject>> sm_ObjectListPool = null;
        private static GenericObjectPool<List<QuadNode>> sm_NodeListPool = null;

        static QuadPool()
        {
            sm_ObjectListPool = new GenericObjectPool<List<IQuadObject>>(() =>
            {
                return new List<IQuadObject>();
            }, 
            null, 
            (list) =>
            {
                list.Clear();
            });

            sm_NodeListPool = new GenericObjectPool<List<QuadNode>>(() =>
            {
                return new List<QuadNode>();
            },
            null,
            (list) =>
            {
                list.Clear();
            });
        }

        internal static List<IQuadObject> GetObjectList()
        {
            return sm_ObjectListPool.Get();
        }

        internal static void ReleaseObjectList(List<IQuadObject> list)
        {
            sm_ObjectListPool.Release(list);
        }

        internal static List<QuadNode> GetNodeList()
        {
            return sm_NodeListPool.Get();
        }

        internal static void ReleaseNodeList(List<QuadNode> list)
        {
            sm_NodeListPool.Release(list);
        }

        internal static void ClearPools()
        {
            sm_ObjectListPool.Clear();
            sm_NodeListPool.Clear();
        }
    }
}
