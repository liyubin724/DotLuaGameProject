using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Pool
{
    public interface IPoolItem
    {
        void OnGet();
        void OnRelease();
    }

    public class ItemPool<T> where T : class, IPoolItem, new()
    {
        private Stack<T> m_Stack = new Stack<T>();

        public ItemPool(int preloadCount = 0)
        {
            if (preloadCount > 0)
            {
                for (int i = 0; i < preloadCount; i++)
                {
                    T element = new T();
                    m_Stack.Push(element);
                }
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

            element?.OnGet();

            return element;
        }

        public void Release(T element)
        {
#if DEBUG
            if (m_Stack.Contains(element))
            {
                Debug.LogError("ObjectPool::Release->The element has been push into pool!");
                return;
            }
#endif
            element.OnRelease();
            m_Stack.Push(element);
        }

        public void Clear()
        {
            m_Stack.Clear();
        }
    }
}
