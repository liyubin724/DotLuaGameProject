using DotEngine.Context.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotEngine.Context
{
    public class ContextContainer<K> : IContextContainer<K>
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

        public K[] Keys
        {
            get
            {
                return itemDic.Keys.ToArray();
            }
        }

        public bool Contains(K key)
        {
            return itemDic.ContainsKey(key);
        }

        public object Get(K key)
        {
            if(!Contains(key))
            {
                throw new ContextKeyNotFoundException(key);
            }else
            {
                object value = itemDic[key];
                if (value == null)
                {
                    throw new ContextValueNullException(key);
                }
                return value;
            }
        }

        public V Get<V>(K key)
        {
            if(!Contains(key))
            {
                throw new ContextKeyNotFoundException(key);
            }else
            {
                object value = itemDic[key];
                if(value == null)
                {
                    throw new ContextValueNullException(key);
                }else if(!IsCastableTo(value.GetType(),typeof(V)))
                {
                    throw new ContextValueCastFailedException(key, value.GetType(), typeof(V));
                }else
                {
                    return (V)value;
                }
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
            if (itemDic.TryGetValue(key, out object item))
            {
                if(item == null || !IsCastableTo(item.GetType(), typeof(V)))
                {
                    value = default;
                    return false;
                }else
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
                throw new ContextKeyRepeatException(key);
            }

            itemDic.Add(key, value);
        }

        public void Update(K key, object value)
        {
            if (Contains(key))
            {
                itemDic[key] = value;
            }
            else
            {
                throw new ContextKeyNotFoundException(key);
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
            }
        }

        public void Remove(K key)
        {
            if(Contains(key))
            {
                itemDic.Remove(key);
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

        public void InjectTo(IContextObject injectObject)
        {
            ContextUtil.Inject(this, injectObject);
        }

        public void ExtractFrom(IContextObject extractObject)
        {
            ContextUtil.Extract(this, extractObject);
        }

        private bool IsCastableTo(Type from, Type to)
        {
            if (to.IsAssignableFrom(from)) return true;

            var methods = from.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(
                    m => m.ReturnType == to &&
                    (m.Name == "op_Implicit" ||
                        m.Name == "op_Explicit")
                );
            return methods.Count() > 0;
        }
    }

    public class IntContextContainer : ContextContainer<int>
    {
    }

    public class StringContextContainer : ContextContainer<string>
    {
    }
}
