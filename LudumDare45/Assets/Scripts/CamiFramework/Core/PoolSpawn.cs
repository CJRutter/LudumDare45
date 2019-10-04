using UnityEngine;

namespace Cami.Core
{
    public class PoolSpawn : MonoBehaviour
    {
        void Start()
        {
        }
        
        public virtual void Spawn(PrefabPool pool)
        {
            this.pool = pool;

            if (Spawned != null)
                Spawned(gameObject);
        }

        public virtual void Despawn()
        {
            pool.Push(this.gameObject);

            if (Despawned != null)
                Despawned(gameObject);
        }

        #region Properties
        #endregion Properties

        #region Fields
        private PrefabPool pool;
        #endregion Fields

        #region Events
        public delegate void PoolSpawnEventHandler(GameObject gameObject);

        public event PoolSpawnEventHandler Spawned;
        public event PoolSpawnEventHandler Despawned;
        #endregion Events
    }
}