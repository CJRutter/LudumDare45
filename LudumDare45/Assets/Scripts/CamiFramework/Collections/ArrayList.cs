using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

namespace Cami.Collections
{
    [System.Serializable]
    public class ArrayList<T> : ICollection<T>, IList<T>
    {
        public ArrayList()
            : this(DefaultInitialBufferSize)
        {
        }

        public ArrayList(int initialSize)
        {
            ResizeCoefficent = 2;
            EnsureFit(initialSize);
        }

        public void Add(T item)
        {
            if (Count + 1 >= BufferLength)
                EnsureFit(Count + 1);
            
            buffer[Count] = item;
            ++Count;
        }
        
        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
                Add(item);
        }

        public void Insert(int index, T item)
        {
            if (Count + 1 >= BufferLength)
                EnsureFit(Count + 1);

            if (index < Count)
            {
                Shift(index, 1);
                buffer[index] = item;
            }
            else
            {
                Add(item);
            }
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);

            if (index < 0)
                return false;

            RemoveAt(index);

            return true;
        }
        
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                return;

            Shift(index, -1);
        }

        public void RemoveLast()
        {
            RemoveAt(Count - 1);
        }

        public void Clear()
        {
            if (buffer == null)
                return;

            Array.Clear(buffer, 0, Count);
            Count = 0;
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (EqualityComparer<T>.Default.Equals(buffer[i], item))
                    return i;
            }
            return -1;
        }

        public void Trim()
        {
            if (Count > 0)
            {
                Array.Resize(ref buffer, Count);
            }
            else
            {
                buffer = null;
            }
        }

        private void Shift(int startIndex, int places)
        {
            if (buffer == null)
                return;

            if (places < 0)
                startIndex -= places;

            Array.Copy(buffer, startIndex, buffer, startIndex + places, Count - startIndex);
            Count += places;

            if (places < 0)
            {
                Array.Clear(buffer, Count, -places);
            }
            else
            {
                Array.Clear(buffer, startIndex, places);
            }
        }
        
        public void EnsureFit(int size)
        {
            if (BufferLength >= size)
                return;

            int newSize = size;
            if (buffer != null)
            {
                newSize = (int)(newSize * ResizeCoefficent);
                while (newSize < size)
                {
                    newSize = (int)(newSize * ResizeCoefficent);
                }
            }

            Resize(newSize);
        }

        public void Resize(int size)
        {
            if (size == 0)
            {
                buffer = null;
                Count = 0;
                return;
            }
            else if (size < Count)
            {
                Count = size;            
            }

            Array.Resize(ref buffer, size);
        }

        public T[] TrimAndGetBuffer()
        {
            Trim();
            return buffer;
        }

        public T[] GetBuffer()
        {
            return buffer;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
                yield return buffer[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Sort()
        {
            Sort(Comparer<T>.Default.Compare);
        }

        public void Sort(Comparison<T> comparison)
        {
            int min;
            for (int i = 0; i < Count - 1; i++)
            {
                min = i;
                for (int j = (i + 1); j < Count; j++)
                {
                    if (comparison(this[j], this[min]) < 0)
                    {
                        min = j;
                    }
                }
                T temp = this[min];
                this[min] = this[i];
                this[i] = temp;
            }

        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for(int i = 0; i < Count; ++i)
            {
                if (arrayIndex >= array.Length)
                    return;

                array[arrayIndex] = buffer[i];
            }
        }

        public void Swap(int indexA, int indexB)
        {
            T temp = buffer[indexA];
            buffer[indexA] = buffer[indexB];
            buffer[indexB] = temp;
        }

        public void Reverse()
        {
            for(int i = 0; i < Count / 2; ++i)
            {
                Swap(i, Count - i - 1);
            }
        }

        public override string ToString()
        {
            return string.Format("ArrayList (Count: {0})", Count);
        }

        #region Properties
        public int Count { get; private set; }
        public int BufferLength { get { return buffer != null ? buffer.Length : 0; } }
        public float ResizeCoefficent { get; set; }
        public bool IsReadOnly { get { return false; } }
        public T First { get { return buffer[0]; } }
        public T Last { get { return buffer[Count - 1]; } }

        public T this[int index]
        {
            get { return buffer[index];  }
            set { buffer[index] = value; }
        }
        #endregion Properties

        #region Fields
        private T[] buffer = null;
        private int bufferSize = 0;

        private const int DefaultInitialBufferSize = 10;
	    #endregion Fields
    }
}