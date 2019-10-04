using UnityEngine;
using System.Collections.Generic;

namespace Cami.Collections
{
    public class Pool<T>
    {
        public Pool()
        {
            InitPool(null, DefaultSize);
        }

        public Pool(Spawner spawnDelegate)
        {
            InitPool(spawnDelegate, DefaultSize);
        }

        public Pool(Spawner spawnDelegate, uint initialSize)
        {
            InitPool(spawnDelegate, initialSize);
        }

        private void InitPool(Spawner spawnDelegate, uint initialSize)
        {
            SpawnDelegate = spawnDelegate;

            for (int i = 0; i < initialSize; ++i)
            {
                Push(SpawnDelegate());
            }
        }

        public T Pop()
        {
            T element;
            if (pooledElements.Count > 0)
            {
                element = pooledElements.Dequeue();
            }
            else
            {
                element = SpawnDelegate();
            }

            return element;
        }

        public void Push(T element)
        {
            pooledElements.Enqueue(element);
        }

        #region Fields
        public Spawner SpawnDelegate;
        private Queue<T> pooledElements = new Queue<T>();
        public int Count { get { return pooledElements.Count; } }

        public delegate T Spawner();
        #endregion Fields

        #region Connts
        private const uint DefaultSize = 0;
        #endregion Consts
    }
}