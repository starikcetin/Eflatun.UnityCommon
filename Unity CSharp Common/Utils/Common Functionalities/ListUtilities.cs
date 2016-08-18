using System.Collections;
using System.Collections.Generic;

namespace UnityCSCommon.Utils.Common
{
    /// <summary>
    /// Utilities for C# Generic Lists.
    /// </summary>
    public static class ListUtilities
    {
        /// <summary>
        /// Returns the circular next index, this means it will return 0 if you call it for last index of list.
        /// </summary>
        public static int GetNextIndexCircular (this IList list, int currentIndex)
        {
            int realCapacity = list.Count;
            int realCurrentIndex = currentIndex + 1;

            return realCapacity == 0 ? 0 : (realCurrentIndex % realCapacity);
        }

        /// <summary>
        /// Swaps the indexes of items at <see cref="i1"/> and <see cref="i2"/>.
        /// </summary>
        public static void Swap<T> (this IList<T> list, int i1, int i2)
        {
            var temp = list[i1];    //save the item at i1 to temp
            list[i1] = list[i2];    //copy the item at i2 to i1
            list[i2] = temp;        //copy the temp to i2
        }
    }
}