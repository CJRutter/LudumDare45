using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cami.Collections
{
    public class BinaryHeap<T>
    {
        public BinaryHeap(Comparison<T> comparison)
        {
            elements = new List<T>();
            this.comparison = comparison;
        }

        public void Add(T item)
        {
            int index = elements.Count + 1;
            elements.Add(item);
            EnsureIndexPosition(index);
        }

        public void Remove(T item)
        {
            int index = elements.IndexOf(item);
            RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            ++index;
            int child1Index = index * 2;
            int child2Index = child1Index + 1;

            SwapElements(index - 1, elements.Count - 1);
            elements.RemoveAt(elements.Count - 1);

            while (child2Index <= elements.Count)
            {
                bool child1Smaller = (comparison(elements[child1Index - 1], elements[index - 1]) < 0);
                bool child2Smaller = (comparison(elements[child2Index - 1], elements[index - 1]) < 0);
                if (child1Smaller || child2Smaller)
                {
                    int smallerChild = child1Index;

                    if (child1Smaller && child2Smaller)
                    {
                        if (comparison(elements[child2Index - 1], elements[child1Index - 1]) < 0)
                        {
                            smallerChild = child2Index;
                        }
                    }
                    else if (child2Smaller)
                    {
                        smallerChild = child2Index;
                    }
                    SwapElements(smallerChild - 1, index - 1);
                    index = smallerChild;
                }
                else
                {
                    break;
                }

                child1Index = index * 2;
                child2Index = child1Index + 1;
            }
        }

        public void Clear()
        {
            elements.Clear();
        }

        public bool Contains(T item)
        {
            return elements.Contains(item);
        }
    
        public void EnsureItemPosition(T item)
        {
            int index = elements.IndexOf(item);
            EnsureIndexPosition(index);
        }

        public void EnsureIndexPosition(int index)
        {
            int parentIndex = index / 2;
            while (index > 1 &&
                comparison(elements[index - 1], elements[parentIndex - 1]) < 0)
            {
                SwapElements(parentIndex - 1, index - 1);
                index = parentIndex;
                parentIndex = index / 2;
            }
        }

        private void SwapElements(int index1, int index2)
        {
            T temp = elements[index2];

            elements[index2] = elements[index1];
            elements[index1] = temp;
        }
    
        #region Properties
        public T this[int i]
        {
            get { return elements[i]; }
        }
    
        public T Front
        {
            get
            {
                if (Count == 0)
                    return default(T);

                return elements[0];
            }
        }

        public int Count
        {
            get { return elements.Count; }
        }
        #endregion Properties
    
        #region Fields
        private List<T> elements;
        private Comparison<T> comparison;
        #endregion Fields
    }
}