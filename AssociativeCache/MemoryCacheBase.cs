using System;
using System.Collections.Concurrent;

namespace AssociativeCache
{
    internal class MemoryCacheBase<TKey, TValue> : ICacheProvider<TKey, TValue>
    {
        private readonly int _capacity;
        private readonly ICacheEvictionPolicy<TKey, TValue> _evictionPolicy;
        private readonly ConcurrentDictionary<TKey, TValue> _cacheItems = new ConcurrentDictionary<TKey, TValue>();

        public MemoryCacheBase(int capacity, ICacheEvictionPolicy<TKey, TValue> evictionPolicy)
        {
            _capacity = capacity;
            _evictionPolicy = evictionPolicy;
        }

        public bool ContainsKey(TKey key)
        {
            return _cacheItems.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            if (_capacity > 0 && _cacheItems.Keys.Count == _capacity)
            {
                var purgeKey = _evictionPolicy.EvictItem();

                Remove(purgeKey);
            }

            _cacheItems.TryAdd(key, value);
            _evictionPolicy.OnItemAdded(key, value);
        }

        public void Remove(TKey key)
        {
            if (!_cacheItems.ContainsKey(key)) return;

            TValue removedValue;

            _cacheItems.TryRemove(key, out removedValue);
            _evictionPolicy.OnItemRemoved(key);
        }

        public TValue TryGetValue(TKey key)
        {
            TValue value;

            if (!_cacheItems.TryGetValue(key, out value)) return default(TValue);

            _evictionPolicy.OnItemAccessed(key, value);

            return value;
        }
    }
}
