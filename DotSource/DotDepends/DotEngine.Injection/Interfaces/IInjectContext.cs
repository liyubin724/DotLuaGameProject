namespace DotEngine.Injection
{
    public interface IInjectContext<K>
    {
        object this[K key] { get; set; }
        
        int Count { get; }
        K[] Keys { get; }

        bool Contains(K key);

        object Get(K key);
        V Get<V>(K key);

        bool TryToGet(K key, out object value);
        bool TryToGet<V>(K key, out V value);

        void Add(K key, object value);
        void Update(K key, object value);
        void AddOrUpdate(K key, object value);

        void Remove(K key);
        void RemoveRange(K[] keys);
        
        void Clear();

        void InjectTo(object injectObject);
        void ExtractFrom(object extractObject);
    }
}
