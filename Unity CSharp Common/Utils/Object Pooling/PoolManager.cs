using System.Collections.Generic;
using System.Linq;
using UnityCSharpCommon.Expansions;
using UnityCSharpCommon.Utils.SingletonPatterns;
using UnityEngine;

namespace UnityCSharpCommon.Utils.Pooling
{
    public class PoolManager : SceneSingleton<PoolManager>
    {
        protected PoolManager() {} //prevent initailization

        #region Fields And Properties
        public PrefabPool_Inspector[] AllPoolSetups;

        /// <summary>
        /// (Prefab, Pool) Dictionary
        /// </summary>
        private readonly OrderedDictionary<GameObject, PrefabPool> _allPools = new OrderedDictionary<GameObject, PrefabPool>();

        /// <summary>
        /// Dictionary of all pools in (Prefab, Pool) format.
        /// </summary>
        public OrderedDictionary<GameObject, PrefabPool> AllPools
        {
            get { return _allPools; }
        }
        #endregion

        #region Unity Methods
        void Awake()
        {
            foreach (var item in AllPoolSetups)
            {
                CreatePool(item.Prefab, item.PrePopulateAmount, item.AutoPopulateAmount);
            }
        }
        #endregion

        #region Prefab Pool Creation
        /// <summary>
        /// Creates a new PrefabPool and adds it to dictionary.
        /// </summary>
        private PrefabPool CreatePool(GameObject prefab, int prePopulateAmount, int autoPopulateAmount)
        {
            var newPrefabPool = new PrefabPool(prefab, prePopulateAmount, autoPopulateAmount);
            _allPools.Add(prefab, newPrefabPool);
            return newPrefabPool;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// If Prefab is pooling, spawns a pooled object and returns it; otherwise instantiates a new instance and returns it.
        /// </summary>
        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return _Spawn(prefab, position, rotation);
        }

        /// <summary>
        /// If this is a pool object, despawns it; otherwise destroys it.
        /// </summary>
        public void Despawn(GameObject gameObject)
        {
            _Despawn(gameObject);
        }
        #endregion

        #region Inner Methods
        private GameObject _Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (_allPools.ContainsKey(prefab))
            {
                PrefabPool pool = _allPools[prefab];
                return pool.Spawn(position, rotation);
            }
            else
            {
                GameObject newObject = Instantiate(prefab, position, rotation) as GameObject;
                return newObject;
            }
        }

        private void _Despawn(GameObject gameObject)
        {
            KeyValuePair<GameObject, PrefabPool> foundEntry = _allPools.SingleOrDefault(a => a.Value.ActiveObjects.Contains(gameObject));

            if (foundEntry.Key != null)
            {
                PrefabPool pool = foundEntry.Value;
                pool.Despawn(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion
    }
}