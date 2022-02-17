using System;
using System.Collections.Generic;

namespace DotEngine.Pool
{
    public interface IElement
    {
        void OnGetFromPool();
        void OnReleaseToPool();
    }

    public class ElementPool<T> where T:class,IElement,new()
    {
        private Stack<T> m_Stack = new Stack<T>();

        public ElementPool(int preloadCount = 0)
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

            element?.OnGetFromPool();

            return element;
        }

        public void Release(T element)
        {
#if DEBUG
            if (m_Stack.Contains(element))
            {
                throw new Exception("ElementPool<T>::Release->The element has been push into pool!");
            }
#endif
            element.OnReleaseToPool();
            m_Stack.Push(element);
        }

        public void Clear()
        {
            m_Stack.Clear();
        }
    }
}
