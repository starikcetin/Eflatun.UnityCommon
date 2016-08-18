using System;
using System.Collections.Generic;

namespace UnityCSCommon.Utils.Common
{
    /// <summary>
    /// Includes methods to shuffle lists with seeding, using Fisher-Yates algorithm.
    /// </summary>
    public class ListShuffle
    {
        private Random _random;

        public ListShuffle(int seed)
        {
            _random = new Random(seed);
        }

        public void ChangeSeed (int seed)
        {
            _random = new Random(seed);
        }

        /// <summary>
        /// <para>Returns a shuffled version of given list. Original doesn't change.</para>
        /// <para>(This method clones a new list and does shuffling operation on the clone; then returns the clone.)</para>
        /// </summary>
        public List<T> SafeShuffle<T> (IList<T> original)
        {
            var newList = new List<T>(original);
            FisherYatesShuffle(newList);
            return newList;
        }

        /// <summary>
        /// <para>Shuffles the given list. Original changes.</para>
        /// <para>(This method does shuffling operation on the original list.)</para>
        /// </summary>
        public void Shuffle<T> (IList<T> original)
        {
            FisherYatesShuffle(original);
        }

        private void FisherYatesShuffle<T> (IList<T> list)
        {
            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                int randomIndex = _random.Next(count); //Get a random index.
                list.Swap (i, randomIndex);            //Swap the items at i and random index.
            }
        }
    }
}