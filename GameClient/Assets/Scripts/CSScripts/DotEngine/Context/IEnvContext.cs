namespace DotEngine.Context
{
    public interface IEnvContext<K>
    {
        object this[K key] { get; set; }
        
        int Count { get; }
        K[] Keys { get; }


        bool ContainsKey(K key);

        object Get(K key);
        V Get<V>(K key);

        bool TryGet(K key, out object value);
        bool TryGet<V>(K key, out V value);

        void Add(K key, object value);
        void Update(K key, object value);
        void AddOrUpdate(K key, object value);

        void Remove(K key);
        void RemoveRange(K[] keys);

        void Clear();
    }
}
