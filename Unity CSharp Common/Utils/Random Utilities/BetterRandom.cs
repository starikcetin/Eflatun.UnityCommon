using System.Collections.Generic;
using UnityCSharpCommon.Utils.RandomUtils.UtilityClasses;
using Random = System.Random;

namespace UnityCSharpCommon.Utils.RandomUtils
{
    /// <summary>
    /// A better Random class.
    /// </summary>
    public class BetterRandom
    {
        public Random Random { get; private set; }
        public int Seed { get; private set; }

        public IntMethods Int { get; private set; }
        public DoubleMethods Double { get; private set; }
        public FloatMethods Float { get; private set; }
        public Vector2Methods Vector2 { get; private set; }
        public Vector3Methods Vector3 { get; private set; }
        public AngleMethods Angle { get; private set; }

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
            Int = new IntMethods (this);
            Double = new DoubleMethods (this);
            Float = new FloatMethods (this);
            Vector2 = new Vector2Methods (this);
            Vector3 = new Vector3Methods (this);
            Angle = new AngleMethods(this);
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

        /// <summary>
        /// Returns a random item from the list.
        /// </summary>
        public T RandomItem<T> (IList<T> list)
        {
            int randomIndex = Int.FromRange (0, list.Count);
            return list[randomIndex];
        }
    }
}