using System;
using System.Collections.Generic;

namespace DotEngine.Framework.Pool
{
    public sealed class ObjectPool<T>
    {
        private Stack<T> stack = new Stack<T>();

        private Func<T> createFunc = null;
        private Action<T> getAction = null;
        private Action<T> releaseAction = null;

        public ObjectPool(Func<T> creater, Action<T> geter, Action<T> releaser, int preload = 0)
        {
            createFunc = creater;
            getAction = geter;
            releaseAction = releaser;

            if(preload>0)
            {
                for(int i =0;i<preload;++i)
                {
                    stack.Push(createFunc());
                }
            }
        }

        public T Get()
        {
            T element;
            if (stack.Count == 0)
            {
                element = createFunc();
            }
            else
            {
                element = stack.Pop();
            }

            getAction?.Invoke(element);

            return element;
        }

        public void Release(T element)
        {
#if DEBUG
            if (stack.Contains(element))
            {
                throw new Exception("ObjectPool::Release->The element has been push into pool!");
            }
#endif
            if (element != null)
            {
                releaseAction?.Invoke(element);
                stack.Push(element);
            }
        }

        public void ClearAll()
        {
            stack.Clear();
        }
    }
}
