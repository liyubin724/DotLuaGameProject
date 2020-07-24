using System;
using System.Collections.Generic;

namespace DotEngine.Pool
{
    public class ObjectItemPool
    {
        private Stack<object> m_Stack = new Stack<object>();

        private Func<object> createItem;
        private Action<object> getItem;
        private Action<object> releaseItem;

        public ObjectItemPool(Func<object> create, Action<object> get, Action<object> release,int preloadCount = 0)
        {
            createItem = create;
            getItem = get;
            releaseItem = release;

            if(preloadCount>0)
            {
                for(int i =0;i<preloadCount;++i)
                {
                    object element = createItem();
                    m_Stack.Push(element);
                }
            }
        }

        public object GetItem()
        {
            object element = null;
            if(m_Stack.Count == 0)
            {
                element = createItem();
            }else
            {
                element = m_Stack.Pop();
            }

            getItem?.Invoke(element);

            return element;
        }

        public void ReleaseItem(object element)
        {
            if(element!=null)
            {
                releaseItem?.Invoke(element);
                m_Stack.Push(element);
            }
        }

        public void Clear()
        {
            m_Stack.Clear();
            createItem = null;
            getItem = null;
            releaseItem = null;
        }
    }
}
