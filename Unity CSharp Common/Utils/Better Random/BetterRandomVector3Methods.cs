using System;
using UnityEngine;

namespace UnityCSharpCommon.Utils.BetterRandom
{
    /// <summary>
    /// Vector3 methods for <see cref="BetterRandom"/> class.
    /// </summary>
    public class BetterRandomVector3Methods
    {
        private readonly BetterRandom _parent;

        public BetterRandomVector3Methods (BetterRandom parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// <para> NonNegative range: (0, 0, 0) (inclusive) -> (<see cref="float.MaxValue"/>, <see cref="float.MaxValue"/>, <see cref="float.MaxValue"/>) (exclusive) </para>
        /// <para> All range: (<see cref="float.MinValue"/>, <see cref="float.MinValue"/>, <see cref="float.MinValue"/>) (inclusive) -> (<see cref="float.MaxValue"/>, <see cref="float.MaxValue"/>, <see cref="float.MaxValue"/>) (exclusive) </para>
        /// </summary>
        public Vector3 Next (RandomRange rangeType)
        {
            var x = _parent.Float.NextUnl (rangeType);
            var y = _parent.Float.NextUnl (rangeType);
            var z = _parent.Float.NextUnl (rangeType);

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// <para> NonNegative range: (0, 0, 0) (inclusive) -> <paramref name="max"/> (exclusive) </para>
        /// <para> All range: (<see cref="float.MinValue"/>, <see cref="float.MinValue"/>, <see cref="float.MinValue"/>) (inclusive) -> <paramref name="max"/> (exclusive) </para>
        /// </summary>
        public Vector3 NextMax (Vector3 max, RandomRange rangeType)
        {
            if (rangeType == RandomRange.NonNegative && (max.x < 0 || max.y < 0 || max.z < 0))
            {
                throw new ArgumentOutOfRangeException ("max", max, "Components of max vector cannot be negative when rangeType is NonNegative.");
            }

            var x = _parent.Float.NextUnlMax (max.x, rangeType);
            var y = _parent.Float.NextUnlMax (max.y, rangeType);
            var z = _parent.Float.NextUnlMax (max.z, rangeType);

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// <para> Range: <paramref name="min"/> (inclusive) -> (<see cref="float.MaxValue"/>, <see cref="float.MaxValue"/>, <see cref="float.MaxValue"/>) (exclusive) </para>
        /// </summary>
        public Vector3 NextMin (Vector3 min)
        {
            var x = _parent.Float.NextUnlMin (min.x);
            var y = _parent.Float.NextUnlMin (min.y);
            var z = _parent.Float.NextUnlMin (min.z);

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// <para> Range: <paramref name="min"/> (inclusive) -> <paramref name="max"/> (exclusive) </para>
        /// </summary>
        public Vector3 FromRange (Vector3 min, Vector3 max)
        {
            var x = _parent.Float.FromRange (min.x, max.x);
            var y = _parent.Float.FromRange (min.y, max.y);
            var z = _parent.Float.FromRange (min.z, max.z);

            return new Vector3(x, y, z);
        }
    }
}