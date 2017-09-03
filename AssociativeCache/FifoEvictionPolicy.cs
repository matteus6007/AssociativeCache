using System.Collections.Generic;

namespace AssociativeCache
{
    public class FifoEvictionPolicy<TKey, TValue> : ICacheEvictionPolicy<TKey, TValue>
    {
        protected LinkedList<TKey> OrderedList = new LinkedList<TKey>();

        public TKey EvictItem()
        {
            var head = OrderedList.First;

            return head.Value;
        }

        public void OnItemAdded(TKey key, TValue value)
        {
            OrderedList.AddLast(key);
        }

        public void OnItemAccessed(TKey key, TValue value)
        {
            //do nothing
        }

        public void OnItemRemoved(TKey key)
        {
            OrderedList.Remove(key);
        }
    }
}
