using NUnit.Framework;
using Shouldly;

namespace AssociativeCache.Tests.WhenUsingLruCacheAlgorithm
{
    //https://en.wikipedia.org/wiki/Cache_replacement_policies#Least_Recently_Used_.28LRU.29
    [TestFixture]
    public class AndAddingItemsToCache
    {
        private ICacheProvider<string, string> _cache;

        [SetUp]
        public void SetUp()
        {
            const int maxItems = 4;
            
            _cache = new InMemoryCache<string, string>(maxItems, typeof(LruEvictionPolicy<,>));
        }

        [TestCase("E", "5")]
        [TestCase("F", "6")]
        public void AndMultipleItemsExistInCache(string key, string expectedValue)
        {
            AndCacheIsPopulated();
            AndItemExistsInCache("A");
            WhenAddingItemToCache(key, expectedValue);
            AndItemDoesNotExistInCache("A");
        }

        [TestCase("E", "5")]
        [TestCase("F", "6")]
        public void AndAfterAccessingExistingItemInCache(string key, string expectedValue)
        {
            AndCacheIsPopulated();
            AndItemExistsInCache("A");
            AndItemExistsInCache("B");
            AndItemIsRetrievedFromCache("A");
            WhenAddingItemToCache(key, expectedValue);
            AndItemDoesNotExistInCache("B");
        }

        private void AndCacheIsPopulated()
        {
            _cache.Add("A", "1");
            _cache.Add("B", "2");
            _cache.Add("C", "3");
            _cache.Add("D", "4");
        }

        private void WhenAddingItemToCache(string key, string value)
        {
            _cache.Add(key, value);
        }

        private void AndItemExistsInCache(string key)
        {
            _cache.ContainsKey(key).ShouldBe(true);
        }

        private void AndItemDoesNotExistInCache(string key)
        {
            _cache.ContainsKey(key).ShouldBe(false);
        }

        private void AndItemIsRetrievedFromCache(string key)
        {
            _cache.TryGetValue(key);
        }
    }
}
