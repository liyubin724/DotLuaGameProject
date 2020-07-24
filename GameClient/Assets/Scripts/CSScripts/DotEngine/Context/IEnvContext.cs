namespace DotEngine.Context
{
    public interface IEnvContext<K>
    {
        object this[K key] { get;set; }

        bool ContainsKey(K key);

        object Get(K key);
        V Get<V>(K key);

        void Add(K key, object value);
        void Add(K key, object value, bool isNeverClear);

        void Update(K key, object value);

        void AddOrUpdate(K key, object value);
        void AddOrUpdate(K key, object value, bool isNeverClear);

        void Remove(K key);

        bool TryGet(K key, out object value);
        bool TryGet<V>(K key, out V value);

        void Clear();
        void Clear(bool isForce);
    }
}
