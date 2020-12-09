using System;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Context
{
    public class EnvContext<K> : IEnvContext<K>
    {
        private Dictionary<K, object> itemDic = new Dictionary<K, object>();

        public object this[K key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                AddOrUpdate(key, value);
            }
        }

        public int Count
        {
            get
            {
                return itemDic.Count;
            }
        }

        public K[] Keys
        {
            get
            {
                return itemDic.Keys.ToArray();
            }
        }

        public bool ContainsKey(K key)
        {
            return itemDic.ContainsKey(key);
        }

        public object Get(K key)
        {
            if (itemDic.TryGetValue(key, out object value))
            {
                return value;
            }

            return null;
        }

        public V Get<V>(K key)
        {
            var value = Get(key);
            if(value!=null)
            {
                return (V)value;
            }
            return default;
        }

        public bool TryGet(K key, out object value)
        {
            value = Get(key);

            if(value !=null)
            {
                return true;
            }else
            {
                return false;
            }
        }

        public bool TryGet<V>(K key, out V value)
        {
            if (TryGet(key, out object item))
            {
                value = (V)item;
                return true;
            }
            value = default;
            return false;
        }

        public void Add(K key, object value)
        {
            if (ContainsKey(key))
            {
                throw new Exception($"EnvContext::Add->The key has been saved into dictionry.you can use 'AddOrUpdate' to changed it.key = {key}");
            }

            itemDic.Add(key, value);
        }

        public void Update(K key, object value)
        {
            if (ContainsKey(key))
            {
                itemDic[key] = value;
            }
            else
            {
                throw new Exception($"AContext::Update->Key not found.key = {key}");
            }
        }

        public void AddOrUpdate(K key, object value)
        {
            if (ContainsKey(key))
            {
                Update(key, value);
            }
            else
            {
                Add(key, value);
            }
        }

        public void Remove(K key)
        {
            if(ContainsKey(key))
            {
                itemDic.Remove(key);
            }
            else
            {
                throw new Exception($"AContext::Remove->Key not found.key = {key}");
            }
        }

        public void RemoveRange(K[] keys)
        {
            if (keys != null && keys.Length > 0)
            {
                foreach (var key in keys)
                {
                    Remove(key);
                }
            }
        }

        public void Clear()
        {
            itemDic.Clear();
        }

    }
}
