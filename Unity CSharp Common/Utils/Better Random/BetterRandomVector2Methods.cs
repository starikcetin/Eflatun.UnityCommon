using System;
using UnityEngine;

namespace UnityCSharpCommon.Utils.BetterRandom
{
    /// <summary>
    /// Vector2 methods for <see cref="BetterRandom"/> class.
    /// </summary>
    public class BetterRandomVector2Methods
    {
        private readonly BetterRandom _parent;

        public BetterRandomVector2Methods (BetterRandom parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// <para> NonNegative range: (0, 0) (inclusive) -> (<see cref="float.MaxValue"/>, <see cref="float.MaxValue"/>) (exclusive) </para>
        /// <para> All range: (<see cref="float.MinValue"/>, <see cref="float.MinValue"/>) (inclusive) -> (<see cref="float.MaxValue"/>, <see cref="float.MaxValue"/>) (exclusive) </para>
        /// </summary>
        public Vector2 Next (RandomRange rangeType)
        {
            var x = _parent.Float.NextUnl(rangeType);
            var y = _parent.Float.NextUnl(rangeType);

            return new Vector2(x, y);
        }

        /// <summary>
        /// <para> NonNegative range: (0, 0) (inclusive) -> <paramref name="max"/> (exclusive) </para>
        /// <para> All range: (<see cref="float.MinValue"/>, <see cref="float.MinValue"/>) (inclusive) -> <paramref name="max"/> (exclusive) </para>
        /// </summary>
        public Vector2 NextMax (Vector2 max, RandomRange rangeType)
        {
            if (rangeType == RandomRange.NonNegative && (max.x < 0 || max.y < 0))
            {
                throw new ArgumentOutOfRangeException ("max", max, "Components of max vector cannot be negative when rangeType is NonNegative.");
            }

            var x = _parent.Float.NextUnlMax (max.x, rangeType);
            var y = _parent.Float.NextUnlMax (max.y, rangeType);

            return new Vector2 (x, y);
        }

        /// <summary>
        /// <para> Range: <paramref name="min"/> (inclusive) -> (<see cref="float.MaxValue"/>, <see cref="float.MaxValue"/>) (exclusive) </para>
        /// </summary>
        public Vector2 NextMin (Vector2 min)
        {
            var x = _parent.Float.NextUnlMin (min.x);
            var y = _parent.Float.NextUnlMin (min.y);

            return new Vector2 (x, y);
        }

        /// <summary>
        /// <para> Range: <paramref name="min"/> (inclusive) -> <paramref name="max"/> (exclusive) </para>
        /// </summary>
        public Vector2 FromRange (Vector2 min, Vector2 max)
        {
            var x = _parent.Float.FromRange (min.x, max.x);
            var y = _parent.Float.FromRange (min.y, max.y);

            return new Vector2 (x, y);
        }
    }
}