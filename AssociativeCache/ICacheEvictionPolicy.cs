namespace AssociativeCache
{
    public interface ICacheEvictionPolicy<TKey, in TValue>
    {
        TKey EvictItem();
        void OnItemAdded(TKey key, TValue value);
        void OnItemAccessed(TKey key, TValue value);
        void OnItemRemoved(TKey key);
    }
}
