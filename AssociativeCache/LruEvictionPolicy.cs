using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AssociativeCache
{
    public class LruEvictionPolicy<TKey, TValue> : ICacheEvictionPolicy<TKey, TValue>
    {
        protected LinkedList<TKey> OrderedList = new LinkedList<TKey>();
        protected ConcurrentDictionary<TKey, LinkedListNode<TKey>> ReverseKey = new ConcurrentDictionary<TKey, LinkedListNode<TKey>>();
        public TKey EvictItem()
        {
            var tail = OrderedList.Last;

            return tail.Value;
        }

        public void OnItemAdded(TKey key, TValue value)
        {
            var node = OrderedList.AddFirst(key);

            //ReverseKey.AddOrUpdate(key, node);
        }

        public void OnItemAccessed(TKey key, TValue value)
        {
            MoveToHead(key);
        }

        public void OnItemRemoved(TKey key)
        {
            OrderedList.Remove(key);
        }

        private void MoveToHead(TKey key)
        {
            OrderedList.Remove(ReverseKey[key]);
            OrderedList.AddFirst(key);
        }
    }
}
