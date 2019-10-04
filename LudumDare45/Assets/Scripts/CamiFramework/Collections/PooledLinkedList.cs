using System;
using System.Collections;
using System.Collections.Generic;

namespace Cami.Collections
{
    public class PooledLinkedList<T> : IEnumerable<T>
    {
        public PooledLinkedList()
        {
            linkPool = new Pool<Link>(CreateNewLink);
        }

        private Link CreateNewLink()
        {
            return new Link();
        }

        public void Add(T item)
        {
            Link link = linkPool.Pop();
            link.Item = item;
            AddLink(link);
        }

        public bool Remove(T item)
        {
            Link link = GetLink(item);
            if (link == null)
                return false;

            RemoveLink(link);
            return true;
        }

        public bool RemoveAt(int index)
        {
            Link link = GetLink(index);
            if (link == null)
                return false;

            RemoveLink(link);
            return true;
        }

        public T ItemAt(int index)
        {
            if (index < 0 || index >= linkCount)
                throw new IndexOutOfRangeException(
                    string.Format("Index must be in the range {0} - {1}", 0, linkCount - 1));

            Link link = GetLink(index);
            return link.Item;
        }

        public void Clear()
        {
            Foreach(RemoveLink);
        }

        public void Foreach(Action<T> action)
        {
            Foreach((link) => { action(link.Item); });
        }

        private void Foreach(Action<Link> action)
        {
            Link current = head;
            while (current != null)
            {
                action(current);
                current = current.Next;
            }
        }

        private void AddLink(Link link)
        {
            if (head == null)
            {
                head = link;
            }
            else
            {
                tail.Next = link;
                link.Prev = tail;
            }
            tail = link;

            ++linkCount;
        }

        public void RemoveLink(Link link)
        {
            if (link.Prev != null)
                link.Prev.Next = link.Next;
            if (link.Next != null)
                link.Next.Prev = link.Prev;

            if (link == head)
                head = link.Next;
            if (link == tail)
                tail = link.Prev;

            link.Clear();
            linkPool.Push(link);
            --linkCount;
        }

        private Link GetLink(T item)
        {
            Link current = head;
            while(current != null)
            {
                if (current.Item.Equals(item))
                    return current;

                current = current.Next;
            }
            return null;
        }

        private Link GetLink(int index)
        {
            Link current = head;
            int currentIndex = 0;
            while (current != null)
            {
                if (currentIndex == index)
                    return current;
                ++currentIndex;

                current = current.Next;
            }
            return null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Link current = head;
            while (current != null)
            {
                yield return current.Item;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Properties
        public T First { get { return head != null ? head.Item : default(T); } }
        public T Last { get { return tail != null ? tail.Item : default(T); } }
        public int Count { get { return linkCount; } }

        public Link HeadLink { get { return head; } }
        public Link TailLink { get { return tail; } }
        #endregion Properties

        #region Fields
        private Pool<Link> linkPool;
        private Link head;
        private Link tail;
        private int linkCount;
        #endregion Fields

        public class Link
        {
            public void Clear()
            {
                Prev = null;
                Next = null;
                Item = default(T);
            }

            #region Properties
            #endregion Properties

            #region Fields
            public T Item;

            public Link Prev;
            public Link Next;
            #endregion Fields
        }
    }
}
