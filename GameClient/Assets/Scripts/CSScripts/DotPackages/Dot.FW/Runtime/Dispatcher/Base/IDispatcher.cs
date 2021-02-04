namespace DotEngine.Framework.Dispatcher
{
    public interface IDispatcher<K,V>
    {
        V this[K key] { get; set; }

        void Initialized();
        void Disposed();

        bool Contains(K key);
        void Register(K key, V value);
        void Unregister(K key);
        V Retrieve(K key);
        void ClearAll();
    }
}
