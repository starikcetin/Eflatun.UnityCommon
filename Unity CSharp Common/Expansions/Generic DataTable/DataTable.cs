using System.Collections;
using System.Collections.Generic;

namespace UnityCSCommon.Expansions
{
    /// <summary>
    /// A fast collection that consists of rows and columns, and cells that corresponds to intersection of these.
    /// </summary>
    /// <typeparam name="TRow">The type of rows.</typeparam>
    /// <typeparam name="TCol">The type of columns.</typeparam>
    /// <typeparam name="TCell">The type of cells (values).</typeparam>
    ///
    /// <remarks>
    /// In this type, we treat all references as values and use integers as keys to access them.
    /// The way this is possible is:
    ///     1- We get hash codes of both row and column.
    ///     2- We use a pairing function to generate a unique number out of two hash codes.
    ///     3- We use the unique number as the key for the entry.
    ///
    /// So, if user didn't make something stupid like overriding the GetHashCode() method with a constant,
    /// we will get the same unique number for the same row and column every time. Thus, the same entry.
    ///
    /// I was using Cantor Pairing at first, but then I discovered Szudzik's Elegant Pairing,
    /// which has better coverage. So I switched to it, but I still keep Cantor Pairing in
    /// it's own method in case I need it.
    /// </remarks>
    public class DataTable<TRow, TCol, TCell> : IEnumerable<DataTableEntry<TRow, TCol, TCell>>
    {
        private static readonly TCell DefaultCellValue = default(TCell);
        private readonly Dictionary<long, DataTableEntry<TRow, TCol, TCell>> _entries = new Dictionary<long, DataTableEntry<TRow, TCol, TCell>>();

        /// <summary>
        /// Sets the intersection of <paramref name="row"/> and <paramref name="col"/> to <paramref name="newCell"/>.
        /// </summary>
        public void Set (TRow row, TCol col, TCell newCell)
        {
            long key = CalculateKey (row, col);
            var newEntry = CreateEntry (row, col, newCell);

            if (_entries.ContainsKey (key))
            {
                _entries[key] = newEntry;
            }
            else
            {
                _entries.Add (key, newEntry);
            }
        }

        /// <summary>
        /// Returns the intersection of <paramref name="row"/> and <paramref name="col"/>.
        /// Returns default value of <typeparamref name="TCell"/> if no entry is found.
        /// </summary>
        public TCell Get (TRow row, TCol col)
        {
            long key = CalculateKey (row, col);
            DataTableEntry<TRow, TCol, TCell> value;

            if (_entries.TryGetValue (key, out value))
            {
                return value.Cell;
            }
            else
            {
                return DefaultCellValue;
            }
        }

        public TCell this [TRow row, TCol col]
        {
            get { return Get (row, col); }
            set { Set (row, col, value); }
        }

        private long CalculateKey (TRow row, TCol col)
        {
            int a = row.GetHashCode();
            int b = col.GetHashCode();

            return SzudzikPairing (a, b);
        }

        private static long SzudzikPairing (int a, int b)
        {
            // Szudzik's Elegant Pairing Function
            return a >= b ? a*a + a + b : b*b + a;
        }

        // Cantor Pairing is no longer in use since Szudzik's Pairing gives better coverage.
        // But I am still keeping it in case we need it later.
        private static long CantorPairing (int a, int b)
        {
            // Cantor Pairing Function
            return (a + b)*(a + b + 1)/2 + b;
        }

        private static DataTableEntry<TRow, TCol, TCell> CreateEntry (TRow row, TCol col, TCell cell)
        {
            return new DataTableEntry<TRow, TCol, TCell> (row, col, cell);
        }

        #region Implementation of IEnumerable
        public IEnumerator<DataTableEntry<TRow, TCol, TCell>> GetEnumerator()
        {
            return _entries.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}