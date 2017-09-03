using System;

namespace AssociativeCache
{
    public class InMemoryCache<TKey, TValue> : ICacheProvider<TKey, TValue>
    {
        private readonly int _partitions;
        private readonly ICacheProvider<TKey, TValue>[] _cachePartitions;

        public InMemoryCache()
            :this(CacheDefaults.DefaultCapacity)
        {
        }

        public InMemoryCache(int capacity)
            : this(capacity, typeof(FifoEvictionPolicy<,>))
        {
        }

        public InMemoryCache(int capacity, Type evictionPolicyType)
            :this(CacheDefaults.DefaultPartitions, capacity, evictionPolicyType)
        {             
        }

        public InMemoryCache(int partitions, int capacity, Type evictionPolicyType)
        {
            if (partitions <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(partitions), "Parition count must be greater than 0");
            }

            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than 0");
            }

            _partitions = partitions;
            _cachePartitions = new ICacheProvider<TKey, TValue>[partitions];

            var setCapacity = capacity/partitions;

            for (var i = 0; i < partitions; i++)
            {
                _cachePartitions[i] = new MemoryCacheBase<TKey, TValue>(setCapacity, (ICacheEvictionPolicy<TKey, TValue>)Activator.CreateInstance(evictionPolicyType.MakeGenericType(typeof(TKey), typeof(TValue))));
            }
        }

        public bool ContainsKey(TKey key)
        {
            return GetCache(key).ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            GetCache(key).Add(key, value);
        }

        public void Remove(TKey key)
        {
            GetCache(key).Remove(key);
        }

        public TValue TryGetValue(TKey key)
        {
            return GetCache(key).TryGetValue(key);
        }

        private ICacheProvider<TKey, TValue> GetCache(TKey key)
        {
            var partition = _partitions > 1 ? Partition(key) : 0;

            return _cachePartitions[partition];
        }

        private int Partition(TKey key)
        {
            return Math.Abs(key.GetHashCode() % _partitions);
        }
    }
}
