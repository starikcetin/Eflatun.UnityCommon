namespace UnityCSharpCommon.Utils.Common
{
    /// <summary>
    /// Generates a unique integer evertime <see cref="Next"/> method is called unless apllication quits or <seealso cref="Reset"/> method is called.
    /// </summary>
    public static class UniqueIdentityGenerator
    {
        private static int _nextIdentity;

        public static void Reset()
        {
            _nextIdentity = 0;
        }

        public static int Next()
        {
            return _nextIdentity++; //takes value first, then increments.
        }
    }
}