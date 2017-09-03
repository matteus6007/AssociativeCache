# Associate Cache

#### Installation
Install the nuget package `AssociativeCache.4.0.0.nupkg`.

##### Using In Memory Cache Manager

Initialise a single instance of the cache manager using with default parameters.

```
var cacheManager = new InMemoryCache<string, string>();
```

###### Adding a new item to cache
```
cacheManager.Add("key", "value");
```

###### Getting item from cache
```
var cacheValue = cacheManager.TryGetValue("key");
```

##### Overriding the default eviction policy (FIFO)

Current implementations include:

* First In First Out - FifoEvictionPolicy (default)
* Most Recently Used - MruEvictionPolicy
* Least Recently Used - LruEvictionPolicy

```
var evictionPolicy = new MruEvictionPolicy<TKey,TValue>();
var cacheManager = new InMemoryCache<string, string>(evictionPolicy);
```

###### Implementing custom eviction policy

Create a new class that derives from the interface `ICacheEvictionPolicy<TKey,TValue>`.

```
public class CustomEvictionPolicy<TKey,TValue> : ICacheEvictionPolicy<TKey,TValue>
{
        public TKey EvictItem()
        {
            //return key of item to evict
        }

        public void OnItemAdded(TKey key, TValue value)
        {
            //custom logic
        }

        public void OnItemAccessed(TKey key, TValue value)
        {
            //custom logic
        }

        public void OnItemRemoved(TKey key)
        {
            //custom logic
        }
}
```