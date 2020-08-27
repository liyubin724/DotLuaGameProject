using System;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Context
{
    public class EnvContext<K> : IEnvContext<K>
    {
        private Dictionary<K, object> itemDic = new Dictionary<K, object>();
        private List<K> neverClearKeyList = new List<K>();

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

        public void Add(K key, object value)
        {
            Add(key, value, false);
        }

        public void Add(K key, object value, bool isNeverClear)
        {
            if (ContainsKey(key))
            {
                throw new Exception($"AContext::Add->The key has been saved into dictionry.you can use 'AddOrUpdate' to changed it.key = {key}");
            }

            itemDic.Add(key, value);

            if(isNeverClear)
            {
                neverClearKeyList.Add(key);
            }
        }

        public void AddOrUpdate(K key, object value)
        {
            if (ContainsKey(key))
            {
                itemDic[key] = value;
            }
            else
            {
                itemDic.Add(key, value);
            }
        }

        public void AddOrUpdate(K key, object value, bool isNeverClear)
        {
            bool cachedIsNeverClear = false;
            if(ContainsKey(key))
            {
                itemDic[key] = value;
                cachedIsNeverClear = neverClearKeyList.Contains(key);
            }else
            {
                itemDic.Add(key, value);
            }

            if(cachedIsNeverClear!=isNeverClear)
            {
                if(isNeverClear)
                {
                    neverClearKeyList.Add(key);
                }else
                {
                    neverClearKeyList.Remove(key);
                }
            }
        }

        public void Update(K key, object value)
        {
            if (itemDic.TryGetValue(key, out object item))
            {
                itemDic[key] = item;
            }
            else
            {
                throw new Exception($"AContext::Update->Key not found.key = {key}");
            }
        }

        public bool ContainsKey(K key)
        {
            return itemDic.ContainsKey(key);
        }

        public object Get(K key)
        {
            if(TryGet(key,out object v))
            {
                return v;
            }
            return null;
        }

        public V Get<V>(K key)
        {
            object obj = Get(key);
            if(obj!=null)
            {
                return (V)obj;
            }
            return default;
        }

        public void Remove(K key)
        {
            if(ContainsKey(key))
            {
                itemDic.Remove(key);
                if(neverClearKeyList.Contains(key))
                {
                    neverClearKeyList.Remove(key);
                }
            }
            else
            {
                throw new Exception($"AContext::Remove->Key not found.key = {key}");
            }
        }

        public bool TryGet(K key, out object value)
        {
            if (itemDic.TryGetValue(key, out value))
            {
                return true;
            }

            value = null;
            return false;
        }

        public bool TryGet<V>(K key, out V value)
        {
            if(itemDic.TryGetValue(key,out object item))
            {
                value = (V)item;
                return true;
            }
            value = default;
            return false;
        }

        public void Clear()
        {
            Clear(false);
        }

        public void Clear(bool isForce)
        {
            if (isForce)
            {
                neverClearKeyList.Clear();
                itemDic.Clear();
            }
            else
            {
                K[] keys = itemDic.Keys.ToArray();
                foreach (var key in keys)
                {
                    if (!neverClearKeyList.Contains(key))
                    {
                        itemDic.Remove(key);
                    }
                }
            }
        }

    }
}
