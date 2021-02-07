using System;

namespace DotEngine.Context
{
    [Obsolete("it has been obsoleted.please fixed it if it was used")]
    public class TypeContext
    {
        private ContextContainer<Type> context = new ContextContainer<Type>();

        public object this[Type type]
        {
            get
            {
                return context[type];
            }
            set
            {
                context[type] = value;
            }
        }

        public bool Contains(Type type)
        {
            return context.Contains(type);
        }

        public T Get<T>(Type type)
        {
            return context.Get<T>(type);
        }

        public T Get<T>()
        {
            return context.Get<T>(typeof(T));
        }

        public object Get(Type type)
        {
            return context.Get(type);
        }

        public void Add(object value)
        {
            Add(value.GetType(), value);
        }

        public void Add(Type type, object value)
        {
            context.Add(type, value);
        }

        public void Update(Type type,object value)
        {
            context.Update(type, value);
        }

        public void Update(object value)
        {
            Update(value.GetType(), value);
        }

        public void AddOrUpdate(object value)
        {
            AddOrUpdate(value.GetType(), value);
        }

        public void AddOrUpdate(Type type,object value)
        {
            context.AddOrUpdate(type, value);
        }

        public void Remove(Type type)
        {
            context.Remove(type);
        }

        public void Clear()
        {
            context.Clear();
        }

    }
}
