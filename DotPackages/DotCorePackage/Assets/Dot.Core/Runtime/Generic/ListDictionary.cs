using System;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Generic
{
    public class ListDictionary<K, V>
    {
        private List<K> keyList = new List<K>();
        private Dictionary<K, V> dataDic = new Dictionary<K, V>();

        public V this[K key]
        {
            get
            {
                if(dataDic.TryGetValue(key,out V v))
                {
                    return v;
                }
                return default(V);
            }
            set
            {
                if(dataDic.ContainsKey(key))
                {
                    dataDic[key] = value;
                }else
                {
                    keyList.Add(key);
                    dataDic.Add(key, value);
                }
            }
        }
        public V this[int index]
        {
            get
            {
                return dataDic[keyList[index]];
            }
            set
            {
                if(index<0 || index>=keyList.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                dataDic[keyList[index]] = value;
            }
        }

        public K[] Keys => keyList.ToArray();
        public V[] Values => dataDic.Values.ToArray();

        public int Count => keyList.Count;

        public void Add(K key, V value)
        {
            if(ContainsKey(key))
            {
                throw new Exception("The key has been add into dictionary");
            }
            keyList.Add(key);
            dataDic.Add(key, value);
        }

        public void Clear()
        {
            keyList.Clear();
            dataDic.Clear();
        }

        public bool ContainsKey(K key)
        {
            return dataDic.ContainsKey(key);
        }

        public int IndexOfKey(K key)
        {
            return keyList.IndexOf(key);
        }

        public bool Remove(K key)
        {
            if(ContainsKey(key))
            {
                keyList.Remove(key);
                dataDic.Remove(key);
                return true;
            }else
            {
                return false;
            }
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= keyList.Count)
            {
                throw new IndexOutOfRangeException();
            }

            K key = keyList[index];
            keyList.RemoveAt(index);
            dataDic.Remove(key);
        }

        public bool TryGetValue(K key, out V value)
        {
            return dataDic.TryGetValue(key, out value);
        }
    }
}
