using System.Collections;

namespace UnityCSharpCommon.Utils.Common
{
    /// <summary>
    /// Utilities for C# Generic Lists.
    /// </summary>
    public static class ListUtilities
    {
        /// <summary>
        /// Returns the circular next index, this means it will return 0 if you call it for last index of list.
        /// </summary>
        public static int GetNextIndex(this IList list, int currentIndex)
        {
            int realCapacity = list.Count;
            int realCurrentIndex = currentIndex + 1;

            return realCapacity == 0 ? 0 : (realCurrentIndex%realCapacity);
        }
    }
}