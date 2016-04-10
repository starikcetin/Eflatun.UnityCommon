using System;
using System.Collections.Generic;

namespace UnityCSharpCommon.Utils.Common
{
    /// <summary>
    /// A class that includes an extension method Shuffle() to shuffle a generic List with a given seed. To seed the class use SetSeed(int) method.
    /// </summary>
    public static class ListShuffle
    {
        private static Random _random;

        public static void SetSeed(int seed)
        {
            _random = new Random(seed);
        }

        /// <summary>
        /// <para>Returns a shuffled version of given list. Original doesn't change.</para>
        /// <para>(This method clones a new list and does shuffling operation on the clone; then returns the clone.)</para>
        /// </summary>
        public static List<T> SafeShuffle<T>(this List<T> original)
        {
            if (_random == null)
            {
                throw new NullReferenceException("The _random class of ListShuffle class has no seed. Please set the seed with SetSeed(int) method before calling Shuffle() method");
            }

            List<T> newList = new List<T>(original);
            int newListCount = newList.Count;

            for (int i = 0; i < newListCount; i++)
            {
                int randomIndex = _random.Next(newListCount); //get "_random index"

                var temp = newList[randomIndex]; //save "_random index"
                newList[randomIndex] = newList[i]; //copy "i" to "_random index"
                newList[i] = temp; //assign original "_random index" to "i"
            }

            return newList;
        }

        /// <summary>
        /// <para>Shuffles the given list. Original changes.</para>
        /// <para>(This method does shuffling operation on the original list.)</para>
        /// </summary>
        public static void Shuffle<T>(this List<T> original)
        {
            if (_random == null)
            {
                throw new NullReferenceException("The _random class of ListShuffle class has no seed. Please set the seed with SetSeed(int) method before calling Shuffle() method");
            }

            int count = original.Count;

            for (int i = 0; i < count; i++)
            {
                int randomIndex = _random.Next(count); //get "_random index"

                var temp = original[randomIndex]; //save "_random index"
                original[randomIndex] = original[i]; //copy "i" to "_random index"
                original[i] = temp; //assign original "_random index" to "i"
            }
        }
    }
}