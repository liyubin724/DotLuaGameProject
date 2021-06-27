using System;
using System.Collections.Generic;

namespace DotEngine.Pool
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
                Logger.Error("ObjectPool", "the element has been released");
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


    //-----以后统一使用ObjectPool，将会陆续替换
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
                Logger.Error("ObjectPool","GenericObjectPool::Release->The element has been push into pool!");
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
                    Logger.Error("ObjectPool","ItemObjectPool::Release->The element has been push into pool!");
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
