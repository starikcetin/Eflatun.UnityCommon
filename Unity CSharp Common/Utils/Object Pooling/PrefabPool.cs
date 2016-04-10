using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCSharpCommon.Utils.Pooling
{
    /// <summary>
    /// Pool for prefabs.
    /// </summary>
    public sealed class PrefabPool
    {
        #region Fields And Properties
        public readonly MaintenanceMethods Maintenance;

        private readonly GameObject _prefab;
        public GameObject Prefab
        {
            get { return _prefab; }
        }

        private readonly List<GameObject> _activeObjects;
        public List<GameObject> ActiveObjects
        {
            get { return _activeObjects; }
        }

        private readonly List<GameObject> _inactiveObjects;
        public List<GameObject> InactiveObjects
        {
            get { return _inactiveObjects; }
        }

        private readonly GameObject _holder;
        private readonly int _autoPopulateAmount;
        #endregion

        #region Initializators
        /// <summary>
        /// Initializes a new instance of the <see cref="PrefabPool"/> class.
        /// </summary>
        /// <param name="prefab">Prefab that this pool will use.</param>
        /// <param name="prePopulateAmount">Populates pool with given amount instantly.</param>
        /// <param name="autoPopulateAmount">The amount to populate this pool automatically when there are no inactive objects left.</param>
        public PrefabPool(GameObject prefab, int prePopulateAmount, int autoPopulateAmount)
        {
            _activeObjects = new List<GameObject>();
            _inactiveObjects = new List<GameObject>();

            _prefab = prefab;

            _holder = new GameObject(string.Format("Pool of {0}", prefab.name));
            _holder.transform.parent = PoolManager.Instance.gameObject.transform;

            _autoPopulateAmount = autoPopulateAmount;

            Maintenance = new MaintenanceMethods(this);
            Maintenance.Populate(prePopulateAmount);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Spawn a GameObject and returns it.
        /// </summary>
        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            GameObject spawned = _Spawn(position, rotation);
            NotifySpawn(spawned);
            return spawned;
        }

        /// <summary>
        /// Despawn a GameObject.
        /// </summary>
        public void Despawn(GameObject gameObject)
        {
            _Despawn(gameObject);
            NotifyDespawn(gameObject);
        }
        #endregion

        #region Inner Methods
        private GameObject _Spawn(Vector3 position, Quaternion rotation)
        {
            GameObject toSpawn = GetInactiveOrInstantiate();
            toSpawn.transform.parent = null;
            toSpawn.transform.position = position;
            toSpawn.transform.rotation = rotation;
            toSpawn.SetActive(true);

            _inactiveObjects.Remove(toSpawn);
            _activeObjects.Add(toSpawn);

            PopulateIfRequired();

            return toSpawn;
        }

        private void _Despawn(GameObject toDespawn)
        {
            toDespawn.SetActive(false);
            toDespawn.transform.parent = _holder.transform;

            _activeObjects.Remove(toDespawn);
            _inactiveObjects.Add(toDespawn);
        }

        /// <summary>
        /// Calls all OnSpawn() methods in given GameObject.
        /// </summary>
        private static void NotifySpawn(GameObject spawned)
        {
            var interfaces = spawned.GetComponents<IPoolInteractions>();
            foreach (var item in interfaces)
            {
                item.OnSpawn();
            }
        }

        /// <summary>
        /// Calls all OnDespawn() methods in given GameObject.
        /// </summary>
        private static void NotifyDespawn(GameObject despawned)
        {
            var interfaces = despawned.GetComponents<IPoolInteractions>();
            foreach (var item in interfaces)
            {
                item.OnDespawn();
            }
        }

        /// <summary>
        /// Populates the pool if there are no inactive objects left.
        /// </summary>
        private void PopulateIfRequired()
        {
            if (!_inactiveObjects.Any()) //if there are no inactive objects left...
            {
                Maintenance.Populate(_autoPopulateAmount); //...populate the pool.
            }
        }

        /// <summary>
        /// Gets the first inactivate object in the list. If there is no available gameObject, instantiates a new one and returns it.
        /// </summary>
        private GameObject GetInactiveOrInstantiate()
        {
            if (_inactiveObjects.Any()) //if there are any inactive objects...
            {
                return _inactiveObjects[0];
            }
            else
            {
                return Maintenance.PopulateSingle();
            }
        }
        #endregion

        #region Maintenance
        /// <summary>
        /// The methods that will be used only for maintenance purposes.
        /// </summary>
        public class MaintenanceMethods
        {
            private readonly PrefabPool _parent;

            public MaintenanceMethods(PrefabPool pool)
            {
                _parent = pool;
            }

            /// <summary>
            /// Populates the pool by given amount of new GameObjects, returns instantiated GameObjects.
            /// </summary>
            public List<GameObject> Populate(int amount)
            {
                List<GameObject> newObjects = InstantiateMultiple(amount);

                foreach (var item in newObjects)
                {
                    Bind(item, true);
                }

                return newObjects;
            }

            /// <summary>
            /// Populates the pool by a single new GameObject and returns it.
            /// </summary>
            public GameObject PopulateSingle()
            {
                GameObject newObject = InstantiateSingle();
                Bind(newObject, true);
                return newObject;
            }

            /// <summary>
            /// Adds the given GameObject to this pool. If despawn is true, also despawns the object.
            /// </summary>
            private void Bind(GameObject gameObject, bool despawn)
            {
                if (despawn)
                {
                    _parent._activeObjects.Add(gameObject); //add to active objects and...
                    _parent.Despawn(gameObject); //...despawn.
                }
                else
                {
                    _parent._activeObjects.Add(gameObject); //add to active objects, but don't despawn.
                }
            }

            /// <summary>
            /// Instantiates a single instance of Prefab and returns it.
            /// </summary>
            private GameObject InstantiateSingle()
            {
                var parentPrefab = _parent._prefab;
                var newObject = MonoBehaviour.Instantiate(parentPrefab) as GameObject;
                return newObject;
            }

            /// <summary>
            /// Instantiates given amount of GameObjects and returns them.
            /// </summary>
            private List<GameObject> InstantiateMultiple(int amount)
            {
                List<GameObject> newObjects = new List<GameObject>();

                for (int i = 0; i < amount; i++)
                {
                    newObjects.Add(InstantiateSingle());
                }

                return newObjects;
            }
        }
        #endregion
    }
}