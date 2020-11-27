using System;
using System.Collections.Generic;

namespace DotEngine.Pool
{
    public class ObjectPool
    {
        private Stack<object> m_Stack = new Stack<object>();

        private Func<object> m_OnNew;
        private Action<object> m_OnGet;
        private Action<object> m_OnRelease;

        public ObjectPool(Func<object> createFunc,
            Action<object> getAction,
            Action<object> releaseAction, 
            int preloadCount = 0)
        {
            m_OnNew = createFunc;
            m_OnGet = getAction;
            m_OnRelease = releaseAction;

            for (int i = 0; i < preloadCount; ++i)
            {
                m_Stack.Push(m_OnNew());
            }
        }

        public object Get()
        {
            object element;
            if (m_Stack.Count == 0)
            {
                element = m_OnNew();
            }
            else
            {
                element = m_Stack.Pop();
            }

            m_OnGet?.Invoke(element);

            return element;
        }

        public void Release(object element)
        {
#if DEBUG
            if (m_Stack.Contains(element))
            {
                DebugLog.Error("ObjectPool::Release->The element has been push into pool!");
                return;
            }
#endif
            if (element != null)
            {
                m_OnRelease?.Invoke(element);
                m_Stack.Push(element);
            }
        }

        public void Clear()
        {
            m_Stack.Clear();
        }
    }

    public class GenericObjectPool<T>
    {
        private readonly Stack<T> m_Stack = new Stack<T>();

        private Func<T> m_OnNew;
        private readonly Action<T> m_OnGet;
        private readonly Action<T> m_OnRelease;

        public GenericObjectPool(Func<T> createFunc,Action<T> getAction,Action<T> releaseAction,int preload = 0)
        {
            m_OnNew = createFunc;
            m_OnGet = getAction;
            m_OnRelease = releaseAction;

            for(int i =0;i<preload;++i)
            {
                m_Stack.Push(m_OnNew());
            }
        }

        public T Get()
        {
            T element;
            if (m_Stack.Count == 0)
            {
                element = m_OnNew();
            }
            else
            {
                element = m_Stack.Pop();
            }

            m_OnGet?.Invoke(element);

            return element;
        }

        public void Release(T element)
        {
#if DEBUG
            if (m_Stack.Contains(element))
            {
                DebugLog.Error("GenericObjectPool::Release->The element has been push into pool!");
                return;
            }
#endif

            m_OnRelease?.Invoke(element);
            m_Stack.Push(element);
        }

        public void Clear()
        {
            m_Stack.Clear();
        }
    }

    public interface IObjectPoolItem
    {
        void OnGet();
        void OnRelease();
    }

    public class ItemObjectPool<T> where T : class, IObjectPoolItem, new()
    {
        private Stack<T> m_Stack = new Stack<T>();

        public ItemObjectPool(int preloadCount=0)
        {
            if(preloadCount>0)
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
            if(element!=null)
            {
                element.OnRelease();

#if DEBUG
                if (m_Stack.Contains(element))
                {
                    DebugLog.Error("ItemObjectPool::Release->The element has been push into pool!");
                    return;
                }
#endif
                m_Stack.Push(element);
            }
        }

        public void Clear()
        {
            m_Stack.Clear();
            m_Stack = null;
        }
    }
}
