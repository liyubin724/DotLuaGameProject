using System;
using System.Collections.Generic;
using SystemObject = System.Object;

namespace DotEngine.Pool
{
    public class ObjectPool
    {
        private readonly Stack<SystemObject> m_Stack = new Stack<SystemObject>();

        private readonly Func<SystemObject> m_CreateItemFunc;
        private readonly Action<SystemObject> m_GetItemFunc;
        private readonly Action<SystemObject> m_ReleaseItemFunc;

        public ObjectPool(
            Func<SystemObject> createFunc,
            Action<SystemObject> getAction = null,
            Action<SystemObject> releaseAction = null,
            int preloadCount = 0)
        {
            m_CreateItemFunc = createFunc;
            m_GetItemFunc = getAction;
            m_ReleaseItemFunc = releaseAction;

            for (int i = 0; i < preloadCount; ++i)
            {
                m_Stack.Push(m_CreateItemFunc());
            }
        }

        public SystemObject Get()
        {
            SystemObject element;
            if (m_Stack.Count == 0)
            {
                element = m_CreateItemFunc();
            }
            else
            {
                element = m_Stack.Pop();
            }

            m_GetItemFunc?.Invoke(element);

            return element;
        }

        public void Release(SystemObject element)
        {
#if DEBUG
            if (m_Stack.Contains(element))
            {
                UnityEngine.Debug.LogError("ObjectPool::Release->The element has been released");
                return;
            }
#endif

            m_ReleaseItemFunc?.Invoke(element);
            m_Stack.Push(element);
        }

        public void Clear()
        {
            m_Stack.Clear();
        }
    }
}
