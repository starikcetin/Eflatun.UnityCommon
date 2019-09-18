using System;

namespace starikcetin.Eflatun.UnityCommon.Utils.Common
{
    /// <summary>
    /// Utilities for <see cref="Enum"/>.
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// Returns an array containing all values of <typeparamref name="T"/>.
        /// </summary>
        public static T[] GetValues<T>() where T : struct, IComparable, IConvertible, IFormattable
        {
            return (T[]) Enum.GetValues(typeof(T));
        }
    }
}