using System;
using System.Collections.Generic;

namespace UnityCSharpCommonLibrary.CSharpExpansions
{
    /// <summary>
    /// A dictionary type which allows bi-directional lookups.
    /// <see cref="StrictBiDictionary{TFirst, TSecond}"/> type allows only one first-second match, thus lookup results are always single values.
    /// A <see cref="NotSupportedException"/> will be thrown if there is an attempt to add more than one first-second match.
    /// </summary>
    /// <seealso cref="SafeBiDictionary{TFirst, TSecond}"/>
    public class StrictBiDictionary<TFirst, TSecond> : IEnumerable<KeyValuePair<TFirst, TSecond>>
    {
        #region IEnumerable implementation
        public IEnumerator<KeyValuePair<TFirst, TSecond>> GetEnumerator()
        {
            return firstToSecond.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Fields
        private IDictionary<TFirst, TSecond> firstToSecond;
        private IDictionary<TSecond, TFirst> secondToFirst;
        #endregion

        #region Properties
        public IDictionary<TFirst, TSecond> FirstToSecond
        {
            get
            {
                return firstToSecond;
            }
        }

        public ICollection<TFirst> Firsts
        {
            get
            {
                return firstToSecond.Keys;
            }
        }

        public IDictionary<TSecond, TFirst> SecondToFirst
        {
            get
            {
                return secondToFirst;
            }
        }

        public ICollection<TSecond> Seconds
        {
            get
            {
                return secondToFirst.Keys;
            }
        }

        public int Count
        {
            get
            {
                return firstToSecond.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StrictBiDictionary{TFirst, TSecond}"/> class.
        /// </summary>
        public StrictBiDictionary()
        {
            EnsureTypesDifferent();

            firstToSecond = new Dictionary<TFirst, TSecond>();
            secondToFirst = new Dictionary<TSecond, TFirst>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StrictBiDictionary{TFirst, TSecond}"/> class with a custom equality comparer for TFirst.
        /// </summary>
        /// <param name="firstComparer">Custom equality comparer for TFirst.</param>
        public StrictBiDictionary(IEqualityComparer<TFirst> firstComparer)
        {
            EnsureTypesDifferent();

            firstToSecond = new Dictionary<TFirst, TSecond>(firstComparer);
            secondToFirst = new Dictionary<TSecond, TFirst>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StrictBiDictionary{TFirst, TSecond}"/> class with a custom equality comparer for TSecond.
        /// </summary>
        /// <param name="secondComparer">Custom equality comparer for TSecond.</param>
        public StrictBiDictionary(IEqualityComparer<TSecond> secondComparer)
        {
            EnsureTypesDifferent();

            firstToSecond = new Dictionary<TFirst, TSecond>();
            secondToFirst = new Dictionary<TSecond, TFirst>(secondComparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StrictBiDictionary{TFirst, TSecond}"/> class with custom equality comparers for both TFirst and TSecond.
        /// </summary>
        /// <param name="firstComparer">Custom equality comparer for TFirst.</param>
        /// <param name="secondComparer">Custom equality comparer for TSecond.</param>
        public StrictBiDictionary(IEqualityComparer<TFirst> firstComparer, IEqualityComparer<TSecond> secondComparer)
        {
            EnsureTypesDifferent();

            firstToSecond = new Dictionary<TFirst, TSecond>(firstComparer);
            secondToFirst = new Dictionary<TSecond, TFirst>(secondComparer);
        }

        private static void EnsureTypesDifferent()
        {
            if (typeof(TFirst) == typeof(TSecond))
            {
                throw new NotSupportedException("TFirst and TSecond cannot be the same type.");
            }
        }
        #endregion

        #region Indexers
        // Note potential ambiguity using indexers (e.g. mapping from int to int)
        // Hence the methods as well...
        public TSecond this[TFirst first]
        {
            get { return GetByFirst(first); }
        }

        public TFirst this[TSecond second]
        {
            get { return GetBySecond(second); }
        }
        #endregion

        #region Public Methods
        public void Add(TFirst first, TSecond second)
        {
            if (firstToSecond.ContainsKey(first) ||
                secondToFirst.ContainsKey(second))
            {
                throw new NotSupportedException("Duplicate firsts or seconds are not supported in StrictBiDictionary type. Consider using SafeBiDictionary if you need multiple first-second matches.");
            }
            firstToSecond.Add(first, second);
            secondToFirst.Add(second, first);
        }

        public bool TryAdd(TFirst first, TSecond second)
        {
            if (firstToSecond.ContainsKey(first) ||
                secondToFirst.ContainsKey(second))
            {
                return false;
            }
            firstToSecond.Add(first, second);
            secondToFirst.Add(second, first);
            return true;
        }

        public TSecond GetByFirst(TFirst first)
        {
            return firstToSecond[first];
        }

        public TFirst GetBySecond(TSecond second)
        {
            return secondToFirst[second];
        }

        public bool ContainsFirst(TFirst first)
        {
            return firstToSecond.ContainsKey(first);
        }

        public bool ContainsSecond(TSecond second)
        {
            return secondToFirst.ContainsKey(second);
        }

        public bool TryGetByFirst(TFirst first, out TSecond second)
        {
            return firstToSecond.TryGetValue(first, out second);
        }

        public bool TryGetBySecond(TSecond second, out TFirst first)
        {
            return secondToFirst.TryGetValue(second, out first);
        }

        public bool RemoveByFirst(TFirst first)
        {
            secondToFirst.Remove(firstToSecond[first]);
            return firstToSecond.Remove(first);
        }

        public bool RemoveBySecond(TSecond second)
        {
            firstToSecond.Remove(secondToFirst[second]);
            return secondToFirst.Remove(second);
        }

        public void Clear()
        {
            firstToSecond.Clear();
            secondToFirst.Clear();
        }
        #endregion
    }
}