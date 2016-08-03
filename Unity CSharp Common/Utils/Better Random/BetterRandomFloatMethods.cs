using System;

namespace UnityCSharpCommon.Utils.BetterRandom
{
    /// <summary>
    /// Float methods for <see cref="BetterRandom"/> class.
    /// </summary>
    public class BetterRandomFloatMethods
    {
        // --- IMPORTANT NOTE ---
        // All implementations of this class is a copy from 'BetterRandomDoubleMethods'.
        // So do the improvments on 'that' class and copy on this one.

        private readonly BetterRandom _parent;

        public BetterRandomFloatMethods (BetterRandom parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// <para> NonNegative range: 0 (inclusive) -> +1 (exclusive) </para>
        /// <para> All range: -1 (exclusive) -> +1 (exclusive) </para>
        /// </summary>
        public float Next01 (RandomRange rangeType)
        {
            return (float)_parent.Double.Next01(rangeType);
        }

        /// <summary>
        /// <para> Range: <paramref name="min"/> (inclusive) -> +1 (exclusive) </para>
        /// </summary>
        public float Next01Min (float min)
        {
            // In this method we don't need to specify the sign type since user decides the "lower boundary".
            // Which means, if the "min" is greater than or equal to 0, the return will be NonNegative.
            // Otherwise it can be both negative and positive (or 0), which is what RandomRange.All means.

            if (min <= -1f || min >= 1f)
            {
                throw new ArgumentOutOfRangeException ("min", min, "The 'min' parameter for this method must be in range of -1 (exclusive) and +1 (exclusive).");
            }

            return FromRange (min, 1f);
        }

        /// <summary>
        /// <para> NonNegative range: 0 (inclusive) -> <paramref name="max"/> (exclusive) </para>
        /// <para> All range: -1 (exclusive) -> <paramref name="max"/> (exclusive) </para>
        /// </summary>
        public float Next01Max (float max, RandomRange rangeType)
        {
            switch (rangeType)
            {
                case RandomRange.NonNegative:
                    if (max <= 0f || max > 1f)
                    {
                        throw new ArgumentOutOfRangeException("max", max, "The 'max' parameter for this method must be in range of 0 (exclusive) and +1 (inclusive) when the 'rangeType' parameter is 'NonNegative'.");
                    }

                    return FromRange(0f, max);

                case RandomRange.All:
                    if (max <= -1f || max > 1f)
                    {
                        throw new ArgumentOutOfRangeException ("max", max, "The 'max' parameter for this method must be in range of -1 (exclusive) and +1 (inclusive) when the 'rangeType' parameter is 'All'.");
                    }

                    var dist = max + 1;                                         //the distance between max and -1.
                    var syMax = dist / 2;                                       //the new max value that makes symetrical '-syMax 0 +syMax' range.
                    var syRnd = FromRange(0f, syMax) * _parent.RandomSign();    //a random in range '-syMax -> +syMax' (both exclusive).
                    var diff = syMax - max;                                     //difference between original max and syMax.
                    return syRnd - diff;                                        //remove the difference from syRnd so we get the random number in original range.

                default:
                    throw new ArgumentOutOfRangeException ("rangeType", rangeType, null);
            }
        }

        /// <summary>
        /// <para> NonNegative range: 0 (inclusive) -> <see cref="float.MaxValue"/> (exclusive) </para>
        /// <para> All range: <see cref="float.MinValue"/> (inclusive) -> <see cref="float.MaxValue"/> (exclusive) </para>
        /// </summary>
        public float NextUnl (RandomRange rangeType)
        {
            switch (rangeType)
            {
                case RandomRange.NonNegative:
                    return FromRange (0f, float.MaxValue);

                case RandomRange.All:
                    return FromRange (float.MinValue, float.MaxValue);

                default:
                    throw new ArgumentOutOfRangeException ("rangeType", rangeType, null);
            }
        }

        /// <summary>
        /// <para> Range: <paramref name="min"/> (inclusive) -> <see cref="float.MaxValue"/> (exclusive) </para>
        /// </summary>
        public float NextUnlMin (float min)
        {
            // In this method we don't need to specify the sign type since user decides the "lower boundary".
            // Which means, if the "min" is greater than or equal to 0, the return will be NonNegative.
            // Otherwise it can be both negative and positive (or 0), which is what RandomRange.All means.

            return FromRange (min, float.MaxValue);
        }

        /// <summary>
        /// <para> NonNegative range: 0 (inclusive) -> <paramref name="max"/> (exclusive) </para>
        /// <para> All range: <see cref="float.MinValue"/> (inclusive) -> <paramref name="max"/> (exclusive) </para>
        /// </summary>
        public float NextUnlMax (float max, RandomRange rangeType)
        {
            switch (rangeType)
            {
                case RandomRange.NonNegative:
                    if (max < 0f)
                    {
                        throw new ArgumentOutOfRangeException ("max", max, "The 'max' parameter cannot be negative when the 'rangeType' parameter is 'NonNegative'.");
                    }

                    return FromRange (0f, max);

                case RandomRange.All:
                    return FromRange (float.MinValue, max);

                default:
                    throw new ArgumentOutOfRangeException ("rangeType", rangeType, null);
            }
        }

        /// <summary>
        /// <para> Range: <paramref name="min"/> (inclusive) -> <paramref name="max"/> (exclusive) </para>
        /// </summary>
        public float FromRange (float min, float max)
        {
            return Next01 (RandomRange.NonNegative) * (max - min) + min;
        }
    }
}