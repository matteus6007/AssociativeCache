using NUnit.Framework;
using Shouldly;

namespace AssociativeCache.Tests.WhenUsingMruCacheAlgorithm
{
    [TestFixture]
    public class AndGettingItemsFromCache
    {
        private ICacheProvider<string, string> _cache;

        [SetUp]
        public void SetUp()
        {
            const int maxItems = 50;

            _cache = new InMemoryCache<string, string>(maxItems, typeof(MruEvictionPolicy<,>));
        }

        [TestCase("key1", "value1")]
        [TestCase("key2", "value2")]
        public void AndItemIsFoundInCache(string key, string expectedValue)
        {
            _cache.Add(key, expectedValue);

            var actualValue = _cache.TryGetValue(key);

            actualValue.ShouldBe(expectedValue);
        }

        [TestCase("key1")]
        public void AndItemIsNotInCache(string key)
        {
            _cache.Remove(key);

            var actualValue = _cache.TryGetValue(key);

            actualValue.ShouldBe(default(string));
        }
    }
}
