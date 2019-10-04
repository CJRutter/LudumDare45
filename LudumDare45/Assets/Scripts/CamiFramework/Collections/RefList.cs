using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

namespace Cami.Collections
{
    public class RefList<TKey> : IEnumerable<TKey>
    {
        public RefList()
        {
            valueRefs = new Dictionary<TKey, LinkedListNode<TKey>>();
            values = new LinkedList<TKey>();
        }

        public void AddFirst(TKey value)
        {
            if (valueRefs.ContainsKey(value))
                return;

            LinkedListNode<TKey> link = values.AddFirst(value);
            valueRefs[value] = link;
        }

        public void AddLast(TKey value)
        {
            if (valueRefs.ContainsKey(value))
                return;

            LinkedListNode<TKey> link = values.AddLast(value);
            valueRefs[value] = link;
        }

        public bool Remove(TKey value)
        {
            LinkedListNode<TKey> link;
            if (valueRefs.TryGetValue(value, out link) == false)
                return false;

            values.Remove(link);
            valueRefs.Remove(value);
            return true;
        }

        public bool RemoveFirst()
        {
            if (Count == 0)
                return false;

            return Remove(values.First.Value);
        }

        public bool RemoveLast()
        {
            if (Count == 0)
                return false;

            return Remove(values.Last.Value);
        }

        public bool Contains(TKey key)
        {
            return valueRefs.ContainsKey(key);
        }

        public void Clear()
        {
            valueRefs.Clear();
            values.Clear();
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            foreach (TKey value in values)
                yield return value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Properties
        public LinkedListNode<TKey> First
        {
            get { return values.First; }
        }

        public LinkedListNode<TKey> Last
        {
            get { return values.Last; }
        }

        public int Count
        {
            get { return values.Count; }
        }
        #endregion Properties

        #region Fields
        private Dictionary<TKey, LinkedListNode<TKey>> valueRefs;
        private LinkedList<TKey> values;
        #endregion Fields
    }
}