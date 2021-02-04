using System;
using System.Collections.Generic;

namespace DotEngine.Framework.Dispatcher
{
    public abstract class ADispatcher<K, V> : IDispatcher<K, V>
    {
        protected Dictionary<K, V> itemDic = null;

        public V this[K key]
        {
            get
            {
                return Retrieve(key);
            }
            set
            {
                if (value == null)
                {
                    Unregister(key);
                } else
                {
                    if (itemDic.ContainsKey(key))
                    {
                        itemDic[key] = value;
                    } else
                    {
                        itemDic.Add(key, value);
                    }
                }
            }
        }

        public void Disposed()
        {
            DoDisposed();
            itemDic.Clear();
        }

        public void Initialized()
        {
            itemDic = new Dictionary<K, V>();
            DoInitalized();
        }

        public void ClearAll()
        {
            itemDic.Clear();
        }

        public bool Contains(K key)
        {
            return itemDic.ContainsKey(key);
        }

        public void Register(K key, V value)
        {
            if (!itemDic.ContainsKey(key))
            {
                DoRegisterItem(key, value);
                itemDic.Add(key, value);
            }
        }

        public V Retrieve(K key)
        {
            if (itemDic.ContainsKey(key))
            {
                return itemDic[key];
            }
            return default;
        }

        public void Unregister(K key)
        {
            if (itemDic.TryGetValue(key, out V value))
            {
                itemDic.Remove(key);

                DoUnregisterItem(key, value);
            }
        }

        protected virtual void DoInitalized() { }
        protected virtual void DoDisposed() { }

        protected virtual void DoRegisterItem(K key, V value) { }
        protected virtual void DoUnregisterItem(K key, V value) { }
    }
}
