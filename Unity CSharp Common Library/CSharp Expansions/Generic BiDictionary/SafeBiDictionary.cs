using System;
using System.Collections.Generic;

namespace UnityCSharpCommonLibrary.CSharpExpansions
{
    /// <summary>
    /// A dictionary type which allows bi-directional lookups.
    /// <see cref="SafeBiDictionary{TFirst, TSecond}"/> type allows multiple first-second matches; thus lookup results are lists containing all found matches.
    /// </summary>
    /// <seealso cref="StrictBiDictionary{TFirst, TSecond}"/>
    public class SafeBiDictionary<TFirst, TSecond> : IEnumerable<KeyValuePair<TFirst, IList<TSecond>>>
    {
        #region IEnumerable implementation
        public IEnumerator<KeyValuePair<TFirst, IList<TSecond>>> GetEnumerator()
        {
            return firstToSecond.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Fields
        private static readonly IList<TFirst> EmptyFirstList = new TFirst[0];
        private static readonly IList<TSecond> EmptySecondList = new TSecond[0];

        private IDictionary<TFirst, IList<TSecond>> firstToSecond;
        private IDictionary<TSecond, IList<TFirst>> secondToFirst;
        #endregion

        #region Properties
        public IDictionary<TFirst, IList<TSecond>> FirstToSecond
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

        public IDictionary<TSecond, IList<TFirst>> SecondToFirst
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
        /// Initializes a new instance of the <see cref="SafeBiDictionary{TFirst, TSecond}"/> class.
        /// </summary>
        public SafeBiDictionary()
        {
            EnsureTypesDifferent();

            firstToSecond = new Dictionary<TFirst, IList<TSecond>>();
            secondToFirst = new Dictionary<TSecond, IList<TFirst>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeBiDictionary{TFirst, TSecond}"/> class with a custom equality comparer for TFirst.
        /// </summary>
        /// <param name="firstComparer">Custom equality comparer for TFirst.</param>
        public SafeBiDictionary(IEqualityComparer<TFirst> firstComparer)
        {
            EnsureTypesDifferent();

            firstToSecond = new Dictionary<TFirst, IList<TSecond>>(firstComparer);
            secondToFirst = new Dictionary<TSecond, IList<TFirst>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeBiDictionary{TFirst, TSecond}"/> class with a custom equality comparer for TSecond.
        /// </summary>
        /// <param name="secondComparer">Custom equality comparer for TSecond.</param>
        public SafeBiDictionary(IEqualityComparer<TSecond> secondComparer)
        {
            EnsureTypesDifferent();

            firstToSecond = new Dictionary<TFirst, IList<TSecond>>();
            secondToFirst = new Dictionary<TSecond, IList<TFirst>>(secondComparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeBiDictionary{TFirst, TSecond}"/> class with custom equality comparers for both TFirst and TSecond.
        /// </summary>
        /// <param name="firstComparer">Custom equality comparer for TFirst.</param>
        /// <param name="secondComparer">Custom equality comparer for TSecond.</param>
        public SafeBiDictionary(IEqualityComparer<TFirst> firstComparer, IEqualityComparer<TSecond> secondComparer)
        {
            EnsureTypesDifferent();

            firstToSecond = new Dictionary<TFirst, IList<TSecond>>(firstComparer);
            secondToFirst = new Dictionary<TSecond, IList<TFirst>>(secondComparer);
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
        public IList<TSecond> this[TFirst first]
        {
            get { return GetByFirst(first); }
        }

        public IList<TFirst> this[TSecond second]
        {
            get { return GetBySecond(second); }
        }
        #endregion

        #region Public Methods
        public void Add(TFirst first, TSecond second)
        {
            IList<TFirst> firsts;
            IList<TSecond> seconds;
            if (!firstToSecond.TryGetValue(first, out seconds))
            {
                seconds = new List<TSecond>();
                firstToSecond[first] = seconds;
            }
            if (!secondToFirst.TryGetValue(second, out firsts))
            {
                firsts = new List<TFirst>();
                secondToFirst[second] = firsts;
            }
            seconds.Add(second);
            firsts.Add(first);
        }

        public IList<TSecond> GetByFirst(TFirst first)
        {
            IList<TSecond> list;
            if (!firstToSecond.TryGetValue(first, out list))
            {
                return EmptySecondList;
            }
            return new List<TSecond>(list); // Create a copy for sanity
        }

        public IList<TFirst> GetBySecond(TSecond second)
        {
            IList<TFirst> list;
            if (!secondToFirst.TryGetValue(second, out list))
            {
                return EmptyFirstList;
            }
            return new List<TFirst>(list); // Create a copy for sanity
        }

        public bool ContainsFirst(TFirst first)
        {
            return firstToSecond.ContainsKey(first);
        }

        public bool ContainsSecond(TSecond second)
        {
            return secondToFirst.ContainsKey(second);
        }

        public bool TryGetByFirst(TFirst first, out IList<TSecond> seconds)
        {
            return firstToSecond.TryGetValue(first, out seconds);
        }

        public bool TryGetBySecond(TSecond second, out IList<TFirst> firsts)
        {
            return secondToFirst.TryGetValue(second, out firsts);
        }

        public bool RemoveByFirst(TFirst first)
        {
            foreach (var second in firstToSecond[first])
            {
                secondToFirst[second].Remove(first);
            }

            return firstToSecond.Remove(first);
        }

        public bool RemoveBySecond(TSecond second)
        {
            foreach (var first in secondToFirst[second])
            {
                firstToSecond[first].Remove(second);
            }

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