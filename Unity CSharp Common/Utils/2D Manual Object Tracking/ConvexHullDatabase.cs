using System.Collections.Generic;
using UnityCSCommon.Expansions;
using UnityCSCommon.Utils.CodePatterns;
using UnityCSCommon.Utils.Common;
using UnityEngine;

namespace UnityCSCommon.Utils.ManualTracking2D
{
    public class ConvexHullDatabase : GlobalSingleton<ConvexHullDatabase>
    {
        protected ConvexHullDatabase() { } //prevent init

        private readonly OrderedDictionary<GameObject, IList<Vector2>> _localConvexHullDictionary = new OrderedDictionary<GameObject, IList<Vector2>>();

        /// <summary>
        /// Gets the local convex hull of the given Prefab.
        /// </summary>
        public IList<Vector2> GetLocalConvexHull (GameObject prefab)
        {
            IList<Vector2> foundValue;
            if (_localConvexHullDictionary.TryGetValue(prefab, out foundValue))
            {
                return foundValue;
            }

            var localConvexHull = prefab.GetComponent<SpriteRenderer>().sprite.vertices.MakeConvexHull();
            _localConvexHullDictionary.Add(prefab, localConvexHull);
            return localConvexHull;
        }
    }
}