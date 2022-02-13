using System;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Injection
{
    public class InjectContext<K> : IInjectContext<K>
    {
        private readonly Dictionary<K, object> itemDic = new Dictionary<K, object>();

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

        private K[] cachedKeys = null;
        public K[] Keys
        {
            get
            {
                if (cachedKeys == null)
                {
                    cachedKeys = itemDic.Keys.ToArray();
                }
                return cachedKeys;
            }
        }

        public bool Contains(K key)
        {
            return itemDic.ContainsKey(key);
        }

        public object Get(K key)
        {
            if (!Contains(key))
            {
                throw new InjectContextKeyNotFoundException(key);
            }
            else
            {
                object value = itemDic[key];
                if (value == null)
                {
                    throw new InjectValueNotFoundException(key);
                }
                return value;
            }
        }

        public V Get<V>(K key)
        {
            if (!Contains(key))
            {
                throw new InjectContextKeyNotFoundException(key);
            }
            else
            {
                object value = itemDic[key];
                if (value == null)
                {
                    throw new InjectValueNotFoundException(key);
                }
                else if (!value.GetType().IsCastableTo(typeof(V)))
                {
                    throw new InjectContextValueCastFailedException(key, value.GetType(), typeof(V));
                }
                else
                {
                    return (V)value;
                }
            }
        }

        public bool TryToGet(K key, out object value)
        {
            if (itemDic.TryGetValue(key, out value))
            {
                return true;
            }

            value = null;
            return false;
        }

        public bool TryToGet<V>(K key, out V value)
        {
            if (itemDic.TryGetValue(key, out object item))
            {
                if (item == null || !item.GetType().IsCastableTo(typeof(V)))
                {
                    value = default;
                    return false;
                }
                else
                {
                    value = (V)item;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public void Add(K key, object value)
        {
            if (Contains(key))
            {
                throw new InjectContextKeyRepeatException(key);
            }

            itemDic.Add(key, value);
            cachedKeys = null;
        }

        public void Update(K key, object value)
        {
            if (Contains(key))
            {
                itemDic[key] = value;
            }
            else
            {
                throw new InjectContextKeyNotFoundException(key);
            }
        }

        public void AddOrUpdate(K key, object value)
        {
            if (Contains(key))
            {
                itemDic[key] = value;
            }
            else
            {
                itemDic.Add(key, value);
                cachedKeys = null;
            }
        }

        public void Remove(K key)
        {
            if (Contains(key))
            {
                itemDic.Remove(key);
                cachedKeys = null;
            }
        }

        public void RemoveRange(K[] keys)
        {
            if (keys != null && keys.Length > 0)
            {
                foreach (var key in keys)
                {
                    itemDic.Remove(key);
                }
                cachedKeys = null;
            }
        }

        public void Clear()
        {
            itemDic.Clear();
            cachedKeys = null;
        }

        public void InjectTo(object target)
        {
            if (target == null)
            {
                return;
            }

            InjectReflectionTypeInfo typeInfo = InjectReflection.GetTypeInfo(target.GetType());
            typeInfo.InjectTo(this, target);
        }

        public void ExtractFrom(object target)
        {
            if (target == null)
            {
                return;
            }

            InjectReflectionTypeInfo typeInfo = InjectReflection.GetTypeInfo(target.GetType());
            typeInfo.ExtractFrom(this, target);
        }
    }

    public class InjectIntContext : InjectContext<int>
    {
    }

    public class InjectStringContext : InjectContext<string>
    {
    }

    public class InjectTypeContext : InjectContext<Type>
    {
    }
}
