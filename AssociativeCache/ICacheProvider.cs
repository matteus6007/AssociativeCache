namespace AssociativeCache
{
    public interface ICacheProvider<in TKey, TValue>
    {
        bool ContainsKey(TKey key);
        void Add(TKey key, TValue value);
        void Remove(TKey key);
        TValue TryGetValue(TKey key);
    }
}