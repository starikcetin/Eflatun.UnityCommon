using System;

namespace UnityCSharpCommon.Utils.BetterRandom
{
    /// <summary>
    /// A better Random class.
    /// </summary>
    public class BetterRandom
    {
        public Random Random { get; private set; }
        public int Seed { get; private set; }

        public BetterRandomIntMethods Int { get; private set; }
        public BetterRandomDoubleMethods Double { get; private set; }
        public BetterRandomFloatMethods Float { get; private set; }
        public BetterRandomVector2Methods Vector2 { get; private set; }
        public BetterRandomVector3Methods Vector3 { get; private set; }

        /// <summary>
        /// Initializes the BetterRandom with the seed "0".
        /// </summary>
        public BetterRandom() : this(0) { }

        /// <summary>
        /// Initialized the BetterRandom with the <see cref="seed"/>.
        /// </summary>
        public BetterRandom(int seed)
        {
            Initialize (seed);
            Int = new BetterRandomIntMethods (this);
            Double = new BetterRandomDoubleMethods (this);
            Float = new BetterRandomFloatMethods (this);
            Vector2 = new BetterRandomVector2Methods (this);
            Vector3 = new BetterRandomVector3Methods (this);
        }

        public void Reset()
        {
            Initialize (Seed);
        }

        public void ChangeSeed (int newSeed)
        {
            Initialize (newSeed);
        }

        private void Initialize (int seed)
        {
            Seed = seed;
            Random = new Random (seed);
        }

        /// <summary>
        /// Returns +1 or -1 randomly.
        /// </summary>
        public int RandomSign()
        {
            return Int.FromRange(0, 2) * 2 -1; //0*2 -1 = -1 | 1*2 -1 = 1
        }
    }
}