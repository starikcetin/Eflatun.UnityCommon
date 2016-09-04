using System;
using System.Collections.Generic;

namespace UnityCSCommon.Utils.Common
{
    /// <summary>
    /// Utilities for <see cref="Enum"/>.
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// Returns an <see cref="IEnumerator{T}"/> containing all values of <typeparamref name="T"/>.
        /// </summary>
        public static IEnumerable<T> GetValues<T>() where T : struct, IComparable, IConvertible, IFormattable
        {
            return (T[]) Enum.GetValues(typeof(T));
        }
    }
}