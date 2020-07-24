using System.Collections.Generic;

namespace DotEngine.Pool
{
    public interface IObjectPoolItem
    {
        void OnNew();
        void OnGet();
        void OnRelease();
    }

    public class ObjectPool<T> where T : class,IObjectPoolItem,new()
    {
        private Stack<T> m_Stack = new Stack<T>();

        public ObjectPool(int preloadCount=0)
        {
            if(preloadCount>0)
            {
                for (int i = 0; i < preloadCount; i++)
                {
                    T element = CreateElement();
                    m_Stack.Push(element);
                }
            }
        }

        public T Get()
        {
            T element = default;
            if (m_Stack.Count == 0)
            {
                element = CreateElement();
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
            if(element!=null)
            {
                element.OnRelease();
                m_Stack.Push(element);
            }
        }

        public void Clear()
        {
            m_Stack.Clear();
            m_Stack = null;
        }

        private T CreateElement()
        {
            T element = new T();
            element.OnNew();

            return element;
        }
    }
}
