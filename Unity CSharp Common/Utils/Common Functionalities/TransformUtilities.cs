using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityCSCommon.Utils.Common
{
    /// <summary>
    /// Utilities and extension methods for <see cref="Transform"/>s.
    /// </summary>
    public static class TransformUtilities
    {
        /// <summary>
        /// Returns all children of <paramref name="transform"/>
        /// </summary>
        public static IEnumerable<Transform> GetChildren (this Transform transform)
        {
            return transform.Cast<Transform>();
        }

        /// <summary>
        /// Returns all children of <paramref name="transform"/> except the ones in <paramref name="except"/>.
        /// </summary>
        public static IEnumerable<Transform> GetChildren (this Transform transform, IEnumerable<Transform> except)
        {
            return transform.Cast<Transform>().Except(except);
        }
    }
}