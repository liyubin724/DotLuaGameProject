using System;
using System.Collections.Generic;

namespace KSTCEngine.Pool
{
    public class SimpleObjectPool<T> where T : new()
    {
        private readonly Stack<T> m_Stack = new Stack<T>();
        private readonly Action<T> m_ActionOnGet;
        private readonly Action<T> m_ActionOnRelease;

        public SimpleObjectPool(Action<T> getAction, Action<T> releaseAction, int preload = 0)
        {
            m_ActionOnGet = getAction;
            m_ActionOnRelease = releaseAction;

            for (int i = 0; i < preload; ++i)
            {
                m_Stack.Push(new T());
            }
        }

        public T Get()
        {
            T element;
            if (m_Stack.Count == 0)
            {
                element = new T();
            }
            else
            {
                element = m_Stack.Pop();
            }

            m_ActionOnGet?.Invoke(element);

            return element;
        }

        public void Release(T element)
        {
#if DEBUG
            if (m_Stack.Contains(element))
            {
                UnityEngine.Debug.LogError("ObjectPool::Release->The element has been push into pool!");
                return;
            }
#endif

            m_ActionOnRelease?.Invoke(element);
            m_Stack.Push(element);
        }

        public void Clear()
        {
            m_Stack.Clear();
        }
    }
}
