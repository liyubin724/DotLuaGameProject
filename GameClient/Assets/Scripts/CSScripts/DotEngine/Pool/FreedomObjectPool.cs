using DotEngine.Log;
using System;
using System.Collections.Generic;

namespace DotEngine.Pool
{
    public class FreedomObjectPool
    {
        private Stack<object> m_Stack = new Stack<object>();

        private Func<object> m_FuncCreate;
        private Action<object> m_ActionGet;
        private Action<object> m_ActionRelease;

        public FreedomObjectPool(Func<object> createFunc, Action<object> getAction, Action<object> releaseAction,int preloadCount = 0)
        {
            m_FuncCreate = createFunc;
            m_ActionGet = getAction;
            m_ActionRelease = releaseAction;

            for (int i = 0; i < preloadCount; ++i)
            {
                m_Stack.Push(m_FuncCreate());
            }
        }

        public object Get()
        {
            object element;
            if(m_Stack.Count == 0)
            {
                element = m_FuncCreate();
            }else
            {
                element = m_Stack.Pop();
            }

            m_ActionGet?.Invoke(element);

            return element;
        }

        public void Release(object element)
        {
#if DEBUG
            if (m_Stack.Contains(element))
            {
                LogUtil.LogError("ObjectPool", "The element has been push into pool!");
                return;
            }
#endif
            if (element!=null)
            {
                m_ActionRelease?.Invoke(element);
                m_Stack.Push(element);
            }
        }

        public void Clear()
        {
            m_Stack.Clear();
        }
    }
}
