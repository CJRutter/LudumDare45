using Cami.Collections;
using UnityEngine;

namespace Cami.Core
{
    public class PrefabPool : MonoBehaviour
    {
        void Start()
        {
            pool.SpawnDelegate = SpawnPrefab;
        }
        
        public T Pop<T>()
        {
            return Pop().GetComponent<T>();
        }

        public GameObject Pop()
        {
            if (pool.SpawnDelegate == null)
                pool.SpawnDelegate = SpawnPrefab;

            GameObject gameObject = pool.Pop();

            PoolSpawn spawn = gameObject.GetComponent<PoolSpawn>();
            if (spawn != null)
                spawn.Spawn(this);

            if (Parent != null)
                gameObject.transform.SetParent(Parent);

            gameObject.SetActive(true);
            return gameObject;
        }

        public void Push(GameObject gameObject)
        {
            gameObject.SetActive(false);
            pool.Push(gameObject);
        }

        private GameObject SpawnPrefab()
        {
            return Instantiate(Prefab);
        }

        #region Properties
        #endregion Properties

        #region Fields
        public GameObject Prefab;
        public Transform Parent;
        private Pool<GameObject> pool = new Pool<GameObject>();
        #endregion Fields
    }
}