using System;
using System.Collections.Generic;

namespace DotEngine.NetCore.Pool
{
    public class ObjectPool<T>
    {
        private readonly Stack<T> m_Stack = new Stack<T>();

        private readonly Func<T> m_CreateItemFunc;
        private readonly Action<T> m_GetItemFunc;
        private readonly Action<T> m_ReleaseItemFunc;

        public ObjectPool(Func<T> createFunc, Action<T> getAction = null, Action<T> releaseAction = null, int preload = 0)
        {
            m_CreateItemFunc = createFunc;
            m_GetItemFunc = getAction;
            m_ReleaseItemFunc = releaseAction;

            for (int i = 0; i < preload; ++i)
            {
                m_Stack.Push(m_CreateItemFunc());
            }
        }

        public T Get()
        {
            T element;
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

        public void Release(T element)
        {
#if DEBUG
            if (m_Stack.Contains(element))
            {
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
