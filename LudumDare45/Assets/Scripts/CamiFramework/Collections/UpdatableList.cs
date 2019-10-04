using Cami.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cami.Collections
{
    public class UpdatableList<T> : IUpdatable, IEnumerable<T>
        where T : IUpdatable
    {
        public UpdatableList()
        {
            Enabled = true;
            items = new List<T>();
            updatesList = new PooledLinkedList<T>();
        }

        public UpdatableList(int capacity)
        {
            Enabled = true;
            items = new List<T>(capacity);
            updatesList = new PooledLinkedList<T>();
        }

        public void Add(T item)
        {
            items.Add(item);
        }

        public bool Remove(T item)
        {
            return items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void Update(float timeStep)
        {
            for(int i = 0; i < items.Count; ++i)
            {
                T item = items[i];
                if (item.Enabled == false)
                    continue;
                updatesList.Add(item);
            }

            PooledLinkedList<T>.Link current, next;
            current = updatesList.HeadLink;
            while (current != null)
            {
                current.Item.Update(timeStep);
                next = current.Next;
                updatesList.RemoveLink(current);

                current = next;
            }
        }
        
        public void Sort(Comparison<T> comparison)
        {
            items.Sort(comparison);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #region Properties
        public int Count { get { return items.Count; } }
        public T this[int index]
        {
            get { return items[index]; }
        }

        public bool Enabled { get; set; }
        #endregion Properties

        #region Fields
        private List<T> items;
        private PooledLinkedList<T> updatesList;
        #endregion Fields
    }
}
