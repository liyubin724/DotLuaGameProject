using System;

namespace DotEngine.Context
{
    public class TypeContext
    {
        private EnvContext<Type> context = new EnvContext<Type>();

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
            return context.ContainsKey(type);
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

        public void Add(object value,bool isNeverClear = false)
        {
            Add(value.GetType(), value, isNeverClear);
        }

        public void Add(Type type,object value,bool isNeverClear = false)
        {
            context.Add(type, value,isNeverClear);
        }

        public void Update(Type type,object value)
        {
            context.Update(type, value);
        }

        public void Update(object value)
        {
            Update(value.GetType(), value);
        }

        public void AddOrUpdate(object value,bool isNeverClear = false)
        {
            AddOrUpdate(value.GetType(), value, isNeverClear);
        }

        public void AddOrUpdate(Type type,object value,bool isNeverClear = false)
        {
            context.AddOrUpdate(type, value, isNeverClear);
        }

        public void Remove(Type type)
        {
            context.Remove(type);
        }

        public void Clear(bool isForce = false)
        {
            context.Clear(isForce);
        }

        public void Inject(object injectObj)
        {
            ContextUtil.Inject<Type>(context, injectObj);
        }

        public void Extract(object extractObj)
        {
            ContextUtil.Extract<Type>(context, extractObj);
        }
    }
}
