using System.Collections.Generic;

namespace AssociativeCache
{
    public class MruEvictionPolicy<TKey, TValue> : ICacheEvictionPolicy<TKey, TValue>
    {
        protected LinkedList<TKey> OrderedList = new LinkedList<TKey>();
        public TKey EvictItem()
        {
            var head = OrderedList.First;

            return head.Value;
        }

        public void OnItemAdded(TKey key, TValue value)
        {
            OrderedList.AddFirst(key);
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
            OrderedList.Remove(key);
            OrderedList.AddFirst(key);
        }
    }
}
